using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Best : MonoBehaviour
{ 
    private Text txt;

    void Start()
    {
        txt = this.gameObject.GetComponent<Text>();
    }


    void Update()
    {
        txt.text = "" + ScoreManager.Instance.Best;
    }
}
