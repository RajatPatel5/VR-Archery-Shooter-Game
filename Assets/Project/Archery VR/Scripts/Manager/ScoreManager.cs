using UnityEngine;
using System.IO;
using System;
using Yudiz.VRArchery.UI;

namespace Yudiz.VRArchery.Managers
{

    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;
        static string filePath;
        public int tempHighScore;
        public int tempCurrentScore;
        public int score;
        public StoreScoreData scoreData;
        //public static event Action<int> OnScoreUpdate;

        private void Awake()
        {
            filePath = Application.persistentDataPath + "/VrArchery.fun";
            instance = this;
            LoadData();
        }

        public void AddScore(int points)
        {
            score += points;
            //OnScoreUpdate?.Invoke(score);
            GameEvents.updateScore?.Invoke(score);
        }


        public void SaveData()
        {
            string jsonData = JsonUtility.ToJson(scoreData);
            File.WriteAllText(filePath, jsonData);

        }

        public void LoadData()
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                scoreData = JsonUtility.FromJson<StoreScoreData>(jsonData);
                //BinaryFormatter formatter = new BinaryFormatter();
                //FileStream stream = new FileStream(filePath, FileMode.Open);
                //appData = formatter.Deserialize(stream) as AppData;
                //stream.Close();
            }
            else
            {
                Debug.Log("No file Found");
            }

       }                
       
    }

    [Serializable]
    public class StoreScoreData
    {
        public float HighScore;
    }
}

