using Assets._Scripts.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject buildingNextLevel_Panel;
    private Dictionary<UIPanelType, UIPanel> gamePanels = new Dictionary<UIPanelType, UIPanel>();

    public UIPanelType CurrentPanel
    {
        get;
        private set;
    }

    public event Action<UIPanelType> PanelChanged;

    private void Awake()
    {
        Instance = this;

        CurrentPanel = UIPanelType.GamePanel;
    }

    public void InitPanels()
    {
        var panels = GetComponentsInChildren<UIPanel>(true);

        foreach (var panel in panels)
        {
            gamePanels.Add(panel.PanelType, panel);
            panel.Init();
        }
    }

    public void ActivatePanel(UIPanelType panelType)
    {
        CurrentPanel = panelType;

        foreach (var panel in gamePanels)
        {
            panel.Value.SetActive(panel.Key == panelType);
        }

        PanelChanged.SaveInvoke(panelType);
    }

    public void DeactivatePanels()
    {
        foreach (var panel in gamePanels)
        {
            panel.Value.SetActive(false);
        }
    }

    public T GetPanel<T>() where T : UIPanel
    {
        if (typeof(T) == typeof(UIGamePanel))
        {
            return (UIGamePanel)gamePanels[UIPanelType.GamePanel] as T;
        }
        else if (typeof(T) == typeof(UILosePanel))
        {
            return (UILosePanel)gamePanels[UIPanelType.LosePanel] as T;
        }
        else if (typeof(T) == typeof(UITimeOverPanel))
        {
            return (UITimeOverPanel)gamePanels[UIPanelType.TimeOverPanel] as T;
        }
        else if (typeof(T) == typeof(UIRatePanel))
        {
            return (UIRatePanel)gamePanels[UIPanelType.RatePanel] as T;
        }
        else if (typeof(T) == typeof(UIRewardPanel))
        {
            return (UIRewardPanel)gamePanels[UIPanelType.RewardPanel] as T;
        }
        else if (typeof(T) == typeof(UISettingsPanel))
        {
            return (UISettingsPanel)gamePanels[UIPanelType.SettingsPanel] as T;
        }
        else if (typeof(T) == typeof(UIShopPanel))
        {
            return (UIShopPanel)gamePanels[UIPanelType.ShopPanel] as T;
        }
        else if (typeof(T) == typeof(UICasinoPanel))
        {
            return (UICasinoPanel)gamePanels[UIPanelType.CasinoPanel] as T;
        }

        //return default;
        return (UIGamePanel)gamePanels[UIPanelType.GamePanel] as T;
    }

    public void SetPanelsBackgroundsColor()
    {
        foreach (var panel in gamePanels.Values)
        {
            if (panel.PanelType != UIPanelType.RewardPanel)
            {
                panel.SetDefaultBackgroundColor();
            }
            else
            {
                Color color = ColorManager.Instance.CurrentWinExplosionColor;

                color.a = 1;

                panel.SetSpecificBackgroundColor(color);
            }
        }
    }
}