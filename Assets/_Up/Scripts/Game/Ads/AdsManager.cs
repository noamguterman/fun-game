using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.Monetization;
//using UnityEngine.Advertisements;
using ShowResult = UnityEngine.Monetization.ShowResult;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;
    public static string targetStr = "";

    //    #region AppLovinAds
    //    // Put AppLovin SDK Key here or in your AndroidManifest.xml / Info.plist
    //    private const string SDK_KEY = "UFkiiJ7AvBeuCci-e6yGWdvy41LQ0LRql0T0fRtd-1graDwnmmKseg9n0-eVTnh6oG9Df795Ag24O1Ag4B6wVt";

    //    // Rewarded Video Button Texts
    //    private const string REWARDED_VIDEO_BUTTON_TITLE_PRELOAD = "Preload Rewarded Video";
    //    private const string REWARDED_VIDEO_BUTTON_TITLE_LOADING = "Show Rewarded Video";
    //    private const string REWARDED_VIDEO_BUTTON_TITLE_SHOW = "Show Rewarded Video";

    //    private bool IsPreloadingRewardedVideo = false;

    //    private void Awake()
    //    {
    //        GameObject[] objs = GameObject.FindGameObjectsWithTag("Ads");
    //        if (objs.Length > 1)
    //            Destroy(this.gameObject);

    //        Instance = this;
    //        DontDestroyOnLoad(this.gameObject);
    //    }

    //    private void Start()
    //    {
    //        // Set SDK key and initialize SDK
    //        AppLovin.SetSdkKey(SDK_KEY);
    //        AppLovin.InitializeSdk();
    //        AppLovin.SetTestAdsEnabled("false");
    //        AppLovin.SetUnityAdListener("ADSManager");
    //        AppLovin.SetRewardedVideoUsername("demo_user");

    //        StartCoroutine(LoadRewardedVideoAD());
    //        PreloadInterstital();

    //        Invoke("ShowBanner", 3);
    //    }

    //    IEnumerator LoadRewardedVideoAD()
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //        AppLovin.LoadRewardedInterstitial();
    //    }

    //    public void PreloadInterstital()
    //    {
    //        AppLovin.PreloadInterstitial();
    //    }

    //    public void ShowInterstitial(string str)
    //    {
    //        Debug.Log("Showing interstitial ad   =" + str);
    //        targetStr = str;
    //#if UNITY_EDITOR
    //        SendSuccessMsg();
    //#else
    //        Debug.Log("3333=" + targetStr);
    //        AppLovin.ShowInterstitial();
    //#endif
    //    }

    //    public  void PreloadRewardedVideo()
    //    {
    //        if (AppLovin.IsIncentInterstitialReady())
    //        {
    //            IsPreloadingRewardedVideo = false;

    //            AppLovin.ShowRewardedInterstitial();
    //        }
    //        else
    //        {
    //            IsPreloadingRewardedVideo = true;

    //            AppLovin.LoadRewardedInterstitial();
    //        }
    //    }

    //    public  void ShowRewardedVideo(string str)
    //    {
    //        Debug.Log("Show Rewarded ad  =" + str);
    //        targetStr = str;
    //        Time.timeScale = 0;

    //#if UNITY_EDITOR
    //        SendSuccessMsg();
    //#else
    //        PreloadRewardedVideo();
    //#endif
    //    }

    //    public void ShowBanner()
    //    {
    //        Debug.Log("Showing banner ad");
    //        AppLovin.ShowAd(AppLovin.AD_POSITION_CENTER, AppLovin.AD_POSITION_BOTTOM);
    //    }

    //    private void onAppLovinEventReceived(string ev)
    //    {
    //        // Log AppLovin event
    //        Debug.Log("~~~~~~~~~" + ev + "   ::" + targetStr);

    //        if (ev.Contains("REWARD"))
    //        {
    //            if (ev.Contains("REWARDAPPROVEDINFO"))
    //            {
    //                //success
    //                SendSuccessMsg();
    //            }
    //        }
    //        // Check if this is a Rewarded Video preloading event
    //        else if (IsPreloadingRewardedVideo && (ev.Contains("LOADREWARDEDFAILED") || ev.Contains("LOADFAILED")))
    //        {
    //            SendFailMsg();
    //            IsPreloadingRewardedVideo = false;
    //            StartCoroutine(LoadRewardedVideoAD());
    //        }
    //        else if (ev.Contains("HIDDENINTER"))
    //        {
    //            SendSuccessMsg();
    //            //Must be add interstitial callback function
    //            AppLovin.PreloadInterstitial();
    //        }
    //        else
    //        {
    //            //StartCoroutine(LoadRewardedVideoAD());
    //        }


    //    }

    //    private void SendSuccessMsg()
    //    {
    //        Debug.Log("@@@@  =" + targetStr);
    //        GameObject.Find("Scripts").SendMessage("OnRVRewardReceived", targetStr);
    //        GameObject.Find("Canvas").BroadcastMessage("OnRVRewardReceived", targetStr);

    //        targetStr = "";
    //    }

    //    private void SendFailMsg()
    //    {

    //        Debug.Log("%%% =" + targetStr);
    //        GameObject.Find("Scripts").SendMessage("OnRVRewardReceived_Fail", targetStr);
    //        GameObject.Find("Canvas").SendMessage("OnRVRewardReceived_Fail", targetStr);

    //        targetStr = "";
    //    }
    //    #endregion

    #region UnityAds
    string placementId_video = "video";
    string placementId_rewardedvideo = "rewardedVideo";
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Ads");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        //Initialize Unity
#if UNITY_ANDROID
        Monetization.Initialize("3221459", false);
