using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InitScene : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync("Game");
    }
}