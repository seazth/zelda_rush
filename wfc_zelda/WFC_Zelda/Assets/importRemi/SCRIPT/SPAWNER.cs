using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAWNER : MonoBehaviour
{
    public Team actorTeam;
    public GameObject actorPrefab;
    public bool spawn_onStart = false;
    public Vector3 offsetPosition;
    public void Start()
    {
        if (spawn_onStart) Spawn();
    }
    public void Spawn()
    {
        print("[SPAWN] " + actorPrefab.name);
        Actor actor = Instantiate(actorPrefab, transform).GetComponent<Actor>();
        actor.TeamEnum = actorTeam;
    }
}
