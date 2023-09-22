using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Yudiz.VRArchery.Managers;

namespace Yudiz.VRArchery.UI
{
    public class GamePlayScreen : BaseScreen
    {
        public TMP_Text currentScore;
        public TMP_Text HighScore;

        private void OnEnable()
        {
            GameEvents.onLoadingHighScore += LoadHighScore;
            //GameEvents.onGameOver += OnGameOver;
            GameEvents.updateScore += ScoreGamePlayScreeen;
        }

        private void OnDisable()
        {
            GameEvents.onLoadingHighScore -= LoadHighScore;
            //GameEvents.onGameOver -= OnGameOver;
            GameEvents.updateScore -= ScoreGamePlayScreeen;
        }

        public void ScoreGamePlayScreeen(int score)
        {
            currentScore.text = score.ToString();
            ScoreManager.instance.tempCurrentScore = Convert.ToInt32(currentScore.text);
            ScoreManager.instance.tempHighScore = Convert.ToInt32(HighScore.text);
        }

        //public void OnGameOver()
        //{
        //    //ScoreManager.instance.CheckPlayerHighScore(this);
        //    ScoreManager.instance.tempHighScore = Convert.ToInt32(HighScore.text);
        //    ScoreManager.instance.tempCurrentScore = Convert.ToInt32(currentScore.text);

        //    if (ScoreManager.instance.tempCurrentScore > ScoreManager.instance.tempHighScore)
        //    {
        //        HighScore.text = currentScore.text;
        //    }
        //    ScoreManager.instance.scoreData.HighScore = Convert.ToInt32(HighScore.text);
        //    ScoreManager.instance.SaveData();
        //}

        //public void LoadHighScore()
        //{            
        //    ScoreManager.instance.LoadHighScore(this);
        //}

        public void LoadHighScore()
        {
            int resetCurrentScore = 0;
            HighScore.text = ScoreManager.instance.scoreData.HighScore.ToString();
            currentScore.text = resetCurrentScore.ToString();
            ScoreManager.instance.tempCurrentScore = resetCurrentScore;
            ScoreManager.instance.score = resetCurrentScore;
        }
    }
}
