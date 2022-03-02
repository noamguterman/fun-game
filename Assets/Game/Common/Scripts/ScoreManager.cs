using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int Cash, Multiplier, Best;

    public GameObject[] rewardParticles;

    public GameObject lightingParticle;
    void Awake()
    {
        Instance = this;
        Load();

        Multiplier = 1;

        Cash = 0;
        Save();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData data = new PlayerData();

        if (Cash > Best)
        {
            Best = Cash;
        }

        //Stats
        data.Best = Best;

        bf.Serialize(file, data);
        file.Close();
        Invoke("SetScore", 0.1f);
    }

    void SetScore()
    {
        ChallengeController.Instance.SetScores();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            //Stats
            Best = data.Best;
        }
    }

    [Serializable]
    class PlayerData
    {
        public int Best, Multiplier;
    }
}
