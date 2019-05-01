using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PNJ : Actor
{
    NavMeshAgent agent;
    Coroutine moveRecovery;
    public bool flag_botIsActive = true;
    public bool flag_canMove = true;
    public float recalculatePathPerSecond = 1f;
    public bool flag_isRunning = false;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        activePath = new NavMeshPath();
        //Invoke("recalculatePath",recalculatePathPerSecond);
    }
    protected virtual void Update()
    {
        drawCurrentPath();
        updatePathFollow();
    }
    public virtual void hasDedectedPlayer(Player actor)
    {
        print("Player detected !");
    }
    public override void takeHit(int amount)
    {
        base.takeHit(amount);
        flag_canMove = false;
        moveRecovery = StartCoroutine(ActionTimer(() => { flag_canMove = true; }, 1f));
    }
    public override void onDeath()
    {
        base.onDeath();
        Destroy(gameObject);
    }

    //PathFollow Remap ------------------------------------------------------------//
    NavMeshPath activePath;
    public Vector3 destination { get; private set; }
    public bool flag_canFollowPath = true;
    public bool flag_naviguationIsActive = true;
    public bool followPath = true;
    public bool turnToward = true;
    int currentNode = 0;
    public float stoppingDistance { get { return agent.stoppingDistance; } }
    public void recalculatePath()
    {
        calculatePath();
        Invoke("recalculatePath", recalculatePathPerSecond);
    }
    public void calculatePath()
    {
        if (destination != null)
        {
            print("BOT : CALCULATE PATH");
            activePath = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, activePath);
            currentNode = 0;
            flag_canFollowPath = (activePath.status != NavMeshPathStatus.PathInvalid);
        }
        else
        {
            flag_canFollowPath = false;
        }

    }
    public void SetNewNaviguation(Vector3 position)
    {
        print(name + " : New destination (" + position + ")");
        destination = position;
        calculatePath();
    }
    void drawCurrentPath()
    {
        if (activePath.status == NavMeshPathStatus.PathComplete)
        {
            for (int i = 0; i < activePath.corners.Length - 1; i++) Debug.DrawLine(activePath.corners[i], activePath.corners[i + 1], Color.magenta);
        }
        else if (activePath.status == NavMeshPathStatus.PathPartial)
        {
            print("DEBUG : activePath.status == NavMeshPathStatus.PathPartial");
            for (int i = 0; i < activePath.corners.Length - 1; i++) Debug.DrawLine(activePath.corners[i], activePath.corners[i + 1], Color.red);
        }
        else
        {
            print("DEBUG : activePath.status == NavMeshPathStatus.Invalid");
        }
    }
    void updatePathFollow()
    {
        flag_isRunning = false;
        if (!flag_canFollowPath || !flag_canMove) return;
        if (getDistanceRemain() < agent.stoppingDistance) return;
        else
        {
            flag_isRunning = true;
            Vector3 direction;
            if (currentNode == activePath.corners.Length)
            {
                direction = (destination - transform.position).normalized;
                direction.y = 0;
                transform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                direction = (activePath.corners[currentNode] - transform.position).normalized;
                direction.y = 0;
                transform.position += (direction * speed) * Time.deltaTime;
                if ((activePath.corners[currentNode] - transform.position).magnitude < agent.stoppingDistance)
                    currentNode++;
            }
            Quaternion dirRotation = Quaternion.FromToRotation(transform.forward, direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation*dirRotation, turnTowardSpeed);

        }
    }
    public float getDistanceRemain()
    {
        if (!flag_canFollowPath) return int.MaxValue;
        if (activePath.corners.Length == 0) return (transform.position - destination).magnitude;

        float res = 0;
        for (int i = Mathf.Max(1, currentNode); i < activePath.corners.Length; i++)
        {
            if (i == 0) res += (activePath.corners[i] - transform.position).magnitude;
            else res += (activePath.corners[i] - activePath.corners[i - 1]).magnitude;
        }
        res += (destination - activePath.corners[activePath.corners.Length - 1]).magnitude;
        return res;
    }

}



