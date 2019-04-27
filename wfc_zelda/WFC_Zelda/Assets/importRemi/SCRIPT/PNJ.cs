using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PNJ : Actor
{
    public NavMeshAgent agent;

    void Start()
    {
        
        agent.SetDestination(new Vector3(0, 0, 4));
    }

    void Update()
    {
        if (agent.path.status == NavMeshPathStatus.PathComplete)
        {
            for (int i = 0; i < agent.path.corners.Length - 1; i++) Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.cyan);
        }
        else if (agent.path.status == NavMeshPathStatus.PathPartial)
        {
            for (int i = 0; i < agent.path.corners.Length - 1; i++) Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red);
        }
    }


}
