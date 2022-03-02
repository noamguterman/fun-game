using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeController : MonoBehaviour
{
    public static ChallengeController Instance;
    public string[] challengeText;
    public int currentChallenge;

    private int[] ChallengeTarget = { 250, 50, 10, 5 };
    //score 250 point
    //collect 50 icons
    //win 10 jackpots
    //score 500+ points 5x in a row

    private int currentCompleteCount = 0;

    public Text txt_challengeDescription;
    public Text txt_currentState;
    public Image img_slider;

    public float progress;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        currentChallenge = PlayerPrefs.GetInt("CurrentChallenge", 1);
    }

    public void CompleteChallenge()
    {
        currentChallenge++;
        PlayerPrefs.SetInt("CurrentChallenge", currentChallenge);
    }

    private void ResetChallenge()
    {
        PlayerPrefs.SetInt("CurrentChallenge", currentChallenge);
        currentCompleteCount = 0;
        img_slider.fillAmount = 0;
        progress = 0;
        txt_challengeDescription.text = challengeText[currentChallenge - 1];
        txt_currentState.text = "0 / " + ChallengeTarget[currentChallenge - 1];
    }

    
    void Start()
    {
        StartCoroutine(ShowChallengeInfo());
    }

    IEnumerator ShowChallengeInfo()
    {
        yield return new WaitForSeconds(0.1f);
        switch (currentChallenge)
        {
            case 1:
                currentCompleteCount = ScoreManager.Instance.Best;
                txt_currentState.text = currentCompleteCount + " / " + ChallengeTarget[0];
                progress = currentCompleteCount / (float)ChallengeTarget[0];
                img_slider.fillAmount = progress;
                txt_challengeDescription.text = challengeText[currentChallenge - 1];
                break;
            case 2:
                currentCompleteCount = PlayerPrefs.GetInt("CollectItemCount", 0);

                txt_currentState.text = currentCompleteCount + " / " + ChallengeTarget[1];
                progress = currentCompleteCount / (float)ChallengeTarget[1];
                img_slider.fillAmount = progress;
                txt_challengeDescription.text = challengeText[currentChallenge - 1];
                break;
            case 3:
                currentCompleteCount = PlayerPrefs.GetInt("JackpotCount", 0);

                txt_currentState.text = currentCompleteCount + " / " + ChallengeTarget[2];
                progress = currentCompleteCount / (float)ChallengeTarget[2];
                img_slider.fillAmount = progress;
                txt_challengeDescription.text = challengeText[currentChallenge - 1];
                break;
            case 4:
                currentCompleteCount = PlayerPrefs.GetInt("Multiple500Count", 0);

                txt_currentState.text = currentCompleteCount + " / " + ChallengeTarget[2];
                progress = currentCompleteCount / (float)ChallengeTarget[2];
                img_slider.fillAmount = progress;
                txt_challengeDescription.text = challengeText[currentChallenge - 1];
                break;
            case 5:
                txt_currentState.text = "";
                progress = 1;
                img_slider.fillAmount = progress;
                txt_challengeDescription.text = "COMPLETED!";
                break;
        }
    }

    public void SetScores()
    {
        if (currentChallenge != 1)
            return;

        currentCompleteCount = ScoreManager.Instance.Best;
        if (currentCompleteCount >= ChallengeTarget[0])
        {
            currentChallenge++;
            ResetChallenge();
        }
    }

    public void SetCollectItemCount()
    {
        if (currentChallenge != 2)
            return;
        currentCompleteCount++;
        Debug.Log(currentCompleteCount);
        PlayerPrefs.SetInt("CollectItemCount", currentCompleteCount);
        if(currentCompleteCount >= ChallengeTarget[1])
        {
            currentChallenge++;
            ResetChallenge();
        }
    }

    public void SetJackpotCount()
    {
        if (currentChallenge != 3)
            return;
        currentCompleteCount++;
        Debug.Log(currentCompleteCount);
        PlayerPrefs.SetInt("JackpotCount", currentCompleteCount);
        if (currentCompleteCount >= ChallengeTarget[2])
        {
            currentChallenge++;
            ResetChallenge();
        }
    }

    public void SetMultiple500Count()
    {
        if (currentChallenge != 4)
            return;
        currentCompleteCount++;
        Debug.Log(currentCompleteCount);
        PlayerPrefs.SetInt("Multiple500Count", currentCompleteCount);
        if (currentCompleteCount >= ChallengeTarget[3])
        {
            currentChallenge++;
            ResetChallenge();
        }
    }
}
