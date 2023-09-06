using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : BaseScreen
{
    [SerializeField] Button playNowBtn;

   [SerializeField] private BowController bow;

    private void Start()
    {
        playNowBtn.onClick.AddListener(GamePlayNow);
    }

    void GamePlayNow()
    {
        ScreenManager.instance.ShowNextScreen(ScreenType.GamePlayCanvas);
        bow.SpwanNewArrow();
        //GamePlayScreen.inst.currentScore.text = "Score : " + 0;
        ScoreManager.instance.LoadHighScore(ScreenManager.instance.screens[1].GetComponent<GamePlayScreen>());        
    }

}