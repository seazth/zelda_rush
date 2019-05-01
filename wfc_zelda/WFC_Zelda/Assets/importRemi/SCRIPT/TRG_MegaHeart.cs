using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRG_MegaHeart : TRG
{
    public override void trigger(Actor actor, Collider other)
    {
        if (actor is Player)
        {
             if (((Player)actor).addHealthMax(1)) Destroy(gameObject);
        }
    }
}
