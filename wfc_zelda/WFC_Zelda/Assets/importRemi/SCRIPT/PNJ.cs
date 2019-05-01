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
        destination = transform.position;
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


