using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRG_Heart : TRG
{
    public int amount = 1;
    public override void trigger(Actor actor)
    {
        if (actor is Player)
        {
            ((Player)actor).addHealthPoint(amount);
            Destroy(gameObject);
        }
    }
}
