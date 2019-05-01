using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TRG_Bomb : TRG
{
    public int amount = 1;
    public TextMesh txt_amount;

    public void Start()
    {
        if (amount>1) txt_amount.text = amount.ToString();
        else txt_amount.text = "";

    }
    public override void trigger(Actor actor, Collider other)
    {
        if (actor is Player)
        {
            ((Player)actor).takeBomb(amount);
            Destroy(gameObject);
        }
    }
}
