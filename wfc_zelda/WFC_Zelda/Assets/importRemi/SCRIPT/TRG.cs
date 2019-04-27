using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TRG_I
{
    void trigger(Actor actor);
}

public abstract class TRG : MonoBehaviour, TRG_I
{
    public abstract void trigger(Actor actor);
}
