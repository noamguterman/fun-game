using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class Test : MonoBehaviour {
    private void Update()
    {
        //Camera.main.transform.position += new Vector3(2, 2, 0) * Time.deltaTime;
        transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, Time.frameCount * -0.001f);
    }
}
