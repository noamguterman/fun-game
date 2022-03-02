using UnityEngine;

[CreateAssetMenu(fileName = "RandomRewardText", menuName = "RandomRewardText")]
public class RandomRewardText : ScriptableObject
{
    public string[] randomText;

    public string GetRandomText()
    {
        return randomText[Random.Range(0, randomText.Length)];
    }
}