using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager1 : MonoBehaviour
{
    public static UIManager1 Instance;
    public GameObject panel_Continue;
    public GameObject panel_FirstScreen;
    public GameObject panel_Notice;
    public GameObject txt_guide;

    public GameObject bigWin_Effect;
    public GameObject threekinds_Effect;
    public GameObject jackpot_Effect;

    Sequence seq;
    private void Awake()
    {
    }
    void Start()
    {
        if (Instance == null)
            Instance = this;

        if (PlayerPrefs.GetInt("isFirstPlay", 0) == 1)
        {
            HideTip();
        }
        else
        {
            seq = DOTween.Sequence();
                seq.Append(txt_guide.transform.DOScale(1.1f, 0.5f)).SetLoops(-1, LoopType.Yoyo);

            txt_guide.GetComponent<Text>().text = "Tap To Jump Up";
        }
    }

    
    public void ShowContinuePanel()
    {
        Invoke("delay_ShowContinuePanel", 0.5f);
    }

    private void delay_ShowContinuePanel()
    {
        panel_Continue.SetActive(true);
    }

    public void HideContinuePanel()
    {
        panel_Continue.SetActive(false);
    }

    public void HideTapPanel()
    {
        panel_FirstScreen.SetActive(false);
    }

    //******************** Show Tutorial

    public void HideTip()
    {
        CancelInvoke("ShowHitText");
        DOTween.Kill(seq);
        txt_guide.transform.localScale = Vector3.zero;
        txt_guide.GetComponent<Text>().text = "";
    }
    public void ShowHitTip()
    {
        DOTween.Kill(seq);
        txt_guide.transform.localScale = Vector3.zero;
        txt_guide.GetComponent<Text>().text = "";

        Invoke("ShowHitText", 0.8f);
    }

    private void ShowHitText()
    {
        txt_guide.GetComponent<Text>().text = "Tap To Smash Down";
        txt_guide.transform.localScale = Vector3.one;
        seq = DOTween.Sequence();
            seq.Append(txt_guide.transform.DOScale(1.1f, 0.5f)).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);

        Time.timeScale = 0;
    }

    public void ShowPerfectTip()
    {
        HideTip();

        txt_guide.transform.localScale = Vector3.zero;
        txt_guide.GetComponent<Text>().text = "Perfect";

        //seq = new Sequence()
        //    .Append(txt_guide.transform.DOScale(0, 0.5f)).SetLoops(1)
        //    .Append(txt_guide.transform.DOScale(1, 0.01f))
        //    .Append(txt_guide.transform.DOScale(1.1f, 0.5f))
        //    .Append(txt_guide.transform.DOScale(0f, 0.5f));
    }

    private void ShowPerfectText()
    {

    }

    public void IgnoreGuide()
    {
        DOTween.Kill(txt_guide, false);
    }
    //**********************************
    
    //******************** First Collect Item Notice   
    private System.Action callback;
    [HideInInspector]
    public bool isShowNotice = false;
    public void ShowFirstCollect_Notice(System.Action action)
    {
        callback = action;
        isShowNotice = true;
        Invoke("ShowNoticeWindow", 0.2f);
    }

    private void ShowNoticeWindow()
    {
        panel_Notice.transform.localScale = Vector3.zero;
        panel_Notice.SetActive(true);
        panel_Notice.transform.DOScale(1, 0.3f).SetUpdate(true);
        Time.timeScale = 0f;
    }
    public void OnClick_Notice()
    {
        panel_Notice.SetActive(false);
        Time.timeScale = 1;
        GlobalSetting.Instance.SetFirstCollectedItem();
        callback();

        Invoke("ReleasePlayerAction", 0.1f);
    }

    private void ReleasePlayerAction()
    {
        isShowNotice = false;
    }
    //*********************************************

    public void ShowBigWinEffects()
    {
        bigWin_Effect.GetComponent<Animator>().SetTrigger("Show");
        bigWin_Effect.GetComponent<AudioSource>().Play();
    }

    public void ShowJackpotEffects()
    {
        jackpot_Effect.GetComponent<Animator>().SetTrigger("Show");
        jackpot_Effect.GetComponent<AudioSource>().Play();
    }

    public void Show3KindsEffects()
    {
        threekinds_Effect.GetComponent<Animator>().SetTrigger("Show");
        threekinds_Effect.GetComponent<AudioSource>().Play();
    }
}
