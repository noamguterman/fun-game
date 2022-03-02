using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackText : MonoBehaviour
{
    public static FeedbackText instance;
    private Text txt;
    private Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        txt = this.gameObject.GetComponent<Text>();
        anim = this.gameObject.GetComponent<Animator>();
        txt.text = "";
    }

    public void ShowFeedback(string message)
    {
        //txt.text = message;
        //anim.Play("Show");
    }
}
