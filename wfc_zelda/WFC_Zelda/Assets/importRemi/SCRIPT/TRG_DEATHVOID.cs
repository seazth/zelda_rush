using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRG_DEATHVOID : TRG
{
    public override void trigger(Actor actor, Collider other)
    {
        if (actor is Player)
        {
            ((Player)actor).RespawnToLastValidPosition();
        }
    }
}