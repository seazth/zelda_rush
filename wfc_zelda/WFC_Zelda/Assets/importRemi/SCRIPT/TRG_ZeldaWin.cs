using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRG_ZeldaWin : TRG
{
    public override void trigger(Actor actor, Collider other)
    {
        if (actor is Player)
        {
            MNG_Game.instance.pnl_winner.SetActive(true);
        }
    }
}