#else
        Monetization.Initialize("3221458", false);
#endif


        //Advertisement.Initialize("3221459", false);
    }

    private void Start()
    {
        //StartCoroutine(ShowBannerWhenReady());
    }

    int trycnt;
    //IEnumerator ShowBannerWhenReady()
    //{
    //    while (!Advertisement.IsReady("bannerPlacement") && trycnt < 5)
    //    {
    //        Debug.Log("Banner not ready");
    //        trycnt++;
    //        yield return new WaitForSeconds(1f);
    //    }
    //    Debug.Log("Showing Banner");
    //    Advertisement.Banner.Show("bannerPlacement");
    //    Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    //}

    public void ShowInterstitial(string str)
    {
        Debug.Log("Showing interstitial ad   =" + str);
        targetStr = str;
        Time.timeScale = 0;
        StartCoroutine(WaitForAd());
    }

    public void ShowRewardedVideo(string str)
    {
        Debug.Log("Show Rewarded ad  =" + str);
        targetStr = str;
        Time.timeScale = 0;
        StartCoroutine(WaitForAd(true));
    }

    IEnumerator WaitForAd(bool rewarded = false)
    {
        string placementId = rewarded ? placementId_rewardedvideo : placementId_video;
        while (!Monetization.IsReady(placementId))
        {
            yield return null;
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent(placementId) as ShowAdPlacementContent;

        if (ad != null)
        {
            if(rewarded == true)
                ad.Show(OnResultRewarded);
            else
                ad.Show(OnResultInterstitial);
        }
    }

    private void OnResultInterstitial(ShowResult result)
    {
        Debug.Log("InterstitialResult ::::::" + result);
        Time.timeScale = 1;
        SendSuccessMsg();
    }

    private void OnResultRewarded(ShowResult result)
    {
        Debug.Log("RewardedResult ::::::" + result);
        if(result == ShowResult.Finished)
            SendSuccessMsg();
        else
        {
            SendFailMsg();
        }
    }
#endregion

    private void SendSuccessMsg()
    {
        Debug.Log("@@@@  =" + targetStr);
        GameObject.Find("Scripts").SendMessage("OnRVRewardReceived", targetStr);
        GameObject.Find("Canvas").BroadcastMessage("OnRVRewardReceived", targetStr);

        targetStr = "";
    }

    private void SendFailMsg()
    {

        Debug.Log("%%% =" + targetStr);
        GameObject.Find("Scripts").SendMessage("OnRVRewardReceived_Fail", targetStr);
        GameObject.Find("Canvas").SendMessage("OnRVRewardReceived_Fail", targetStr);

        targetStr = "";
    }

    
}