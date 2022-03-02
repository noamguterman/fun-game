using Assets._Scripts.Tools;
using System;
using UnityEngine;

public static class GameData
{
    public static event Action<int> CoinsAmountChanged;
    public static event Action<int> BestScoreChanged;
    public static event Action<int> BestEndlessScoreChanged;
    public static event Action<int> CurrentScoreChanged;

    public static int Coins
    {
        get
        {
            return PlayerPrefs.GetInt("Coins");
        }
        set
        {
            PlayerPrefs.SetInt("Coins", value);
            CoinsAmountChanged.SaveInvoke(value);
        }
    }

    public static int BestScore
    {
        get
        {
            return PlayerPrefs.GetInt("BestScore");
        }
        set
        {
            PlayerPrefs.SetInt("BestScore", value);

            BestScoreChanged.SaveInvoke(value);
        }
    }

    public static int BestEndlessScore
    {
        get
        {
            return PlayerPrefs.GetInt("BestEndlessScore");
        }
        set
        {
            PlayerPrefs.SetInt("BestEndlessScore", value);

            BestEndlessScoreChanged.SaveInvoke(value);
        }
    }

    public static int CurrentScore
    {
        get
        {
            return currentScore;
        }
        set
        {
            currentScore = value;

            CurrentScoreChanged.SaveInvoke(value);

            if (GameManager.Mode == GameMode.Levels)
            {
                if (currentScore > BestScore)
                {
                    BestScore = currentScore;
                }
            }
            else if (GameManager.Mode == GameMode.Endless)
            {
                if (currentScore > BestEndlessScore)
                {
                    BestEndlessScore = currentScore;
                }
            }
        }
    }

    private static int currentScore;
}