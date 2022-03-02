using UnityEngine;
using UnityEngine.UI;

public class UIGameModeButton : MonoBehaviour
{
    [SerializeField] private GameMode mode;

    [SerializeField] private Image checkMark;

    private void Start()
    {
        checkMark.enabled = GameManager.Mode == mode;
    }
}