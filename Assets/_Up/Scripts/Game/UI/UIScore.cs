using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        SetScore(GameData.CurrentScore);
    }

    private void OnEnable()
    {
        GameData.CurrentScoreChanged += SetScore;
    }

    private void OnDisable()
    {
        GameData.CurrentScoreChanged -= SetScore;
    }

    public void SetScore(int points)
    {
        textMesh.text = points.ToString();
    }
}