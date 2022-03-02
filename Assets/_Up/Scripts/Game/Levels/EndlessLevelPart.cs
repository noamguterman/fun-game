using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EndlessLevelPart : MonoBehaviour
{
    public int Id
    {
        get
        {
            return id;
        }
    }

    [SerializeField] private int id;

    [SerializeField] private Transform startPoint;
    [SerializeField] private TextMeshPro levelCompletedText;

    private List<LevelStruct> levelStructs;
    private List<SpriteRenderer> levelRenderersOfLines;
    private List<Teleport> teleports;

    private void Awake()
    {
        levelStructs = GetComponentsInChildren<LevelStruct>().ToList();
        levelRenderersOfLines = GetComponentsInChildren<SpriteRenderer>().Where(r => r.tag.Contains("Line")).ToList();
        teleports = GetComponentsInChildren<Teleport>().ToList();

        levelCompletedText.text = string.Empty;
    }

    public void PutPlayerToStartPlace()
    {
        Player.Instance.transform.position = startPoint.position;
    }

    public bool IsLastStruct(LevelStruct levelStruct)
    {
        bool isLastStruct = levelStruct == levelStructs.Last();

        return isLastStruct;
    }

    public int GetStructsCount()
    {
        return levelStructs.Count;
    }

    public LevelStruct GetFirstStruct()
    {
        return levelStructs.First();
    }

    public LevelStruct GetLastStruct()
    {
        return levelStructs.Last();
    }

    public void SetLinesColor(Color color)
    {
        foreach (var lineRenderer in levelRenderersOfLines)
        {
            lineRenderer.color = color;
        }
    }

    public void SetStructsPointTextColor(Color color)
    {
        foreach (var levelStruct in levelStructs)
        {
            levelStruct.SetStructPointTextColor(color);
        }
    }

    public List<LevelStruct> GetLevelStructs()
    {
        return levelStructs;
    }

    public void SetLevelCompletedLabel(int level)
    {
        levelCompletedText.text = string.Format("LEVEL {0} \n COMPLETED", level);
    }

    public void SetTeleportsActive(bool enabled)
    {
        foreach (var teleport in teleports)
        {
            teleport.gameObject.SetActive(enabled);
        }
    }
}