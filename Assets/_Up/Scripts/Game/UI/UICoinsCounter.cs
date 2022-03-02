using TMPro;
using UnityEngine;

public class UICoinsCounter : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        SetCoins(GameData.Coins);
    }

    private void OnEnable()
    {
        GameData.CoinsAmountChanged += SetCoins;
    }

    private void OnDisable()
    {
        GameData.CoinsAmountChanged -= SetCoins;
    }

    public void SetCoins(int coins)
    {
        textMesh.text = coins.ToString();
    }
}