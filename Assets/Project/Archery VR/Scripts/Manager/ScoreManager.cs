using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    static string filePath;
    int tempHighScore;
    int tempCurrentScore;
    public static int score;
    public StoreScoreData scoreData;

    public static event Action<int> OnScoreUpdate;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/VrArchery.fun";
        instance = this;
        LoadData();
    }

    public void AddScore(int points)
    {
        score += points;
        OnScoreUpdate?.Invoke(score);
        //CrystalMainMenu.text = Score.ToString();
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

    public void CheckPlayerHighScore(GamePlayScreen gamePlayScreen)
    {
        tempHighScore = Convert.ToInt32(gamePlayScreen.HighScore.text);
        tempCurrentScore = Convert.ToInt32(gamePlayScreen.currentScore.text);

        if (tempCurrentScore > tempHighScore)
        {
            gamePlayScreen.HighScore.text = gamePlayScreen.currentScore.text;
        }
        scoreData.HighScore = Convert.ToInt32(gamePlayScreen.HighScore.text);
        SaveData();
    }

    public void LoadHighScore(GamePlayScreen gamePlayScreen)
    {
        int resetCurrentScore = 0;
        gamePlayScreen.HighScore.text = scoreData.HighScore.ToString();
        gamePlayScreen.currentScore.text = resetCurrentScore.ToString();
        score = resetCurrentScore;
    }

    public void ConnectGamePlayAndGameOverScore(GameOverCanvas gameOver)
    {
       gameOver.yourScore.text = tempCurrentScore.ToString();
       gameOver.HighScoreTillNow.text = tempHighScore.ToString();
    }
}

[Serializable]
public class StoreScoreData
{
    public float HighScore;
}

