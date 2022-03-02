using Assets._Scripts.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameMode Mode
    {
        get;
        set;
    }

    public GameState State
    {
        get
        {
            return gameState;
        }
        set
        {
            gameState = value;

            Time.timeScale = gameState == GameState.Paused ? 0 : 1;
        }
    }

    private GameState gameState;

    public int LevelsCompleted
    {
        get
        {
            return PlayerPrefs.GetInt("LevelsCompleted");
        }
        set
        {
            PlayerPrefs.SetInt("LevelsCompleted", value);
            LeaderboardManager.Instance.SubmitMyLevel();
        }
    }

    public int LevelTime
    {
        get
        {
            if (LevelsCompleted != 0 && LevelsCompleted % 4 == 3)
            {
                return PlayerPrefs.GetInt("LevelTime", 8);
            }
            else
            {
                return -1;
            }
        }
        set
        {
            PlayerPrefs.SetInt("LevelTime", value);
        }
    }

    public bool IsGameRated
    {
        get
        {
            return PlayerPrefs.GetInt("IsRated") == 1;
        }
        set
        {
            PlayerPrefs.SetInt("IsRated", value ? 1 : 0);
        }
    }

    private readonly int[] rateAppAfterLevels =
    {
        6, 26
    };

    public static List<EndlessLevelPart> EndlessLevelParts = new List<EndlessLevelPart>();
    public static List<EndlessLevelPart> SpecialLevelParts = new List<EndlessLevelPart>();

    private static List<int> storedLevelParts;

    private Level instantiatedLevel;

    List<EndlessLevelPart> instantiatedParts;

    private bool isFirstTapPassed;
    private bool isReviveUsed;

    public static GameManager Instance;

    public static int deathCount = 0;

    [HideInInspector] public bool isBuildingtime = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (Mode == GameMode.Levels)
        {
            LoadLevelParts();
            CreateLevel();

            UIManager.Instance.InitPanels();
            UIManager.Instance.GetPanel<UIGamePanel>().ProgressBar.Init();
            UIManager.Instance.GetPanel<UIGamePanel>().TimerBar.Init();

            InitCommonEvents();
            InitLevelsEvents();

            PromptManager.Instance.SetPrompts();

            //if(LevelsCompleted == 24)
            //{
            //    System.DateTime currentTime = System.DateTime.FromBinary(System.Convert.ToInt64(PlayerPrefs.GetString("SetNextLevelTime")));
            //    if(currentTime.Subtract(System.DateTime.Now).TotalMilliseconds > 1)
            //    {
            //        State = GameState.Paused;
            //        UIManager.Instance.buildingNextLevel_Panel.SetActive(true);
            //    }
            //}
        }
        else if (Mode == GameMode.Endless)
        {
            LoadLevelParts();
            CreateEndlessLevel();

            UIManager.Instance.InitPanels();
            UIManager.Instance.GetPanel<UIGamePanel>().ProgressBar.SetActive(false);
            UIManager.Instance.GetPanel<UIGamePanel>().TimerBar.SetActive(false);

            InitCommonEvents();
        }

        ShopManager.Instance.Init();

        State = GameState.Alive;

        StartAnalyticsCall();

        //if(Mode == GameMode.Levels)
        //{
        //    if (LevelsCompleted == 24)
        //    {
        //        System.DateTime currentTime = System.DateTime.FromBinary(System.Convert.ToInt64(PlayerPrefs.GetString("SetNextLevelTime")));
        //        if (currentTime.Subtract(System.DateTime.Now).TotalMilliseconds > 1)
        //        {
        //            State = GameState.Paused;
        //            UIManager.Instance.buildingNextLevel_Panel.SetActive(true);
        //            isBuildingtime = true;
        //        }
        //    }
        //}
    }

    private void StartAnalyticsCall()
    {
        string message = string.Empty;

        if (Mode == GameMode.Levels && LevelsCompleted == 0)
        {
            //ServiceProvider.Analytics.SendEvent("started_level_1");
        }
        else if (Mode == GameMode.Endless)
        {
            message = string.Format("started_endless_mode");

            //ServiceProvider.Analytics.SendEvent(message);
        }
        else if (Mode == GameMode.Sprint)
        {
            message = string.Empty;
        }     
    }

    private void InitLevelsEvents()
    {
        Player.Instance.CollisionsHandler.LastStructReached += OnLevelCompleted;
    }

    private void InitCommonEvents()
    {
        Player.Instance.PlayerDead += OnPlayerDead;

        Player.Instance.Movement.PlayerJump += UIManager.Instance.GetPanel<UILosePanel>().OnPlayerJump;

        UIManager.Instance.GetPanel<UILosePanel>().ReviveButtonPressed += OnReviveButtonPressed;

        UIManager.Instance.PanelChanged += OnPanelChanged;
    }

    private void OnPanelChanged(UIPanelType panelType)
    {
        if (isFirstTapPassed && panelType == UIPanelType.GamePanel)
        {
            //Debug.LogError("AdsManager.Instance.ShowBanner();");
        }
        else
        {
            //Debug.LogError("AdsManager.Instance.HideBanner();");
        }
    }

    private void LoadLevelParts()
    {
        if (EndlessLevelParts.Count == 0)
        {
            var levelParts = Resources.LoadAll(GameConfig.PATH_TO_SRUCTS_COMBINATIONS, typeof(GameObject));

            foreach (var level in levelParts)
            {
                EndlessLevelParts.Add((level as GameObject).GetComponent<EndlessLevelPart>());
            }

            EndlessLevelParts = EndlessLevelParts.OrderBy(level => level.Id).ToList();
        }

        if (SpecialLevelParts.Count == 0)
        {
            var levelParts = Resources.LoadAll(GameConfig.PATH_TO_SPECIAL_SRUCTS_COMBINATIONS, typeof(GameObject));

            foreach (var level in levelParts)
            {
                SpecialLevelParts.Add((level as GameObject).GetComponent<EndlessLevelPart>());
            }

            SpecialLevelParts = SpecialLevelParts.OrderBy(level => level.Id).ToList();
        }
    }

    private void CreateLevel()
    {
        GameObject level = new GameObject("Level");

        Vector3 creationPosition = Vector3.zero;

        instantiatedParts = new List<EndlessLevelPart>();

        int maxCountPartsToUse = GetCountPartsToUse();
        int countPartsToCreate = 0;
        Debug.Log(LevelsCompleted);

        if (LevelsCompleted == 0)
        {
            countPartsToCreate = 1;
        }
        else
        {
            countPartsToCreate = GetCountPartsToCreate();
        }

        List<EndlessLevelPart> endlessLevelParts = EndlessLevelParts;

        if (LevelsCompleted > 8 && LevelsCompleted % GameConfig.SPECIAL_LEVELS_FREQUENCY == 1)
        {
            endlessLevelParts = SpecialLevelParts;
        }

        for (int i = 0; i < countPartsToCreate; i++)
        {
            EndlessLevelPart randomPart = null;

            if (storedLevelParts != null)
            {
                randomPart = endlessLevelParts.First(levelPart => levelPart.Id == storedLevelParts[i]);
            }
            else
            {
                randomPart = endlessLevelParts.ElementAt(Random.Range(1, maxCountPartsToUse));
            }

            if(LevelsCompleted < 2)
            {
                randomPart = endlessLevelParts[0];
            }
            EndlessLevelPart instantiatedRandomPart = Instantiate(randomPart.gameObject, level.transform).GetComponent<EndlessLevelPart>();

            if (instantiatedParts.Count > 0)
            {
                EndlessLevelPart previousLevelPart = instantiatedParts.Last();

                if (previousLevelPart.GetLevelStructs().Any(str => str.IsTransitionPlatform))
                {
                    creationPosition.x = previousLevelPart.GetLastStruct().transform.position.x;
                }
            }

            instantiatedRandomPart.transform.position = creationPosition;

            float offsetY = instantiatedRandomPart.GetStructsCount() * GameConfig.STRUCTS_DISTANCE;

            creationPosition += new Vector3(0, offsetY, 0);

            instantiatedParts.Add(instantiatedRandomPart);
        }

        EndlessLevelPart firstLevelPart = instantiatedParts.First();
        EndlessLevelPart lastLevelPart = instantiatedParts.Last();

        instantiatedLevel = level.AddComponent<Level>();

        firstLevelPart.GetFirstStruct().IsTriggered = true;

        Player.Instance.CollisionsHandler.LastTriggeredStruct = firstLevelPart.GetFirstStruct();

        lastLevelPart.GetLastStruct().InitAsLastStruct();
        lastLevelPart.SetLevelCompletedLabel(LevelsCompleted + 1);

        FXManager.Instance.SetLastStructParticlePosition();
        ColorManager.Instance.SetLevelColors();

        lastLevelPart.SetTeleportsActive(false);

        instantiatedLevel.PutPlayerToStartPlace();
    }

    private void CreateEndlessLevel()
    {
        GameObject level = new GameObject("Level");

        Vector3 creationPosition = Vector3.zero;

        instantiatedParts = new List<EndlessLevelPart>();

        for (int i = 0; i < GameConfig.ENDLESS_LEVELS_CREATING_PARTS; i++)
        {
            EndlessLevelPart randomPart = EndlessLevelParts.ElementAt(Random.Range(0, EndlessLevelParts.Count));

            EndlessLevelPart instantiatedRandomPart = Instantiate(randomPart.gameObject, level.transform).GetComponent<EndlessLevelPart>();

            instantiatedRandomPart.transform.position = creationPosition;

            float offsetY = instantiatedRandomPart.GetStructsCount() * GameConfig.STRUCTS_DISTANCE;

            creationPosition += new Vector3(0, offsetY, 0);

            instantiatedParts.Add(instantiatedRandomPart);
        }

        EndlessLevelPart firstLevelPart = instantiatedParts.First();

        instantiatedLevel = level.AddComponent<Level>();
        ColorManager.Instance.SetLevelColors();

        firstLevelPart.GetFirstStruct().IsTriggered = true;

        instantiatedLevel.PutPlayerToStartPlace();
    }

    public void DeactiveEndlessLevelPart(EndlessLevelPart curPart)
    {
        for (int i = 0; i < instantiatedParts.Count; i++)
        {
            if(curPart == instantiatedParts[i])
            {
                if (i - 2 >= 0)
                    instantiatedParts[i-2].transform.gameObject.SetActive(false);
            }
        }
    }
    private void Update()
    {
        EscapeButtonHandler();

        if (State == GameState.Paused)
        {
            return;
        }

        if (Player.Instance.State != PlayerState.NotActive && Player.Instance.State != PlayerState.Dead)
        {
            Player.Instance.Movement.Move();

#if UNITY_EDITOR

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    if (!EventSystem.current.currentSelectedGameObject)
                    {
                        TapHandler();
                    }
                }
                else
                {
                    TapHandler();
                }
            }

