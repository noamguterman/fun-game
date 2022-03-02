using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BingoCollider : MonoBehaviour
{
    public static BingoCollider instance;

    public Animator bingoboard;

    private void Awake()
    {
        instance = this;
    }

    public void PlayBingoBoard()
    {
        bingoboard.Play("LuckyHighlight");
    }

    public void Update()
    {
        this.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y + 2, 0);
    }
}
