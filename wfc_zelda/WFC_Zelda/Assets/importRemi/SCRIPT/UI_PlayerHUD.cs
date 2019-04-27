using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHUD : MonoBehaviour
{
    public Image[] Lifes;
    public Sprite[] Sprites;
    public Text txt_rubys;
    public Text txt_bombs;

    Color opaque = new Color(255, 255, 255, 255);
    Color transparent = new Color(255, 255, 255, 0);

    public void Refresh_Health(int lifePts, int maxLife)
    {
        int _lp = lifePts;
        for (int i = 0; i < Lifes.Length; i++)
        {

            if (i >= maxLife)
            {
                Lifes[i].color = transparent;
            }
                
            else if (_lp >= 2)
            {
                Lifes[i].sprite = Sprites[0];
                _lp -= 2;
                Lifes[i].color = opaque;
            }
            else if (_lp >= 1)
            {
                Lifes[i].sprite = Sprites[1];
                _lp -= 2;
                Lifes[i].color = opaque;
            }
            else
            {
                Lifes[i].sprite = Sprites[2];
                Lifes[i].color = opaque;
            }
        }
    }

    public void Refresh_Bomb(int amount)
    {
        txt_bombs.text = amount.ToString();
    }
    public void Refresh_Rubys(int amount)
    {
        txt_rubys.text = amount.ToString();
    }
}
