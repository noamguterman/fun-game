using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    public static PromptManager Instance;

    [SerializeField] PromptDatabase promptDatabase;
    [SerializeField] GameObject promptPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void SetPrompts()
    {
        int currentLevelId = GameManager.Instance.LevelsCompleted + 1;

        Level currentLevel = Level.Instance;

        switch (currentLevelId)
        {
            case 1:
                {
                    List<LevelStruct> suitableStructs = currentLevel.GetStructsWithSolidHorizontalLines();

                    if (suitableStructs.Count > 0)
                    {
                        List<LevelStruct> moreSuitableStructs = suitableStructs.Where(str => str.Id < 4).ToList();

                        LevelStruct finalStruct = null;

                        if (suitableStructs.Count == 0)
                        {
                            finalStruct = suitableStructs.First();
                        }
                        else
                        {
                            finalStruct = moreSuitableStructs.Last();
                        }

                        TextMeshPro prompt = Instantiate(promptPrefab, finalStruct.transform.position + 0.425f * Vector3.up, Quaternion.identity).GetComponent<TextMeshPro>();

                        prompt.text = promptDatabase.GetPrompt(PromptPhrase.TapToJump);
                    }
                    break;
                }
            case 2:
                {
                    List<LevelStruct> suitableStructs = currentLevel.GetStructsWithSolidHorizontalLines();

                    if (suitableStructs.Count > 0)
                    {
                        List<LevelStruct> moreSuitableStructs = suitableStructs.Where(str => str.Id < 4).ToList();

                        LevelStruct finalStruct = null;

                        if (suitableStructs.Count == 0)
                        {
                            finalStruct = suitableStructs.First();
                        }
                        else
                        {
                            finalStruct = moreSuitableStructs.Last();
                        }

                        TextMeshPro prompt = Instantiate(promptPrefab, finalStruct.transform.position + 0.425f * Vector3.up, Quaternion.identity).GetComponent<TextMeshPro>();

                        prompt.text = promptDatabase.GetPrompt(PromptPhrase.TapFast);
                    }
                    break;
                }
        }
    }
}