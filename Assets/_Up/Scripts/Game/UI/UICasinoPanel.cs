using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UICasinoPanel : UIPanel
{
    [SerializeField] private Button spinButton;
    [SerializeField] private Button spin_rewardButton;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject[] casinoElements;

    [SerializeField] private GameObject earnObj;

    [SerializeField] private TextMeshProUGUI congratulationLabel;
    public string[] congratulationText;

    private int selectedCasinoIdx = 0;

    public GameObject[] rewardParticles;

    bool isRewarded = false;

    private void OnEnable()
    {
        //if(PlayerPrefs.GetInt("FirstCasino", 0) == 0)
        //{
        //    selectedCasinoIdx = 0;
        //    PlayerPrefs.SetInt("FirstCasino", 1);
        //}
        //else
        //    selectedCasinoIdx = Random.Range(0, 3);
        if (GameManager.Instance.LevelsCompleted < 29)
        {
            selectedCasinoIdx = 0;
        }
        else
            selectedCasinoIdx = Random.Range(0, 3);

        for (int i = 0; i < 3; i++)
        {
            casinoElements[i].SetActive(false);
        }
        casinoElements[selectedCasinoIdx].SetActive(true);

        spinButton.gameObject.SetActive(true);
        spinButton.enabled = true;
        spin_rewardButton.gameObject.SetActive(false);
        spin_rewardButton.enabled = false;
        backButton.gameObject.SetActive(false);
        backButton.enabled = false;

        isRewarded = false;

        earnObj.SetActive(false);

        congratulationLabel.text = congratulationText[Random.Range(0, 4)];
    }

    private void OnDisable()
    {

    }

    public void StartCasinoElement()
    {
        switch (selectedCasinoIdx)
        {
            case 0:
                FindObjectOfType<CasinoElement.SlotManager>().ShowSlot_Event();
                break;
            case 1:
                FindObjectOfType<CasinoElement.CardController>().Start_Cards();
                break;
            case 2:
                FindObjectOfType<CasinoElement.LuckyCube>().PlayLuckyCube();
                break;
        }
    }

    public void Show()
    {
        SetActive(true);

        SoundManager.Instance.PlayRewardSFX();
    }

    public void ShowEarnCoins(int earnMoney)
    {
        TextMeshProUGUI textMesh;
        textMesh = earnObj.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = "+ " + earnMoney.ToString();
        earnObj.SetActive(true);
    }

    public void OnFinishCasinoElements()
    {
        Debug.Log("OnFinishCasinoElements");

        if (isRewarded == false)
        {
            spin_rewardButton.gameObject.SetActive(true);
            spin_rewardButton.enabled = true;
            backButton.gameObject.SetActive(true);
            backButton.enabled = true;

        }
        else
        {
            Invoke("OnBackButtonPressed", 1);
        }
    }

    public void ShowBackButton()
    {
        backButton.gameObject.SetActive(true);
        backButton.enabled = true;
    }

    public void OnSpinButtonPressed()
    {
        SoundManager.Instance.PlayButtonSFX();
        spinButton.gameObject.SetActive(false);
        Debug.LogError("Spin");
        spinButton.enabled = false;
        StartCasinoElement();

        earnObj.SetActive(false);
    }

    public void OnSpinRewardButtonPressed()
    {
        isRewarded = true;
        spin_rewardButton.gameObject.SetActive(false);
        spin_rewardButton.enabled = false;
        earnObj.SetActive(false);

        AdsManager.Instance.ShowInterstitial(GlobalConstants.SpinAgain);
    }

    public void OnBackButtonPressed()
    {
        SoundManager.Instance.PlayButtonSFX();

        SceneManager.LoadScene("Game");
    }

    public void OnRVRewardReceived(string key)
    {
        if (key.Equals(GlobalConstants.SpinAgain))
        {
            Time.timeScale = 1;
            Debug.Log("~~~" + GlobalConstants.SpinAgain);
            SoundManager.Instance.PlayButtonSFX();

            spin_rewardButton.enabled = false;
            backButton.enabled = false;
            StartCasinoElement();

        }
    }
}
