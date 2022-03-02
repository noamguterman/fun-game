using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level : MonoBehaviour
{
    private List<EndlessLevelPart> endlessLevelParts;

    private List<LevelStruct> allLevelStructs = new List<LevelStruct>();

    private List<DisposableLine> disposableLines = new List<DisposableLine>();

    public static Level Instance;

    private void Awake()
    {
        Instance = this;

        endlessLevelParts = GetComponentsInChildren<EndlessLevelPart>().ToList();

        disposableLines = GetComponentsInChildren<DisposableLine>().ToList();

        InitStructs();

        if(GameManager.Instance.LevelsCompleted > 1)
        {
            if (GameManager.Instance.LevelsCompleted % 4 == 3 && GameManager.Instance.LevelsCompleted != 0)
            {
                BGMManager.Instance.SetBGM(1);
                CreateCoins();
            }
            else
            {
                BGMManager.Instance.SetBGM(0);
                CreateCoins();
                CreateTeleports();
            }
        }
    }

    private void InitStructs()
    {
        int i = 0;

        foreach (var levelPart in endlessLevelParts)
        {
            foreach (var levelStruct in levelPart.GetLevelStructs())
            {
                levelStruct.Id = i;
                i++;

                allLevelStructs.Add(levelStruct);
            }
        }
    }

    public void PutPlayerToStartPlace()
    {
        endlessLevelParts.First().PutPlayerToStartPlace();
    }

    public int GetStructsCount()
    {
        int levelStructs = endlessLevelParts.Sum(elp => elp.GetLevelStructs().Count);

        return levelStructs;
    }

    public void CreateProtectors()
    {
        var protector = Resources.LoadAll<Protector>("Other");

        var firstStruct = GetSctructById(0);
        var protectorPosition = firstStruct.transform.position + 0.4f * Vector3.up;

        for (int i = 0; i < 2; i++)
        {
            GameObject prot = Instantiate(protector[0].gameObject, Vector3.zero, Quaternion.identity);

            protectorPosition.x = i == 0 ? -2.5f : 2.5f;

            prot.transform.position = protectorPosition;
        }
    }

    public void CreateTeleports()
    {
        var teleportLoaded = Resources.LoadAll<Teleport>("Other");

        int lastStructId = GetLastStruct().Id;

        List<LevelStruct> usedLevelStructs = new List<LevelStruct>();      

        List<LevelStruct> availableStructs = allLevelStructs.Where(str => str.Id != 0 && str.Id < lastStructId - 5 && str.HasSolidHorizontalLine).ToList();

        int maxTeleportsAmount = availableStructs.Count > GameConfig.MAX_TELEPORTS_PER_LEVEL ? Random.Range(0, GameConfig.MAX_TELEPORTS_PER_LEVEL + 1) : availableStructs.Count;

        for (int i = 0; i < maxTeleportsAmount; i++)
        {
            LevelStruct randomLevelStruct = availableStructs[Random.Range(0, availableStructs.Count)];

            while (usedLevelStructs.Contains(randomLevelStruct))
            {
                randomLevelStruct = availableStructs[Random.Range(0, availableStructs.Count)];
            }

            Teleport teleportInstantiated = Instantiate(teleportLoaded[0].gameObject, randomLevelStruct.transform).GetComponent<Teleport>();

            teleportInstantiated.transform.position = RamdomizePositionWithinStruct(randomLevelStruct);

            teleportInstantiated.SkipedStructs = Random.Range(GameConfig.MIN_TELEPORT_SKIPPED_STRUCTS, GameConfig.MAX_TELEPORT_SKIPPED_STRUCTS);

            randomLevelStruct.HasTelepor = true;

            usedLevelStructs.Add(randomLevelStruct);
        }
    }

    private Vector3 RamdomizePositionWithinStruct(LevelStruct randomLevelStruct)
    {
        float xPos = randomLevelStruct.transform.position.x + Random.Range(-1f, 1f);
        float yPos = randomLevelStruct.transform.position.y + 0.4f;

        return new Vector3(xPos, yPos, 0);
    }

    public void CreateCoins()
    {
        var coinLoaded = Resources.LoadAll<Coin>("Other");

        int lastStructId = GetLastStruct().Id;

        List<LevelStruct> usedLevelStructs = new List<LevelStruct>();

        List<LevelStruct> availableStructs = allLevelStructs.Where(str => str.Id != 0 && str.Id < lastStructId - 1 && str.HasSolidHorizontalLine && !str.HasTelepor).ToList();

        int coinsAmount = Random.Range(0, availableStructs.Count);

        if (coinsAmount > GameConfig.MAX_COINS_PER_LEVEL)
        {
            coinsAmount = GameConfig.MAX_COINS_PER_LEVEL;
        }

        for (int i = 0; i < coinsAmount; i++)
        {
            LevelStruct randomLevelStruct = availableStructs[Random.Range(0, availableStructs.Count)];

            while (usedLevelStructs.Contains(randomLevelStruct))
            {
                randomLevelStruct = availableStructs[Random.Range(0, availableStructs.Count)];
            }

            Vector3 coinPosition = RamdomizePositionWithinStruct(randomLevelStruct);

            Coin coinInstantiated = Instantiate(coinLoaded[0].gameObject, randomLevelStruct.transform).GetComponent<Coin>();

            coinInstantiated.transform.position = coinPosition;

            usedLevelStructs.Add(randomLevelStruct);
        }
    }

    public LevelStruct GetLastStruct()
    {
        return endlessLevelParts.Last().GetLevelStructs().Last();
    }

    public void SetLinesColor(Color color)
    {
        foreach (var levelPart in endlessLevelParts)
        {
            levelPart.SetLinesColor(color);
        }
    }

    public void SetStructPointsTextsColor(Color color)
    {
        foreach (var levelPart in endlessLevelParts)
        {
            levelPart.SetStructsPointTextColor(color);
        }
    }

    public List<int> GetLevelPartsIDs()
    {
        return endlessLevelParts.Select(levelPart => levelPart.Id).ToList();
    }

    public LevelStruct GetNearestClosedLevelStruct(int lastTriggeredStructId)
    {
        LevelStruct nearestClosedStruct = allLevelStructs
            .Where(str => str.IsClosedStruct && str.Id <= lastTriggeredStructId)
            .OrderBy(str => str.Id)
            .LastOrDefault();

        if (nearestClosedStruct == null)
        {
            nearestClosedStruct = allLevelStructs.First();
        }

        return nearestClosedStruct;
    }

    public void ResetDisposableLines()
    {
        foreach (var disposableLine in disposableLines)
        {
            disposableLine.ResetDispose();
        }
    }

    public LevelStruct GetSctructById(int id)
    {
        return allLevelStructs.First(str => str.Id == id);
    }

    public bool IsProtectorsAvailable()
    {
        return endlessLevelParts.All(levelPart => allLevelStructs.All(str => !str.IsTransitionPlatform));
    }

    public List<LevelStruct> GetStructsWithSolidHorizontalLines()
    {
        return allLevelStructs.Where(str => str.HasSolidHorizontalLine).ToList();
    }
}