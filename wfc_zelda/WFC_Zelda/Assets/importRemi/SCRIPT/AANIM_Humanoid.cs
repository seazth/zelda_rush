using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AANIM : MonoBehaviour
{
    public Actor actor;
    public Animator anim;
    public const string statename = "AnimState";

    void Start()
    {
        if (actor == null) actor = gameObject.GetComponent<Actor>();
        if (anim == null) anim = GetComponentInChildren<Animator>();
    }
}

public class AANIM_Humanoid : AANIM
{
    public enum MyEnum { idle,running,attack }
    public MyEnum state;
    public void Update()
    {
        switch (state)
        {
            case MyEnum.idle:
                SetAnimState(0);
                break;
            case MyEnum.running:
                SetAnimState(1);
                break;
            case MyEnum.attack:
                SetAnimState(2);
                break;
            default:
                break;
        }
    }
    public int GetAnimState()
    {
        return anim.GetInteger(statename);
    }
    public void SetAnimState(int value)
    {
        anim.SetInteger(statename, value);
    }
}
