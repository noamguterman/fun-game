using Assets._Scripts.Tools;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePanel : UIPanel
{
    public event Action SettingsButtonPressed;
    public event Action ShopButtonPressed;

    public UIScore Score { get; private set; }
    public UIProgressBar ProgressBar { get; private set; }
    public UITimer TimerBar { get; private set; }
    public UICoinsCounter CoinsCounter { get; private set; }
    public UISettings Settings { get; private set; }

    [SerializeField] public GameObject bottomPanel;
    [SerializeField] public GameObject gemsReward;
    [SerializeField] public TextMeshProUGUI amountOfGemsText;

    [SerializeField] public Button settingsButton;
    [SerializeField] public Button shopButton;
    [SerializeField] public Button boosterButton;
    [SerializeField] public Button protectorsButton;

    private void Awake()
    {
        Score = GetComponentInChildren<UIScore>();
        ProgressBar = GetComponentInChildren<UIProgressBar>();
        TimerBar = GetComponentInChildren<UITimer>();
        CoinsCounter = GetComponentInChildren<UICoinsCounter>();
        Settings = GetComponentInChildren<UISettings>();
    }

    private void Start()
    {
        settingsButton.onClick.AddListener(OnSettingsButtonPressed);
        shopButton.onClick.AddListener(OnShopButtonPressed);
        boosterButton.onClick.AddListener(OnBoosterButtonPressed);
        protectorsButton.onClick.AddListener(OnProtectorsButtonPressed);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public override void Init()
    {
        base.Init();

        CheckBoostersAvailability();
    }

    private void CheckBoostersAvailability()
    {
        boosterButton.interactable = GameData.Coins >= GameConfig.GEMS_BOOSTER_PRICE;
        protectorsButton.interactable = GameData.Coins >= GameConfig.GEMS_PROTECTORS_PRICE;

        if (Level.Instance.IsProtectorsAvailable())
        {
            boosterButton.gameObject.SetActive(false);
        }
        else
        {
            protectorsButton.gameObject.SetActive(false);
        }

        if (GameManager.Instance.LevelsCompleted == 24)
        {
            System.DateTime currentTime = System.DateTime.FromBinary(System.Convert.ToInt64(PlayerPrefs.GetString("SetNextLevelTime")));
            if (currentTime.Subtract(System.DateTime.Now).TotalMilliseconds > 1)
            {
                protectorsButton.gameObject.SetActive(false);
            }

        }
        //if (GameManager.Mode != GameMode.Levels || GameManager.Instance.LevelsCompleted < 10)
        if (GameManager.Mode != GameMode.Levels)
        {
            boosterButton.gameObject.SetActive(false);
            protectorsButton.gameObject.SetActive(false);
        }

        if(PlayerPrefs.GetInt("HasProtected") == 1)
        {
            protectorsButton.gameObject.SetActive(false);

            if (Level.Instance.IsProtectorsAvailable())
                Level.Instance.CreateProtectors();
        }

        if(GameManager.Instance.LevelsCompleted == 4 || GameManager.Instance.LevelsCompleted == 5)
        {
            protectorsButton.gameObject.SetActive(false);

            if (Level.Instance.IsProtectorsAvailable())
                Level.Instance.CreateProtectors();
        }
    }

    private void OnSettingsButtonPressed()
    {
        SoundManager.Instance.PlayButtonSFX();

        GameManager.Instance.State = GameState.Paused;

        UIManager.Instance.ActivatePanel(UIPanelType.SettingsPanel);

        SettingsButtonPressed.SaveInvoke();
    }

    private void OnShopButtonPressed()
    {
        SoundManager.Instance.PlayButtonSFX();

        GameManager.Instance.State = GameState.Paused;

        UIManager.Instance.ActivatePanel(UIPanelType.ShopPanel);

        ShopButtonPressed.SaveInvoke();
    }

    private void OnBoosterButtonPressed()
    {
        AdsManager.Instance.ShowInterstitial(GlobalConstants.Booster);
    }

    private void OnProtectorsButtonPressed()
    {
        AdsManager.Instance.ShowInterstitial(GlobalConstants.Protectors);
    }

    public void ShowReward(int amountOfGems)
    {
        amountOfGemsText.text = amountOfGems.ToString();

        gemsReward.SetActive(true);
    }

    public void SetActiveBottomPanel(bool enabled)
    {
        bottomPanel.SetActive(enabled);
    }

    private void OnRVAvailabilityChanged(bool isReady)
    {
        RVButton(isReady);
    }

    private void RVButton(bool isEnabled)
    {
        boosterButton.interactable = isEnabled;
        protectorsButton.interactable = isEnabled;
        if (PlayerPrefs.GetInt("HasProtected") == 1)
        {
            protectorsButton.gameObject.SetActive(false);
        }
    }

    public void OnRVRewardReceived(string key)
    {
        if (key.Equals(GlobalConstants.Booster))
        {
            Debug.Log("~~~" + GlobalConstants.Booster);
            bottomPanel.SetActive(false);

            GameData.Coins -= GameConfig.GEMS_BOOSTER_PRICE;

            SoundManager.Instance.PlayBoostSFX();

            int countLevelStructs = Level.Instance.GetStructsCount();
            int skippedStructs = countLevelStructs / 2;

            Player.Instance.Movement.MoveToTarget(skippedStructs, false);
        }
        else if (key.Equals(GlobalConstants.Protectors))
        {
            Debug.Log("~~~" + GlobalConstants.Protectors);
            bottomPanel.SetActive(false);

            Level.Instance.CreateProtectors();

            GameData.Coins -= GameConfig.GEMS_PROTECTORS_PRICE;
        }
    }
}