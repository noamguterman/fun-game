using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    public enum ItemType
    {
        score = 0,
        jackPot = 1,
        collectIcon = 2,
    }
    public static ChallengeManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;


        ChallengeManager.Instance.Init((level) =>
        {
            print(string.Format("completed level {0}", ChallengeManager.Instance.currentLevel));



            ChallengeManager.Instance.currentLevel++;
        }, (itemType) => {
            //progress increase           
            var curlevel = GetLevelInfo(currentLevel);
            if (curlevel !=null && curlevel.Keys.Count > 0)
            {
                float sum = 0f;
                foreach (var child in curlevel.Keys)
                {
                    sum += Mathf.Clamp01(Get((ItemType)child) / (float)curlevel[child]);
                }
                float progress = (sum / (float)curlevel.Keys.Count) * 100f;
                print(string.Format("current level progress is {0}%  changed {1}", progress, itemType));
            }
        });


        //currentLevel = 1;
        //ChallengeManager.Instance.UpdateValue(ItemType.score, +10);

        //ChallengeManager.Instance.UpdateValue(ItemType.score, +10);
        //ChallengeManager.Instance.UpdateValue(ItemType.score, +10);
        //ChallengeManager.Instance.UpdateValue(ItemType.jackPot, +3);

        //ChallengeManager.Instance.UpdateValue(ItemType.score, +10);
        //ChallengeManager.Instance.UpdateValue(ItemType.jackPot, +1);
        //ChallengeManager.Instance.UpdateValue(ItemType.jackPot, +1);
        //ChallengeManager.Instance.UpdateValue(ItemType.score, +20);
        //ChallengeManager.Instance.UpdateValue(ItemType.jackPot, +3);


        //ChallengeManager.Instance.UpdateValue(ItemType.score, +100);
        //ChallengeManager.Instance.UpdateValue(ItemType.score, +100);
        //ChallengeManager.Instance.UpdateValue(ItemType.score, +500);
        //ChallengeManager.Instance.UpdateValue(ItemType.jackPot, +3);


        //ChallengeManager.Instance.UpdateValue(ItemType.score, +1500);

        //ChallengeManager.Instance.UpdateValue(ItemType.score, +1500);
    }



    #region private 
    private Dictionary<ItemType, bool> gotValues = new Dictionary<ItemType, bool>();
    private Dictionary<ItemType, int> itemSet = new Dictionary<ItemType, int>();
    public System.Action<int> On_CompletedLevel;
    public System.Action<ItemType> On_LevelProgress;
    private int pcurrentLevel = 0;
    public int currentLevel
    {
        get { return pcurrentLevel; }
        set
        {
            PlayerPrefs.SetInt("curChanllenge", value);
            pcurrentLevel = value;
            gotValues = new Dictionary<ItemType, bool>();
            itemSet = new Dictionary<ItemType, int>();
        }
    }
    public void Init(System.Action<int> on_completedlevel,System.Action<ItemType> levelProgress)
    {
        On_CompletedLevel = on_completedlevel;
        On_LevelProgress = levelProgress;
        readData();
        currentLevel = PlayerPrefs.GetInt("curChanllenge", 1);
        if (currentLevel < 0) currentLevel = 1;
    }
    
    
    #region CSV Reader
    public TextAsset csvFile; // Reference of CSV file
    private char lineSeperater = '\n'; // It defines line seperate character
    private char fieldSeperator = ','; // It defines field seperate chracter
    private Dictionary<int, Dictionary<int, int>> levelsinfo = new Dictionary<int, Dictionary<int, int>>();

    private bool intParse(string value,System.Action<int> parseAction)
    {
        int pv = 0;
        if (int.TryParse(value, out pv))
        {
            parseAction(pv);
            return true;
        }
        return false;
    }
    // Read data from CSV file
    private void readData()
    {
        levelsinfo = new Dictionary<int, Dictionary<int, int>>();
        string[] records = csvFile.text.Split(lineSeperater);
        for (int i=1;i< records.Length;i++)
        {
            var record = records[i];
            string[] fields = record.Split(fieldSeperator);
            levelsinfo.Add(i, new Dictionary<int, int>());
            intParse(fields[0], (level) =>
            {
                for (int j = 1; j < fields.Length; j++)
                {
                    var field = fields[j];
                    if (string.IsNullOrEmpty(field.Trim()) == false)
                    {
                        int pv = 0;
                        if (int.TryParse(field, out pv))
                        {
                            levelsinfo[level].Add(j-1, pv);
                            //Debug.Log(string.Format("{0},{1}={2}", level,j-1,levelsinfo[level][j - 1]));
                        }
                        else
                        {
                            UnityEngine.Debug.LogError(string.Format("({0},{1}) value is invalid.", i, j));
                        }
                    }
                }
            });
        }
    }
    #endregion

    public Dictionary<int, int> GetLevelInfo(int level)
    {
        Dictionary<int, int> levelinfo = null;
        if (levelsinfo.TryGetValue(level, out levelinfo))
        {
            return levelinfo;
        }
        return null;
    }
    public bool GotItem(ItemType type)
    {
        if (gotValues.ContainsKey(type)){
            gotValues[type] = true;
        }
        else
        {
            gotValues.Add(type, true);
        }

        Dictionary<int, int> levelinfo = GetLevelInfo(currentLevel);
        foreach (var itemtype in levelinfo.Keys)
        {
            if (gotValues.ContainsKey((ItemType)itemtype)){
                if (gotValues[(ItemType)itemtype] == false) return false;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    protected void on_update(ItemType type,int from,int to)
    {
        Dictionary<int, int> levelinfo = GetLevelInfo(currentLevel);
        if (levelinfo!=null)
        {
            int lv = 0;
            if (levelinfo.TryGetValue((int)type,out lv))
            {
                On_LevelProgress(type);
                if (to>=lv)
                {
                    if (GotItem(type))
                    {
                        On_CompletedLevel(currentLevel);
                    }
                }
            }
        }
    }
    public ChallengeManager UpdateValue(ItemType type, int value,int defaultValue = 0)
    {
        int oldv = Get(type, defaultValue);
        int newv = oldv + value;
        Set(type, newv);
        on_update(type, oldv, newv);
        return this;
    }
    public ChallengeManager Set(ItemType type,int value)
    {
        if (itemSet.ContainsKey(type))
        {
            itemSet[type] = value;
        }
        else
        {
            itemSet.Add(type, value);
        }
        return this;
    }
    public int Get(ItemType type,int defaultValue=0)
    {
        int v = 0;
        if (itemSet.TryGetValue(type,out v))
        {
            return v;
        }
        else
        {
            Set(type, defaultValue);
            return defaultValue;
        }
    }
    #endregion
}
