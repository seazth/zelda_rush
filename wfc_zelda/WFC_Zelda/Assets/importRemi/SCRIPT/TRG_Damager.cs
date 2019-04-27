using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRG_Damager : TRG
{
    public int amount = 1;
    public float pushFactor = 1;
    public float pushVecY = 4;
    public override void trigger(Actor actor)
    {
        Vector3 f = actor.transform.position - transform.position;
        f.y += pushVecY;
        actor.takeHit(amount);

        if (actor is Player)
        {
            ((Player)actor).bdy.velocity += (f * pushFactor);
        }
        if (actor is PNJ)
        {
            ((PNJ)actor).agent.velocity += (f * pushFactor);
        }
    }
}
