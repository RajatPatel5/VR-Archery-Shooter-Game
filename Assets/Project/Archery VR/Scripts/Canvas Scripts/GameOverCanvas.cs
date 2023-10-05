using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Yudiz.VRArchery.Managers;
using System;

namespace Yudiz.VRArchery.UI
{
    public class GameOverCanvas : BaseScreen
    {
        public Button MainMenuButton;
        public Button RestartGameButton;
        public TMP_Text yourScore;
        public TMP_Text HighScoreTillNow;

        private void Start()
        {
            MainMenuButton.onClick.AddListener(BackHomeScreen);
            RestartGameButton.onClick.AddListener(RestartGame);
        }

        private void OnEnable()
        {
            GameEvents.onGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            GameEvents.onGameOver -= OnGameOver;
        }

        void BackHomeScreen()
        {
            AudioManager.inst.PlayAudio(AudioManager.AudioName.Onclick);
            ScreenManager.instance.ShowNextScreen(ScreenType.HomeScreen);
        }

        public void RestartGame()
        {
            //ScreenManager.instance.ShowNextScreen(ScreenType.CountDownCanvas);
            //GameEvents.countDown?.Invoke();
            AudioManager.inst.PlayAudio(AudioManager.AudioName.Onclick);
            ScreenManager.instance.ShowNextScreen(ScreenType.GamePlayCanvas);
            GameEvents.spwanArrow?.Invoke();
            GameEvents.onLoadingHighScore?.Invoke();
            //ScoreManager.instance.LoadHighScore(ScreenManager.instance.screens[1].GetComponent<GamePlayScreen>());
        }

        public void OnGameOver()
        {
            //AudioManager.inst.PlayAudio(AudioManager.AudioName.Onclick);
            //ScoreManager.instance.ConnectGamePlayAndGameOverScore(this);
            yourScore.text = ScoreManager.instance.tempCurrentScore.ToString();
            HighScoreTillNow.text = ScoreManager.instance.tempHighScore.ToString();


            //ScoreManager.instance.CheckPlayerHighScore(this);


            if (ScoreManager.instance.tempCurrentScore > ScoreManager.instance.tempHighScore)
            {
                ScoreManager.instance.scoreData.HighScore = Convert.ToInt32(yourScore.text);
                HighScoreTillNow.text = yourScore.text;
            }
            else
            {
                ScoreManager.instance.scoreData.HighScore = Convert.ToInt32(HighScoreTillNow.text);
            }
            ScoreManager.instance.SaveData();
        }
    }
}


