using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioSource bgms0;
    public AudioSource bgms1;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetBGM(int idx)
    {
        if(idx == 0)
        {
            bgms0.enabled = true;
            bgms1.enabled = false;
        }
        else
        {
            bgms0.enabled = false;
            bgms1.enabled = true;
        }
        //transform.GetComponent<AudioSource>().Play();
    }

    public void SetVolume(bool state)
    {
        if (state)
        {
            bgms0.volume = 1;
            bgms1.volume = 1;
        }
        else
        {
            bgms0.volume = 0;
            bgms1.volume = 0;
        }
        
    }
}
