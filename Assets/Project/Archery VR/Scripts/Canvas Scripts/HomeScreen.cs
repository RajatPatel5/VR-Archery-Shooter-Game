using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yudiz.VRArchery.Managers;

namespace Yudiz.VRArchery.UI
{
    public class HomeScreen : BaseScreen
    {
        [SerializeField] Button playNowBtn;

        private void Start()
        {
            playNowBtn.onClick.AddListener(GamePlayNow);
        }

        void GamePlayNow()
        {
            ScreenManager.instance.ShowNextScreen(ScreenType.CountDownCanvas);
            GameEvents.countDown?.Invoke();
            GameEvents.onLoadingHighScore?.Invoke();
            //ScoreManager.instance.LoadHighScore(ScreenManager.instance.screens[1].GetComponent<GamePlayScreen>());
        }
    }

}
