using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRG_Vision : TRG
{
    public PNJ PNJ;
    public LayerMask layer;
    public override void trigger(Actor actor, Collider other)
    {
        if (actor is Player)
        {
            RaycastHit hit;
            Vector3 dir = actor.transform.position - PNJ.transform.position;
            Ray ray = new Ray(PNJ.transform.position + new Vector3(0, 1, 0), dir);
            print("inZoneDetext");
            if (Physics.Raycast(ray, out hit, 40, layer))
            {
                Debug.DrawRay(PNJ.transform.position + new Vector3(0, 1, 0), dir, Color.cyan, 100);
                print("++++++++ "+hit.rigidbody);
                if (hit.rigidbody != null && hit.rigidbody == actor.bdy)
                    PNJ.hasDedectedPlayer((Player)actor);
            }

        }
    }
}
