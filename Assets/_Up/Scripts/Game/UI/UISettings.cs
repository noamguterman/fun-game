using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [SerializeField] private Button settingsButton;

    [SerializeField] private GameObject otherButtons;

    [SerializeField] private UITwoSpritesButton musicButton;
    [SerializeField] private UITwoSpritesButton soundsButton;
    [SerializeField] private Button removeAdButton;
    [SerializeField] private Button restoreButton;

    public bool IsMusicActive
    {
        get
        {
            if (!PlayerPrefs.HasKey("IsMusicActive"))
            {
                return false;
            }

            return PlayerPrefs.GetInt("IsMusicActive") == 1;
        }
        set
        {
            PlayerPrefs.SetInt("IsMusicActive", value ? 1 : 0);
        }
    }

    public bool IsSoundsActive
    {
        get
        {
            if (!PlayerPrefs.HasKey("IsSoundsActive"))
            {
                return true;
            }

            return PlayerPrefs.GetInt("IsSoundsActive") == 1;
        }
        set
        {
            PlayerPrefs.SetInt("IsSoundsActive", value ? 1 : 0);
        }
    }

    private void OnEnable()
    {
    }

    private void Start()
    {
        InitLastSettings();

        settingsButton.onClick.AddListener(OnSettingsButtonPressed);

        musicButton.Button.onClick.AddListener(OnMusicButtonPressed);
        soundsButton.Button.onClick.AddListener(OnSoundsButtonPressed);
        removeAdButton.onClick.AddListener(OnRemoveAdButtonPressed);
        restoreButton.onClick.AddListener(OnRestoreButtonPressed);

        musicButton.UpdateState(IsMusicActive);
        soundsButton.UpdateState(IsSoundsActive);

        otherButtons.SetActive(false);

#if UNITY_ANDROID
        restoreButton.gameObject.SetActive(false);
#endif
    }

    private void OnSettingsButtonPressed()
    {
        otherButtons.SetActive(!otherButtons.activeInHierarchy);
    }

    private void InitLastSettings()
    {
        //AudioSource music = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();

        //music.volume = IsMusicActive ? 1 : 0;

        GameObject.FindGameObjectWithTag("music").GetComponent<BGMManager>().SetVolume(IsMusicActive);

        SoundManager.Instance.ActiveSounds(IsSoundsActive);
    }

    private void OnMusicButtonPressed()
    {
        IsMusicActive = !IsMusicActive;

        //AudioSource music = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();

        //music.volume = IsMusicActive ? 1 : 0;

        GameObject.FindGameObjectWithTag("music").GetComponent<BGMManager>().SetVolume(IsMusicActive);

        if (IsMusicActive)
        {
            //ServiceProvider.Analytics.SendEvent("music_enabled");
        }
        else
        {
            //ServiceProvider.Analytics.SendEvent("music_disabled");
        }
    }

    private void OnSoundsButtonPressed()
    {
        IsSoundsActive = !IsSoundsActive;

        SoundManager.Instance.ActiveSounds(IsSoundsActive);

        if (IsSoundsActive)
        {
            //ServiceProvider.Analytics.SendEvent("sound_enabled");
        }
        else
        {
            //ServiceProvider.Analytics.SendEvent("sound_disabled");
        }
    }

    private void OnRemoveAdButtonPressed()
    {
    }

    private void OnNoAdsActivated()
    {
        removeAdButton.gameObject.SetActive(false);
    }

    private void OnRestoreButtonPressed()
    {
    }
}