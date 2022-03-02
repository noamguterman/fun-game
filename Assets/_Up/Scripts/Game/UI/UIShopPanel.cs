using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIShopPanel : UIPanel
{
    public Button freeprotectButton;
    public Button premiumButton;

    public bool HasProtected
    {
        get
        {
            return PlayerPrefs.GetInt("HasProtected") == 1;
        }
        set
        {
            PlayerPrefs.SetInt("HasProtected", value ? 1 : 0);

            UpdateProtector();
        }
    }

    public bool HasPremium
    {
        get
        {
            return PlayerPrefs.GetInt("HasPremium") == 1;
        }
        set
        {
            PlayerPrefs.SetInt("HasPremium", value ? 1 : 0);

            UpdatePremium();
        }
    }

    private void Awake()
    {
        freeprotectButton.onClick.AddListener(OnFreeProtectButtonPressed);
        premiumButton.onClick.AddListener(OnPremiumButtonPressed);

        UpdateProtector();
        UpdatePremium();
    }

    private void OnEnable()
    {
        transform.Find("CareerItems").GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
    }

    private void OnDisable()
    {
    
    }

    public override void Init()
    {
        base.Init();
        transform.Find("CareerItems").GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
    }


    void UpdateProtector()
    {
        if (PlayerPrefs.GetInt("HasProtected") == 1)
            freeprotectButton.gameObject.SetActive(false);
        else
            freeprotectButton.gameObject.SetActive(true);
    }

    void UpdatePremium()
    {
        if (PlayerPrefs.GetInt("HasPremium") == 1)
            premiumButton.gameObject.SetActive(false);
        else
            premiumButton.gameObject.SetActive(true);
    }

    public void OnBackButtonPressed()
    {
        SoundManager.Instance.PlayButtonSFX();

        SceneManager.LoadScene("Game");
        return;
        UIManager.Instance.ActivatePanel(UIPanelType.GamePanel);

        GameManager.Instance.State = GameState.Alive;
        GameData.Coins += 0;
    }

    public void OnFreeProtectButtonPressed()
    {
        SoundManager.Instance.PlayButtonSFX();

        if (GameData.Coins >= 5000)
        {
            GameData.Coins -= 5000;
            HasProtected = true;
        }
    }

    public void OnPremiumButtonPressed()
    {
        SoundManager.Instance.PlayButtonSFX();

        if (GameData.Coins >= 2500)
        {
            GameData.Coins -= 2500;
            HasPremium = true;
        }
    }

    private void OnRVAvailabilityChanged(bool isReady)
    {
        RVButton(isReady);
    }

    private void RVButton(bool isEnabled)
    {
    }

    public void OnRVRewardReceived(string key)
    {
        
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Q))
    //    {
    //        IsRandomActive = true;
    //        randomSkinsButton.gameObject.SetActive(false);
    //        RandomFeatureTime = 0;

    //        StartCoroutine(StartRandomFeature());
    //    }
    //}
}