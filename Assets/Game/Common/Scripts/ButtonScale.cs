using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonScale : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }
    void Start()
    {
        
    }

    private void OnEnable()
    {
        transform.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
    }
}
