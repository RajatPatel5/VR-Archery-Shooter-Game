using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayScreen : BaseScreen
{
    public TMP_Text currentScore;
    public TMP_Text HighScore;
    
    private void OnEnable()
    {
        ScoreManager.OnScoreUpdate += ScoreGamePlayScreeen;
    }

    public void ScoreGamePlayScreeen(int score)
    {
        currentScore.text = score.ToString();
    }


    //private void Awake()
    //{
    //    inst = this;
    //}


}
