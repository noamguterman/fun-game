using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public UIPanelType PanelType;

    [SerializeField] private Image background;

    public virtual void Init()
    {

    }

    public void SetDefaultBackgroundColor()
    {
        if (background != null)
        {
            background.color = ColorManager.Instance.CurrentBackgroundColor;
        }
    }

    public void SetSpecificBackgroundColor(Color color)
    {
        if (background != null)
        {
            background.color = color;
        }
    }

    public virtual void SetActive(bool enabled)
    {
        gameObject.SetActive(enabled);
    }

    internal object ToList()
    {
        throw new NotImplementedException();
    }
}