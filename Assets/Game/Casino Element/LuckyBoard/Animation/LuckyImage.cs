using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckyImage : MonoBehaviour
{
    public Sprite[] spr;

    private Image img;


    void Start()
    {
        img = this.GetComponent<Image>();
        img.sprite = spr[0];
    }

    public void OnImage()
    {
        img.sprite = spr[1];
    }

    public void OffImage()
    {
        img.sprite = spr[0];
    }
}
