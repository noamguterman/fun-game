using System;
using UnityEngine;

public class DateManager : MonoBehaviour
{
    private const int HOURS_PER_DAY = 24;
    private const int MINUTES_PER_HOUR = 60;
    private const int MINUTES_PER_DAY = 1440;

    private int PlayedConsecutiveDays
    {
        get
        {
            return PlayerPrefs.GetInt("PlayedConsecutiveDays");
        }
        set
        {
            PlayerPrefs.SetInt("PlayedConsecutiveDays", value);
        }
    }

    private DateTime currentDate;
    private DateTime oldDate;

    private static TimeSpan timeSinceLastLaunch;

    private static float countedTime;
    private static bool isCountTicks;

    private static bool isInitialized;

    private UIShopPanel shopPanel;

    public static DateManager Instance;

    private void Awake()
    {
        if (!isInitialized)
        {
            Instance = this;

            currentDate = DateTime.Now;
            oldDate = DateTime.Now;

            timeSinceLastLaunch = new TimeSpan(0);

            if (PlayerPrefs.HasKey("sysString"))
            {
                long temp = Convert.ToInt64(PlayerPrefs.GetString("sysString"));

                oldDate = DateTime.FromBinary(temp);
                timeSinceLastLaunch = currentDate.Subtract(oldDate);

                print("Saving this date to prefs: " + oldDate);

                AnalyticsCall();
            }
            else
            {
                PlayedConsecutiveDays = 1;

                //string message = string.Format("played_consecutive_days_{0}", PlayedConsecutiveDays);

                //ServiceProvider.Analytics.SendEvent(message);
            }

            isInitialized = true;
        }
    }

    private void AnalyticsCall()
    {
        if (currentDate.Day != oldDate.Day)
        {
            int oldDateLeftMinutes = (HOURS_PER_DAY - oldDate.Hour) * 60 - oldDate.Minute;

            if (timeSinceLastLaunch.TotalMinutes < MINUTES_PER_DAY + oldDateLeftMinutes)
            {
                PlayedConsecutiveDays++;

                string message = string.Format("played_consecutive_days_{0}", PlayedConsecutiveDays);

                //ServiceProvider.Analytics.SendEvent(message);
            }
            else
            {
                PlayedConsecutiveDays = 1;

                //ServiceProvider.Analytics.SendEvent("played_consecutive_days_restart");
            }
        }
    }

    private void Update()
    {
        //if (isCountTicks)
        //{
        //    countedTime += Time.deltaTime;

        //    if (countedTime >= GameConfig.RANDOM_SKINS_TIME)
        //    {
        //        shopPanel = UIManager.Instance.GetPanel<UIShopPanel>();

        //        shopPanel.StopRandom();

        //        StopCountTicks();
        //    }
        //}
    }

    public void StartCountTicks(float startCountedTime)
    {
        if (!isCountTicks)
        {
            isCountTicks = true;
            countedTime = startCountedTime;
        }
    }

    public void StopCountTicks()
    {
        isCountTicks = false;
        countedTime = 0;
    }

    public float GetTimeSinceLastLaunch()
    {
        float time = (float)timeSinceLastLaunch.TotalSeconds;

        timeSinceLastLaunch = new TimeSpan(0);

        return time;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerPrefs.SetString("sysString", DateTime.Now.ToBinary().ToString());

            //print("Saving this date to prefs: " + DateTime.Now);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("sysString", DateTime.Now.ToBinary().ToString());

        // print("Saving this date to prefs: " + DateTime.Now);
    }
}