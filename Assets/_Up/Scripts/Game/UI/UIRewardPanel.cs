using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIRewardPanel : UIPanel
{
    [SerializeField] private Image skinIcon;
    [SerializeField] private TextMeshProUGUI randomText;
    [SerializeField] private RandomRewardText randomTextsDatabase;
    [SerializeField] private Button get3RandomSkinsButton;

    private UIShopItem shopItem;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void Show()
    {
        Debug.Log("Show Rewards");
        SetActive(true);

        shopItem = ShopManager.Instance.GetNextCareerSkin();

        randomText.text = randomTextsDatabase.GetRandomText();
        skinIcon.sprite = shopItem.ItemSprite;
        shopItem.IsOpened = true;

        SoundManager.Instance.PlayRewardSFX();
    }

    public void OnEquipButtonPressed()
    {
        SoundManager.Instance.PlayButtonSFX();

        shopItem.IsSelected = true;

        OnBackButtonPressed();
    }

    public void OnGet3ButtonPressed()
    {
        AdsManager.Instance.ShowRewardedVideo(GlobalConstants.Get3RandomSkins);
    }

    public void OnBackButtonPressed()
    {
        SoundManager.Instance.PlayButtonSFX();

        //if(PlayerPrefs.GetInt("isFirstReward", 0) == 0)
        //{
        //    PlayerPrefs.SetInt("isFirstReward", 1);
        //    UIManager.Instance.ActivatePanel(UIPanelType.ShopPanel);
        //}
        //else
        {
            SceneManager.LoadScene("Game");
        }
    }

    private void OnRVAvailabilityChanged(bool isReady)
    {
        RVButton(isReady);
    }

    private void RVButton(bool isEnabled)
    {
        get3RandomSkinsButton.interactable = isEnabled;
    }

    public void OnRVRewardReceived(string key)
    {
        if (key.Equals(GlobalConstants.Get3RandomSkins))
        {
            Time.timeScale = 1;

            Debug.Log("~~~" + GlobalConstants.Get3RandomSkins);
            SoundManager.Instance.PlayButtonSFX();

            for (int i = 0; i < 3; i++)
            {
                ShopManager.Instance.OpenRandomCarrerSkin();
            }

            SceneManager.LoadScene("Game");
        }
    }

}