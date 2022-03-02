using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CasinoElement;

public class GlobalSetting : MonoBehaviour
{
    public static GlobalSetting Instance;

    public enum CASINOELEMENT
    {
        SLOT,
        WHEEL,
        BINGO,
        CUBS,
        CARDS
    };

    public enum THEME
    {
        CAVEOFWOW,
        LOSTPEARL,
        BOOFAMILY
    }

    [HideInInspector]
    public CASINOELEMENT currentCasinoElement = CASINOELEMENT.SLOT;
    [HideInInspector]
    public THEME currentTheme;

    public GameObject[] casino_root;
    public GameObject wheelCamera;

    public bool isPlayableGame = false;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if(Instance == null)
            Instance = this;

        currentCasinoElement =(CASINOELEMENT)PlayerPrefs.GetInt("CasinoElement", 0);
        currentTheme = (THEME)PlayerPrefs.GetInt("Theme", 0);
    }

    public void ShowCasinoElement(int idx)
    {
        for(int i = 0; i < casino_root.Length; i++)
        {
            casino_root[i].SetActive(false);
        }
        wheelCamera.SetActive(false);


        casino_root[idx].SetActive(true);
        if(idx == 1)
        {
            wheelCamera.SetActive(true);
        }
    }

    public void StartCasinoElement()
    {
        switch (currentCasinoElement)
        {
            case CASINOELEMENT.SLOT:
                FindObjectOfType<SlotManager>().ShowSlot_Event();
                break;
            case CASINOELEMENT.WHEEL:
                FindObjectOfType<WheelScript>().StartWheel();
                break;
            case CASINOELEMENT.BINGO:
                FindObjectOfType<BingoController>().PlayBingoBoard();
                break;
            case CASINOELEMENT.CUBS:
                FindObjectOfType<LuckyCube>().PlayLuckyCube();
                break;
            case CASINOELEMENT.CARDS:
                FindObjectOfType<CardController>().Start_Cards();
                break;
        }
    }

    public bool isFirstCollectToItem()
    {
        if (PlayerPrefs.GetString("FirstCollected", "") == "OK")
        {
            return false;
        }
        else
            return true;
    }

    public void SetFirstCollectedItem()
    {
        Invoke("SetValue", 0.3f);
    }

    private void SetValue()
    {
        PlayerPrefs.SetString("FirstCollected", "OK");
    }
}
