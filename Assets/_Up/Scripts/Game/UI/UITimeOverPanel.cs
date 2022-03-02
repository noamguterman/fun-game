using Assets._Scripts.Tools;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class UITimeOverPanel : UIPanel
{
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] private TextMeshProUGUI percentCompleted;
    [SerializeField] private TextMeshProUGUI needCoin;
    [SerializeField] private TextMeshProUGUI timer;

    [SerializeField] private Button reviveButton;
    [SerializeField] private Button skipButton;

    private void Awake()
    {
        reviveButton.onClick.AddListener(OnReviveButtonPressed);
        skipButton.onClick.AddListener(OnSkipButtonPressed);
    }

    public void Show(float percent = 0)
    {
        SetActive(true);

        if (GameManager.Mode == GameMode.Levels)
        {
            if (GameData.CurrentScore == GameData.BestScore)
            {
                currentScore.text = "New Highscore!";
            }
            else
            {
                currentScore.text = GameData.CurrentScore.ToString();
            }

            bestScore.text = GameData.BestScore.ToString();
            percentCompleted.text = string.Format("{0}% completed", (int)(percent * 100));
            timer.text = GameManager.Instance.LevelTime.ToString();
            needCoin.text = NeedCoinToIncreaseTime().ToString();
        }
    }

    private void ActivateSkipButton()
    {
        skipButton.gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnReviveButtonPressed()
    {
        if(GameData.Coins >= NeedCoinToIncreaseTime())
        {
            GameData.Coins -= NeedCoinToIncreaseTime();
            GameManager.Instance.LevelTime += 1;
            timer.text = GameManager.Instance.LevelTime.ToString();
            SoundManager.Instance.PlayButtonSFX();
            needCoin.text = NeedCoinToIncreaseTime().ToString();
        }
    }

    private int NeedCoinToIncreaseTime()
    {
        int needCoin = 0;
        needCoin = ((GameManager.Instance.LevelTime + 1)- 8) * 50;
        return needCoin;
    }

    private void OnSkipButtonPressed()
    {
        GameData.CurrentScore = 0;

        SoundManager.Instance.PlayButtonSFX();

        SceneManager.LoadScene("Game");
    }

   
}