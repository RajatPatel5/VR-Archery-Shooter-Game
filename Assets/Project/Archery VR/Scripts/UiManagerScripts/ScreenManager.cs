using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public BaseScreen[] screens;

    public BaseScreen HomeScreen;

    public static ScreenManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        HomeScreen.canvas.enabled = true;
    }

    public void ShowNextScreen(ScreenType screenType)
    {
        HomeScreen.canvas.enabled = false;

        foreach (BaseScreen baseScreen in screens)
        {
            if (baseScreen.screenType == screenType)
            {
                baseScreen.canvas.enabled = true;
                HomeScreen = baseScreen;
                break;
            }

        }

        //switch (screenType)
        //{
        //    case ScreenType.HomeScreen:
        //        GameStateManager.inst.ChangeGameState(GameStates.HomeScreen);
        //        break;           
        //    case ScreenType.GamePlayCanvas:
        //        GameStateManager.inst.ChangeGameState(GameStates.GamePlay);
        //        break;
        //    case ScreenType.GameOverPage:
        //        GameStateManager.inst.ChangeGameState(GameStates.GameOver);
        //        break;            
        //}
    }
}