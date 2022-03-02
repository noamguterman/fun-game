using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITimer : MonoBehaviour
{
    [SerializeField] private Image progressImage;

    private int missionTime;
    private float passedTime;
    private bool isTimeOver = false;

    public GameObject timerPopupPanel;

    [HideInInspector]
    public bool isStartCountDown = false;

    public void Init()
    {
        Debug.Log("TimerInit = " + GameManager.Instance.LevelTime);

        
        if (GameManager.Instance.LevelTime == -1)
        {
            gameObject.SetActive(false);
            return;
        }
        missionTime = GameManager.Instance.LevelTime;
        passedTime = 0;

        if(GameManager.Instance.LevelsCompleted == 3 && PlayerPrefs.GetInt("isFirstTimerPopup", 0) == 0)
        {
            PlayerPrefs.SetInt("isFirstTimerPopup", 1);
            Invoke("SetPause", 0.1f);
        }
        else if(GameManager.Instance.LevelsCompleted == 7 && PlayerPrefs.GetInt("isSecondTimerPopup", 0) == 0)
        {
            PlayerPrefs.SetInt("isSecondTimerPopup", 1);
            Invoke("SetPause", 0.1f);
        }
        else if (GameManager.Instance.LevelsCompleted == 11 && PlayerPrefs.GetInt("isThirdTimerPopup", 0) == 0)
        {
            PlayerPrefs.SetInt("isThirdTimerPopup", 1);
            Invoke("SetPause", 0.1f);
        }
    }

    void SetPause()
    {
        timerPopupPanel.SetActive(true);
        timerPopupPanel.transform.localScale = Vector3.zero;
        timerPopupPanel.transform.DOScale(1, 0.2f).SetUpdate(true);
        GameManager.Instance.State = GameState.Paused;
        SoundManager.Instance.PlayOpenTimePanel();
    }

    public void OnClosePopup()
    {
        GameManager.Instance.State = GameState.Alive;
    }

    private void Update()
    {
        if (isStartCountDown == false)
            return;
        if (GameManager.Instance.reachedLastStructure == true)
            return;

        passedTime += Time.deltaTime;

        if (passedTime >= missionTime)
        {
            if(isTimeOver == false)
            {
                isTimeOver = true;
                SoundManager.Instance.PlayTimeOverSFX();
                GameManager.Instance.OnTimeOver();
            }
        }
        else
        {
            float progress = (missionTime - passedTime) / missionTime;
            progressImage.fillAmount = progress;
            progressImage.color = new Color(1, 0, progress);
            if(progress < 0.3f)
            {
                transform.Find("Image").GetComponent<Animator>().SetTrigger("play");
            }
        }
        
    }

    public void SetActive(bool enabled)
    {
        gameObject.SetActive(enabled);
    }
}
