using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJ_Soldier : PNJ
{
    public int state = 1; // 1=seeking 2=moving
    public Coroutine coroutineState;
    public GameObject head;
    public AANIM_Humanoid aanim;
    public float statePause = 1;
    public float sprintSpeed = 15;
    public float normalSpeed = 8;
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
                speed = normalSpeed;
                if (UnityEngine.Random.Range(0, 2) == 0)//
                {
                    print("SeekWithHead");
                    coroutineState = StartCoroutine(SeekWithHead(UnityEngine.Random.Range(0, 3)));
                }
                else
                {
                    print("goToRandomPosition");
                    coroutineState = StartCoroutine(goToRandomDirection(UnityEngine.Random.Range(0, 4), UnityEngine.Random.Range(4, 15)));
                }
            }
            else if(state == 2)
            {
                speed = sprintSpeed;
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