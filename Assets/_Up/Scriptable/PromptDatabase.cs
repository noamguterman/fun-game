using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PromptDatabase", menuName = "PromptDatabase")]
public class PromptDatabase : ScriptableObject
{
    public Prompt[] PromptPhrases;

    public string GetPrompt(PromptPhrase promptKey)
    {
        return PromptPhrases.First(prompt => prompt.PromptKey == promptKey).PromptStr;
    }
}

[System.Serializable]
public struct Prompt
{
    public PromptPhrase PromptKey;

    public string PromptStr;
}