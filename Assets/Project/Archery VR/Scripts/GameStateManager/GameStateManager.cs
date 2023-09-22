using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager inst;
    public static Action<GameStates> onStateChange;

    public GameStates CurrentState;

    private void Awake()
    {
        inst = this;
    }

    public void ChangeGameState(GameStates gs)
    {
        CurrentState = gs;
        onStateChange?.Invoke(gs);
    }
}
    public enum GameStates
    {
        HomeScreen,            
        GamePlay,
        GameOver,
        countDown,
    }