public class PNJ_Soldier : PNJ
{
    public int state = 1; // 1=seeking 2=moving
    public Coroutine coroutineState;
    public GameObject head;
    public AANIM_Humanoid aanim;
    public float statePause = 1;
    protected override void Awake()
    {
        base.Awake();
        if (aanim == null) aanim = GetComponent<AANIM_Humanoid>();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if (flag_isRunning) aanim.state = AANIM_Humanoid.MyEnum.running;
        else aanim.state = AANIM_Humanoid.MyEnum.idle;

        if (!flag_botIsActive) return;
        if (coroutineState == null)
        {
            if (state == 0) // MODIFICATION DE RETOUR A ZERO
            {
                state = 1;
            }
            if (state == 1)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)//
                {
                    print("SeekWithHead");
                    coroutineState = StartCoroutine(SeekWithHead(UnityEngine.Random.Range(0, 3)));
                }
                else
                {
                    print("goToRandomPosition");
                    coroutineState = StartCoroutine(goToRandomDirection(UnityEngine.Random.Range(0, 4), UnityEngine.Random.Range(4, 10)));
                }
            }

        }
    }
    public override void hasDedectedPlayer(Player actor)
    {
        base.hasDedectedPlayer(actor);
        if (state == 1)
        {
            state = 2;
            if (coroutineState != null) StopCoroutine(coroutineState);
            print("runOnPlayerPosition");
            coroutineState = StartCoroutine(runToPosition(actor.transform.position));
        }
    }
    public override void takeHit(int amount) // MODIFICATION DE RETOUR A ZERO
    {
        base.takeHit(amount);
        state = 0;
    }

    public float speedHeadRotation = 0.1f;
    public IEnumerator SeekWithHead(int direction)
    {
        float headRotationTarget;
        float diffAngle = 999;
        switch (direction) // direction : 0=devant 1=droite 2=derriere 3=gauche 
        {
            case 0: headRotationTarget = 0; break;
            case 1: headRotationTarget = 90; break;
            default: headRotationTarget = 270; break;
        }

        Quaternion dest = Quaternion.Euler(0, headRotationTarget, 0);
        //print("headRotationTarget= " + headRotationTarget);
        diffAngle = Mathf.Abs(head.transform.localRotation.eulerAngles.y - headRotationTarget);

        while (state == 1 && diffAngle > 1)
        {
            head.transform.localRotation = Quaternion.Lerp(head.transform.localRotation, dest, speedHeadRotation);
            diffAngle = Mathf.Abs(head.transform.localRotation.eulerAngles.y - headRotationTarget);
            yield return null;
        }

        yield return new WaitForSeconds(statePause);
        //END STATE
        print("END STATE");
        coroutineState = null;
        /*
         
         */
    }
    public IEnumerator goToRandomDirection(int direction, int distance)
    {
        Quaternion dest = Quaternion.Euler(0, 0, 1);
        Vector3 newPosition = transform.position;
        switch (direction) // direction : 0=devant 1=droite 2=derriere 3=gauche 
        {
            case 0: newPosition += new Vector3(0, 0, distance); break;
            case 1: newPosition += new Vector3(distance, 0, 0); break;
            case 2: newPosition += new Vector3(0, 0, -distance); break;
            default: newPosition += new Vector3(-distance, 0, 0); break;
        }
        SetNewNaviguation(newPosition);
        //print("newPosition= " + newPosition);


        while (state == 1 && flag_canFollowPath)
        {
            head.transform.localRotation = Quaternion.Lerp(head.transform.localRotation, dest, speedHeadRotation);
            if (getDistanceRemain() <= stoppingDistance) break;
            yield return null;
        }

        //END STATE
        if (flag_canFollowPath) yield return new WaitForSeconds(statePause);
        print("END STATE");
        coroutineState = null;
    }
    public IEnumerator runToPosition(Vector3 position)
    {
        Quaternion dest = Quaternion.Euler(0, 0, 1);
        Vector3 newPosition = position;
        SetNewNaviguation(newPosition);
        //print("newPosition= " + newPosition);

        while (state == 2 && flag_canFollowPath)
        {
            head.transform.localRotation = Quaternion.Lerp(head.transform.localRotation, dest, speedHeadRotation);
            if (getDistanceRemain() <= stoppingDistance) break;
            yield return null;
        }


        state = 1;
        //END STATE
        yield return new WaitForSeconds(statePause);
        print("END STATE");
        coroutineState = null;
    }
}