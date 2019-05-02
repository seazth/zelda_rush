using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRG_Damager : TRG
{
    public int amount = 1;
    public float pushFactor = 1;
    public float push_Y = 0.1f;
    //[EnumFlagsAttribute]
    public Team Team_Damageable = Team.Monster ^ Team.Neutral ^ Team.Player;
    //public Actor ActorAppartenance;

    public bool isCounterable;
    public override void trigger(Actor actor, Collider other)
    {
        //print(other.attachedRigidbody.name);
        if (true)//Team_Damageable.Equals(actor.TeamEnum)
        {
            //GameObject go = Instantiate(MNG_Game.instance.fx_hitBounce, transform.position, Quaternion.identity);
            //

            Vector3 f = (actor.transform.position - transform.position).normalized;
            f.y = push_Y;
            actor.takeHit(amount);
            actor.bdy.AddForce(f * pushFactor, ForceMode.Impulse);
        }
    }
}
