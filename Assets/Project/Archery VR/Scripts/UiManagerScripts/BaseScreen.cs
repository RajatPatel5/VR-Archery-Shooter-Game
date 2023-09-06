using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScreen : MonoBehaviour
{
    [HideInInspector]
    public Canvas canvas;

    public ScreenType screenType;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }
}

public enum ScreenType
{
    HomeScreen,
    GameOverPage,
    GamePlayCanvas,

}