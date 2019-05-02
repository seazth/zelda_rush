using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAWNER : MonoBehaviour
{
    public GameObject actorPrefab;
    public bool spawn_onStart = false;
    public Vector3 offsetPosition;
    public void Start()
    {
        MNG_Game.instance.spwn_objects.Add(this);
        if (spawn_onStart) Spawn();
    }
    public void Spawn()
    {
        print("[SPAWN] " + actorPrefab.name);
        GameObject go = Instantiate(actorPrefab,transform.position,Quaternion.identity, MNG_Game.instance.cnt_main.transform);
    }
}
