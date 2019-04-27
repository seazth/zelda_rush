using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MNG_Game : MonoBehaviour
{
    public static MNG_Game instance { get; private set; }
    private void Awake()
    {
        instance = this;
        print("MNG_Game : LOADED");
    }

    public GameObject fx_deathSmoke;
    public GameObject fx_hitPoc;
    public GameObject pnl_gameover;
    public GameObject pnl_menu;
    public GameObject go_uicursor;
    public UI_PlayerHUD hud;

}
