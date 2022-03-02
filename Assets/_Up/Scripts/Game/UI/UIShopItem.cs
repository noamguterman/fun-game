using Assets._Scripts.Tools;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour
{
    public event Action<UIShopItem> ItemSelected;

    public string ItemKey
    {
        get; private set;
    }

    public Sprite ItemSprite
    {
        get; private set;
    }

    public bool IsAdItem
    {
        get
        {
            return isAdItem;
        }
    }

    [SerializeField] private bool isAdItem;

    [SerializeField] private GameObject blank;

    [SerializeField] private Image icon;
    [SerializeField] private Image outline;

    private Button button;

    public bool IsOpened
    {
        get
        {
            if(isAdItem == true)
                return true;
            else
                return PlayerPrefs.GetInt(ItemKey) == 1;
        }
        set
        {
            if(isAdItem == true)
                PlayerPrefs.SetInt(ItemKey, 1);
            else
                PlayerPrefs.SetInt(ItemKey, value ? 1 : 0);

            UpdateView();
        }
    }

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        {
            isSelected = value;

            outline.enabled = isSelected;

            if (isSelected)
            {
                ItemSelected.SaveInvoke(this);
            }
        }
    }

    private bool isSelected;

    private static string selectedItemKey;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClicked);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void Init(Sprite itemSprite)
    {
        ItemSprite = itemSprite;
        ItemKey = itemSprite.name;

        outline.enabled = false;

        UpdateView();
    }

    public void UpdateView()
    {
        if (IsOpened)
        {
            if (blank != null)
            {
                blank.SetActive(false);
            }

            icon.gameObject.SetActive(true);
            icon.sprite = ItemSprite;
        }
    }

    private void OnClicked()
    {
        SoundManager.Instance.PlayButtonSFX();

        if (IsOpened)
        {
            IsSelected = true;
        }
        else if(isAdItem)
        {
            selectedItemKey = ItemKey;

            if(GameData.Coins >= 2500)
            {
                GameData.Coins -= 2500;
                IsOpened = true;
                IsSelected = true;
            }
            else
            {

            }
            //AdsManager.Instance.ShowRewardedVideo(GlobalConstants.VideoSkin);
        }
    }

    private void OnRVAvailabilityChanged(bool isReady)
    {
        RVButton(isReady);
    }

    private void RVButton(bool isEnabled)
    {
        button.interactable = isEnabled;
    }

    public void OnRVRewardReceived(string key)
    {
        if (key.Equals(GlobalConstants.VideoSkin) && ItemKey.Equals(selectedItemKey))
        {
            Debug.Log("~~~" + GlobalConstants.VideoSkin);
            IsOpened = true;
            IsSelected = true;
        }
    }
}