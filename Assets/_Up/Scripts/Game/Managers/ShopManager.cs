using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public string LastSkin
    {
        get
        {
            if (!PlayerPrefs.HasKey("LastSkin"))
            {
                PlayerPrefs.SetString("LastSkin", "Career1");
            }

            return PlayerPrefs.GetString("LastSkin");
        }
        set
        {
            PlayerPrefs.SetString("LastSkin", value);
        }
    }

    [SerializeField] private Image currentItem;

    [SerializeField] private Transform adShopItemsContainer;
    [SerializeField] private Transform careerShopItemsContainer;

    [SerializeField] private GameObject adShopItem;
    [SerializeField] private GameObject careerShopItem;

    [SerializeField] private List<Sprite> adShopItemsSprites;
    [SerializeField] private List<Sprite> careerShopItemsSprites;

    private List<UIShopItem> instantiatedItems = new List<UIShopItem>();

    public static ShopManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        for (int i = 0; i < adShopItemsSprites.Count; i++)
        {
            UIShopItem adShopItem = Instantiate(this.adShopItem, adShopItemsContainer).GetComponent<UIShopItem>();
            Sprite itemSprite = adShopItemsSprites.ElementAt(i);

            adShopItem.Init(itemSprite);
            instantiatedItems.Add(adShopItem);

            adShopItem.ItemSelected += OnItemSelected;
        }

        for (int i = 0; i < careerShopItemsSprites.Count; i++)
        {
            UIShopItem careerShopItem = Instantiate(this.careerShopItem, careerShopItemsContainer).GetComponent<UIShopItem>();
            Sprite itemSprite = careerShopItemsSprites.ElementAt(i);

            careerShopItem.Init(itemSprite);
            instantiatedItems.Add(careerShopItem);

            careerShopItem.ItemSelected += OnItemSelected;
        }

        careerShopItemsContainer.transform.parent.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        SetLastSkin();
    }

    public void OpenRandomCarrerSkin()
    {
        List<UIShopItem> notOpenedCarrerItems = instantiatedItems.Where(item => !item.IsAdItem && !item.IsOpened).ToList();

        if (notOpenedCarrerItems.Count() > 0)
        {
            int index = Random.Range(0, notOpenedCarrerItems.Count());

            UIShopItem firstNotOpenedItem = notOpenedCarrerItems.ElementAt(index);
            firstNotOpenedItem.IsOpened = true;
        }
    }

    public UIShopItem GetNextCareerSkin()
    {
        UIShopItem firstNotOpenedItem = instantiatedItems.FirstOrDefault(item => !item.IsAdItem && !item.IsOpened);

        return firstNotOpenedItem;
    }

    public bool HasAdSkins()
    {
        return instantiatedItems.Any(item => item.IsAdItem && !item.IsOpened);
    }

    public bool HasCareerSkins()
    {
        return instantiatedItems.Any(item => !item.IsAdItem && !item.IsOpened);
    }

    private void SetLastSkin()
    {
        UIShopItem suitableSkin = instantiatedItems.FirstOrDefault(item => item.ItemKey.Equals(LastSkin));

        suitableSkin.IsOpened = true;
        suitableSkin.IsSelected = true;
    }

    private void OnItemSelected(UIShopItem shopItem)
    {
        List<UIShopItem> notSelectedItems = instantiatedItems.Where(item => !item.ItemKey.Equals(shopItem.ItemKey)).ToList();

        foreach (var item in notSelectedItems)
        {
            item.IsSelected = false;
        }

        currentItem.sprite = shopItem.ItemSprite;

        Player.Instance.SetSkin(shopItem.ItemSprite);

        LastSkin = shopItem.ItemKey;
    }

    public List<UIShopItem> GetAvailableSkins()
    {
        return instantiatedItems.Where(item => item.IsOpened).ToList();
    }
}