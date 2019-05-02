using System.Collections;
using System.Collections.Generic;
//using UnityEditor.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MNG_Game : MonoBehaviour
{
    public static MNG_Game instance { get; private set; }
    public NavBuilder navbuilder;
    private void Awake()
    {
        inPause = true;
        instance = this;
        pnl_mainmenu.SetActive(true);

        print("MNG_Game : LOADED");


        Cursor.SetCursor(tex_cursor, Vector2.zero, CursorMode.Auto);
    }

    public void InitNavMesh()
    {
        Invoke("BuildNav", 1f);
    }
    public void BuildNav()
    {
        print("MNG_Game : NAVMESH - INIT");
        Debug.Log("Build " + Time.realtimeSinceStartup.ToString());
        navbuilder.Build();
        Debug.Log("Build finished " + Time.realtimeSinceStartup.ToString());
        Debug.Log("Update " + Time.realtimeSinceStartup.ToString());
        navbuilder.UpdateNavmeshData();
        print("MNG_Game : NAVMESH - DONE");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            pnl_help.SetActive(!pnl_help.activeInHierarchy);
        }
    }
    public void sendCommand(string cmd)
    {
        Debug.Log("[CMD] " + cmd);
        switch (cmd)
        {
            case "BTN_RTRNJEU":
                setPause(false);
                break;
            case "BTN_RTRNMENU":
                pnl_ingamemenu.SetActive(false);
                pnl_mainmenu.SetActive(true);
                break;
            case "BTN_QUITGAME":
                // save any game data here
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                break;

            case "BTN_STRTGAME":
                StartNewGame();
                pnl_mainmenu.SetActive(false);
                break;
            case "BTN_DEATHRETURNMAIN":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
                /*
                pnl_mainmenu.SetActive(true);
                pnl_gameover.SetActive(false);
                while (cnt_main.transform.childCount > 0)
                {
                    GameObject go = cnt_main.transform.GetChild(0).gameObject;
                    go.transform.SetParent(null);
                    Destroy(go);
                }*/
                break;
            default:
                print(">> Commande inconnu");
                break;
        }
    }

    public GameObject fx_deathSmoke;
    public GameObject fx_hitPoc;
    public GameObject fx_hitBounce;
    public GameObject pnl_gameover;
    public GameObject pnl_winner;
    public GameObject pnl_ingamemenu; // ◄◄ NEW
    public GameObject pnl_mainmenu;
    public GameObject pnl_help;
    public UI_PlayerHUD hud;


    //▼▼▼ NEW ▼▼▼

    public bool inPause { get; private set; }
    public void setPause(bool value)
    {
        inPause = value;
        if (value)
        {
            Time.timeScale = 0.001f;
            pnl_ingamemenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pnl_ingamemenu.SetActive(false);
        }
    }

    public Texture2D tex_cursor;

    public GameObject gop_rubee;
    public GameObject gop_rubeeSilver;
    public GameObject gop_rubeeGold;
    public GameObject gop_heart;
    public GameObject gop_hearthalf;
    public GameObject gop_megaHeart;
    public GameObject gop_bomb;

    public void StartNewGame()
    {
        print("IMPLEMENT NEW GAME HERE");
        spwn_player.Spawn();
        setPause(false);
        for (int i = 0; i < spwn_enemies.Length; i++) spwn_enemies[i].Spawn();
        for (int i = 0; i < spwn_objects.Length; i++) spwn_objects[i].Spawn();
    }

    public GameObject cnt_main;

    public SPAWNER spwn_player;
    public SPAWNER[] spwn_enemies;
    public SPAWNER[] spwn_objects;
}
