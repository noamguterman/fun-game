using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashUI : MonoBehaviour
{
    private Text txt;
    private int Coins;

    private ScoreManager sM;

    void Start()
    {
        txt = this.gameObject.GetComponent<Text>();
        Coins = ScoreManager.Instance.Cash;
    }


    void Update()
    {
        txt.text = "" + Coins;

        if (Coins < ScoreManager.Instance.Cash)
        {
            //SoundManager.Instance.PlayMoneySound();

            //If 10 or less
            if ((Coins < ScoreManager.Instance.Cash) && ((Coins + 10) > ScoreManager.Instance.Cash))
            {
                Coins += 1;
            }
            //If more than 10
            else if (((Coins + 10) <= ScoreManager.Instance.Cash) && (Coins + 100 > ScoreManager.Instance.Cash))
            {
                Coins += 5;
            }
            //If more than 100
            else if (((Coins + 100) <= ScoreManager.Instance.Cash) && (Coins + 1000 > ScoreManager.Instance.Cash))
            {
                Coins += 20;
            }
            //If more than 1000
            else if (((Coins + 1000) <= ScoreManager.Instance.Cash) && (Coins + 10000 > ScoreManager.Instance.Cash))
            {
                Coins += 100;
            }
            //If more than 10000
            else if (((Coins + 10000) <= ScoreManager.Instance.Cash) && (Coins + 100000 > ScoreManager.Instance.Cash))
            {
                Coins += 1000;
            }
            //If more than 10000
            else if (((Coins + 100000) <= ScoreManager.Instance.Cash) && (Coins + 1000000 > ScoreManager.Instance.Cash))
            {
                Coins += 10000;
            }
            //If more than 10000
            else if (((Coins + 100000) <= ScoreManager.Instance.Cash) && (Coins + 1000000 > ScoreManager.Instance.Cash))
            {
                Coins += 10000;
            }
        }
        else if (Coins >= ScoreManager.Instance.Cash)
        {
            Coins = ScoreManager.Instance.Cash;

            //SoundManager.instance.StopMoneySound();
        }

    }
}
