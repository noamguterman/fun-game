using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfter10 : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Deactivate", 10f);
    }

    private void Deactivate()
    {
        //this.gameObject.SetActive(false);
    }
}
