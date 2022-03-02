using UnityEngine;
using UnityEngine.SceneManagement;

public class UIRatePanel : UIPanel
{
    [SerializeField] private string iOSBundleId = "";

    private string androidBundleId = "";

    private string androidMarketAppUrl = "market://details?id=";
    private string iOSMarketAppUrl = "itms-apps://itunes.apple.com/app/";

    private void Awake()
    {
#if UNITY_ANDROID
        androidMarketAppUrl = Application.identifier;
#endif
    }

    public void OnBackButtonPressed()
    {
        SoundManager.Instance.PlayButtonSFX();

        SceneManager.LoadScene("Game");
    }

    public void OnNeverPressed()
    {
        SoundManager.Instance.PlayButtonSFX();

        GameManager.Instance.IsGameRated = true;

        SceneManager.LoadScene("Game");
    }

    public void OnRateAppPressed()
    {
        SoundManager.Instance.PlayButtonSFX();

        GameManager.Instance.IsGameRated = true;
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=com.epc.fungame");
        //RateApplication();
#elif UNITY_IOS
        Application.OpenURL("itms-apps://itunes.apple.com/app/id1481442320");
#endif

        SceneManager.LoadScene("Game");
    }

    private void RateApplication()
    {
        string appAlias = GetAppAlias();

        if (!string.IsNullOrEmpty(appAlias))
        {
            string url = GetAppMarketURL(appAlias);

            Debug.Log("RateMeDialogue: open URL=" + url);

            //Application.OpenURL(url);
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.epc.fungame");
        }
        else
        {
            Debug.LogError("RateMeDialogue: package_name is empty");
        }
    }

    string GetAppAlias()
    {
        string res = "";

#if UNITY_ANDROID
        res = androidBundleId;
#elif UNITY_IPHONE
            res = iOSBundleId;
#endif

        return res;
    }

    public string GetAppMarketURL(string appAlias)
    {
        string res = "";

#if UNITY_ANDROID
        res = androidMarketAppUrl + appAlias;
#elif UNITY_IPHONE
        res = iOSMarketAppUrl + appAlias;
#endif

        return res;
    }
}