#elif UNITY_ANDROID || UNITY_IPHONE

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    if (!EventSystem.current.currentSelectedGameObject)
                    {
                        TapHandler();
                    }
                }
                else
                {
                    TapHandler();
                }
            }
#endif

        }
    }

    private void TapHandler()
    {
        Player.Instance.Movement.Jump();

        if (!isFirstTapPassed)
        {
            UIManager.Instance.GetPanel<UIGamePanel>().SetActiveBottomPanel(false);

            //Debug.LogError("AdsManager.Instance.ShowBanner();");

            isFirstTapPassed = true;
        }
        else
        {
            if (FindObjectOfType<UITimer>() != null)
                FindObjectOfType<UITimer>().isStartCountDown = true;
        }
    }

    private void EscapeButtonHandler()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.Instance.PlayButtonSFX();

            switch (UIManager.Instance.CurrentPanel)
            {
                case UIPanelType.SettingsPanel:
                case UIPanelType.ShopPanel:
                    {
                        UIManager.Instance.ActivatePanel(UIPanelType.GamePanel);
                        State = GameState.Alive;

                        break;
                    }
                case UIPanelType.GamePanel:
                    {
                        Application.Quit();

                        break;
                    }
            }
        }
    }

    private void FinalAnalyticsCall()
    {
        string message = string.Format("finished_level_{0}", LevelsCompleted + 1);

        //ServiceProvider.Analytics.SendEvent(message);
    }

    [HideInInspector] public bool reachedLastStructure = false;
    private void OnLevelCompleted()
    {
        if (Player.Instance.GetComponent<BoxCollider2D>().isTrigger == true)
            return;

        reachedLastStructure = true;
        storedLevelParts = null;

        StartCoroutine(RewardAndGoAhead());
    }

    private IEnumerator RewardAndGoAhead()
    {
        yield return new WaitForSeconds(0.05f);

        Player.Instance.State = PlayerState.NotActive;
        Player.Instance.PlayerEffects.SetTrailActive(false);

        while (!Player.Instance.Movement.IsCalm)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);

        LevelStruct lastStruct = instantiatedLevel.GetLastStruct();

        FXManager.Instance.PlayWinVFX(lastStruct.transform.position);
        SoundManager.Instance.PlayVictorySFX();

        int coinsReward = Random.Range(GameConfig.LEVEL_MIN_GEMS_REWARDED, GameConfig.LEVEL_MAX_GEMS_REWARDED);
        if(LevelsCompleted >= 30)
            coinsReward = Random.Range(100, 200);

        GameData.Coins += coinsReward;

        UIManager.Instance.GetPanel<UIGamePanel>().ShowReward(coinsReward);

        yield return new WaitForSeconds(1.5f);

        Player.Instance.Movement.StartFlyAway();

        yield return new WaitForSeconds(1f);

        FinalAnalyticsCall();

        PanelsConditions();
    }

    private void PanelsConditions()
    {
        int currentLevelId = LevelsCompleted + 1;

        LevelsCompleted++;
        if(LevelsCompleted == 24)
        {
            System.DateTime currentTime = System.DateTime.Now.Add(new System.TimeSpan(0, 6, 0, 0));
            PlayerPrefs.SetString("SetNextLevelTime", currentTime.ToBinary().ToString());
        }

        if(LevelsCompleted == 24)
        {
            UIManager.Instance.GetPanel<UICasinoPanel>().Show();
        }
        else if (rateAppAfterLevels.Any(id => id == currentLevelId) && !IsGameRated)
        {
            UIManager.Instance.ActivatePanel(UIPanelType.RatePanel);
        }
        //else if(currentLevelId == 3)
        //{
        //    UIManager.Instance.ActivatePanel(UIPanelType.ShopPanel);
        //}
        else if (currentLevelId > 5 && currentLevelId % 3 == 0 && ShopManager.Instance.HasCareerSkins())
        {
            UIManager.Instance.GetPanel<UIRewardPanel>().Show();
        }
        else if (currentLevelId % 3 == 2 && currentLevelId > 7)
        {
            UIManager.Instance.GetPanel<UICasinoPanel>().Show();
        }
        else
        {
            if(currentLevelId >= 10)
            {
                //AdsManager.Instance.ShowInterstitial(GlobalConstants.LevelComplete);
                Player.Instance.State = PlayerState.Alive;
                SceneManager.LoadScene("Game");
            }
            else
            {
                Player.Instance.State = PlayerState.Alive;
                SceneManager.LoadScene("Game");
            }
        }
    }

    public void OnTimeOver()
    {
        Debug.Log("OnTimeOver");
        Player.Instance.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnPlayerDead()
    {
        float structsCompleted = 0;

        if (Player.Instance.CollisionsHandler.LastTriggeredStruct != null)
        {
            structsCompleted = Player.Instance.CollisionsHandler.LastTriggeredStruct.Id;
        }

        if (Mode == GameMode.Levels)
        {
            if(LevelsCompleted % 4 == 3 && LevelsCompleted != 0)
            {
                float structsTotal = instantiatedLevel.GetLastStruct().Id;
                float percentCompleted = structsCompleted / structsTotal;

                if (percentCompleted >= 0.1f && Player.Instance.GetComponent<BoxCollider2D>().isTrigger == true)
                {
                    UIManager.Instance.GetPanel<UITimeOverPanel>().Show(percentCompleted);
                }
                else
                {
                    if (storedLevelParts != null)
                    {
                        storedLevelParts = null;
                    }
                    else
                    {
                        storedLevelParts = instantiatedLevel.GetLevelPartsIDs();
                    }

                    GameData.CurrentScore = 0;

                    SceneManager.LoadScene("Game");
                }
            }
            else
            {
                float structsTotal = instantiatedLevel.GetLastStruct().Id;
                float percentCompleted = structsCompleted / structsTotal;

                if (percentCompleted >= 0.25f && !isReviveUsed)
                {
                    UIManager.Instance.GetPanel<UILosePanel>().Show(percentCompleted);
                }
                else
                {
                    if (storedLevelParts != null)
                    {
                        storedLevelParts = null;
                    }
                    else
                    {
                        storedLevelParts = instantiatedLevel.GetLevelPartsIDs();
                    }

                    GameData.CurrentScore = 0;

                    deathCount++;
                    if (deathCount % 3 == 0)
                    {
                        AdsManager.Instance.ShowInterstitial(GlobalConstants.Death3Times);
                    }
                    else
                    {
                        SceneManager.LoadScene("Game");
                    }
                }
            }
        }
        else if (Mode == GameMode.Endless)
        {
            UIManager.Instance.GetPanel<UILosePanel>().Show();
        }
    }

    private void OnReviveButtonPressed()
    {
        int lastTriggeredStructId = Player.Instance.CollisionsHandler.LastTriggeredStruct.Id;

        LevelStruct nearestClosedStruct = instantiatedLevel.GetNearestClosedLevelStruct(lastTriggeredStructId);

        Vector3 playerPosition = nearestClosedStruct.transform.position + 0.1f * Vector3.up;

        Player.Instance.Movement.Reset();

        Player.Instance.transform.position = playerPosition;
        Player.Instance.State = PlayerState.Alive;

        CameraFollow.Instance.FocusImmediately();

        instantiatedLevel.ResetDisposableLines();

        isReviveUsed = true;
    }

    private int GetCountPartsToUse()
    {
        if (LevelsCompleted % GameConfig.SPECIAL_LEVELS_FREQUENCY == 0)
        {
            return SpecialLevelParts.Count;
        }

        int countAvailableParts = 2;

        countAvailableParts += LevelsCompleted / 5;

        if (countAvailableParts > EndlessLevelParts.Count)
        {
            countAvailableParts = EndlessLevelParts.Count;
        }

        return countAvailableParts;
    }

    private int GetCountPartsToCreate()
    {
        int countAvailableParts = 2;

        countAvailableParts += LevelsCompleted / 5;

        return countAvailableParts;
    }

    public void OnRVRewardReceived(string key)
    {
        if (key.Equals(GlobalConstants.Death3Times))
        {
            SceneManager.LoadScene("Game");
        }
        else if (key.Equals(GlobalConstants.LevelComplete))
        {
            Player.Instance.State = PlayerState.Alive;
            SceneManager.LoadScene("Game");
        }
    }
}