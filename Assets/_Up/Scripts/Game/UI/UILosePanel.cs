using Assets._Scripts.Tools;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class UILosePanel : UIPanel
{
    public event Action ReviveButtonPressed;

    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] private TextMeshProUGUI percentCompleted;
    [SerializeField] private TextMeshProUGUI timer;


    //[SerializeField] private Button gemsButton;
    [SerializeField] private Button reviveButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button randomSkinsButton;

    public bool IsRandomActive
    {
        get
        {
            if (!PlayerPrefs.HasKey("IsRandomActive"))
            {
                return false;
            }

            return PlayerPrefs.GetInt("IsRandomActive") == 1;
        }

        private set
        {
            if (value)
            {
                PlayerPrefs.SetInt("IsRandomActive", 1);
            }
            else
            {
                PlayerPrefs.DeleteKey("IsRandomActive");
            }
        }
    }

    private void Awake()
    {
        //gemsButton.onClick.AddListener(OnGemsButtonPressed);
        reviveButton.onClick.AddListener(OnReviveButtonPressed);
        skipButton.onClick.AddListener(OnSkipButtonPressed);
        randomSkinsButton.onClick.AddListener(OnRandomSkinsPressed);
    }

    public void OnPlayerJump()
    {
        if (IsRandomActive)
        {
            var availableSkins = ShopManager.Instance.GetAvailableSkins();

            if (availableSkins.Count > 1)
            {
                var randomSkin = availableSkins[UnityEngine.Random.Range(0, availableSkins.Count)];

                while (randomSkin.IsSelected)
                {
                    randomSkin = availableSkins[UnityEngine.Random.Range(0, availableSkins.Count)];
                }

                randomSkin.IsSelected = true;
            }
        }
    }

    public void Show(float percent = 0)
    {
        IsRandomActive = false;

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

            //skipButton.gameObject.SetActive(false);

            //Invoke("ActivateSkipButton", 1);
        }
        else if (GameManager.Mode == GameMode.Endless)
        {
            if (GameData.CurrentScore == GameData.BestEndlessScore)
            {
                currentScore.text = "New Highscore!";
            }
            else
            {
                currentScore.text = GameData.BestEndlessScore.ToString();
            }

            bestScore.text = GameData.BestEndlessScore.ToString();
            percentCompleted.gameObject.SetActive(false);
            //skipButton.gameObject.SetActive(false);

            //Invoke("ActivateSkipButton", 1);
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

    private void OnGemsButtonPressed()
    {
        AdsManager.Instance.ShowRewardedVideo(GlobalConstants.Gems);
    }

    private void OnReviveButtonPressed()
    {
        AdsManager.Instance.ShowInterstitial(GlobalConstants.Revive);
    }

    private void OnRandomSkinsPressed()
    {
        AdsManager.Instance.ShowInterstitial(GlobalConstants.RandomSkinFeature);
    }

    private void OnSkipButtonPressed()
    {
        GameData.CurrentScore = 0;

        SoundManager.Instance.PlayButtonSFX();

        SceneManager.LoadScene("Game");
    }

    private void OnRVAvailabilityChanged(bool isReady)
    {
        RVButton(isReady);
    }

    private void RVButton(bool isEnabled)
    {
        reviveButton.gameObject.SetActive(isEnabled);
        //gemsButton.interactable = isEnabled;
    }

    public void OnRVRewardReceived(string key)
    {
        if (key.Equals(GlobalConstants.Revive))
        {
            Time.timeScale = 1;

            Debug.Log("~~~" + GlobalConstants.Revive);
            SoundManager.Instance.PlayButtonSFX();

            Hide();

            ReviveButtonPressed.SaveInvoke();
        }
        else if (key.Equals(GlobalConstants.Gems))
        {
            Time.timeScale = 1;

            Debug.Log("~~~" + GlobalConstants.Gems);
            SoundManager.Instance.PlayButtonSFX();

            GameData.Coins += GameConfig.WATCH_AD_COINS_REWARDED;
            GameData.CurrentScore = 0;

            SceneManager.LoadScene("Game");
        }
        else if (key.Equals(GlobalConstants.RandomSkinFeature))
        {
            Time.timeScale = 1;
            Debug.Log("~~~" + GlobalConstants.RandomSkinFeature);
            SoundManager.Instance.PlayButtonSFX();

            IsRandomActive = true;
            SceneManager.LoadScene("Game");
        }
    }
}