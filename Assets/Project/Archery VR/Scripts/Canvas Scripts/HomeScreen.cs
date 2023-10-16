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
            AudioManager.inst.PlayAudio(AudioManager.AudioName.Onclick);
            ScreenManager.instance.ShowNextScreen(ScreenType.GamePlayCanvas);            
            GameEvents.spwanArrow?.Invoke();
            GameEvents.onLoadingHighScore?.Invoke();
            //ScoreManager.instance.LoadHighScore(ScreenManager.instance.screens[1].GetComponent<GamePlayScreen>());
        }
    }

}
