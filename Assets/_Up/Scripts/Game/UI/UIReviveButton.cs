using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIReviveButton : MonoBehaviour
{
    [SerializeField] private float reviveTime;
    [SerializeField] private TextMeshProUGUI timer;

    private void OnEnable()
    {
        gameObject.SetActive(true);

        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        timer.text = string.Format("00:0{0}", reviveTime);

        for (float t = 1.0f; t > 0.0f; t -= Time.deltaTime / reviveTime)
        {
            timer.text = string.Format("00:0{0}", (int)(t* reviveTime) + 1);

            yield return null;
        }

        SceneManager.LoadScene("Game");
    }
}