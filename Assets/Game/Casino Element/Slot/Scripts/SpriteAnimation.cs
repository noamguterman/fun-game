using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private float timeIterval = 0.5f;

    float time;
    int idx; 
    void Start()
    {
        if (image == null)
            image = transform.GetComponent<Image>();
        time = Time.time;
        idx = 0;
    }

    // Update is called once per frame

    void Update()
    {
        if((Time.time - time) > timeIterval)
        {
            time = Time.time;
            idx++;
            if (idx >= sprites.Length)
                idx = 0;

            image.sprite = sprites[idx];
        }
    }
}
