using Assets._Scripts.Tools.Base;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    private const float DELTA_MULTIPLAYER = 0.5f;

    private int multiplayer = 1;

    private float timeSinceLastPointsAdded;

    public static PointsManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        timeSinceLastPointsAdded += Time.deltaTime;

        if (timeSinceLastPointsAdded > DELTA_MULTIPLAYER && multiplayer != 1)
        {
            SoundManager.Instance.ResetPitch();
            multiplayer = 1;
        }
    }

    public string AddPoints()
    {
        int points = 1;

        switch (multiplayer)
        {
            case 2:
                {
                    points = 2;
                    break;
                }
            case 3:
                {
                    points = 4;
                    break;
                }
            case 4:
                {
                    points = 6;
                    break;
                }
            case 5:
                {
                    points = 8;
                    break;
                }
            case 6:
                {
                    points = 10;
                    break;
                }
            case 7:
                {
                    points = 12;
                    break;
                }
            case 8:
                {
                    points = 24;
                    break;
                }
            case 9:
                {
                    points = 36;
                    break;
                }
            case 10:
                {
                    points = 48;
                    break;
                }
        }

        if (multiplayer > 10)
        {
            points = 48;
        }

        GameData.CurrentScore += points;
        SoundManager.Instance.AddPitch();

        timeSinceLastPointsAdded = 0;
        multiplayer++;

        return MakePointsString(points);
    }

    private string MakePointsString(int points)
    {
        string postfix = string.Empty;

        switch (points)
        {
            case 1:
                {
                    postfix = "Easy";
                    break;
                }
            case 2:
                {
                    postfix = "Nice";
                    break;
                }
            case 4:
                {
                    postfix = "Cool";
                    break;
                }
            case 6:
                {
                    postfix = "Great";
                    break;
                }
            case 8:
                {
                    postfix = "Amazing";
                    break;
                }
            case 10:
                {
                    postfix = "Unreal";
                    break;
                }
            case 12:
                {
                    postfix = "Impossible";
                    break;
                }
            case 24:
                {
                    postfix = "Brilliant";
                    break;
                }
            case 36:
                {
                    postfix = "Superstar";
                    break;
                }
            case 48:
                {
                    postfix = "Ninja Jumps";
                    break;
                }
        }

        var finalText = string.Format("+{0} {1}", points, postfix);

        return finalText;
    }
}