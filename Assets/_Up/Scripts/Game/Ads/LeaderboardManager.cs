using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CloudOnce;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;
    public bool isInitialized = false;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private System.Action pcb;
    public void Init(System.Action cb)
    {
        if (isInitialized)
        {
            cb();
            return;
        }
        pcb = cb;
        Cloud.OnInitializeComplete += CloudOnceInitializeComplete;
        Cloud.Initialize(false, true);
    }

    public void CloudOnceInitializeComplete()
    {
        isInitialized = true;
        Cloud.OnInitializeComplete -= CloudOnceInitializeComplete;
        Debug.Log("~~Leaderboard Initialized");
        pcb();
    }

    public void SubmitMyLevel()
    {
        if(isInitialized == true)
            Leaderboards.MyGameHighLevel.SubmitScore(GameManager.Instance.LevelsCompleted);
    }
}
