using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yudiz.VRArchery.CoreGameplay;
namespace Yudiz.VRArchery.Managers
{
    public static class GameEvents
    {
        public delegate void OnCountDown();
        public static OnCountDown countDown;

        public delegate void OnGamePlay();
        public static OnGamePlay spwanArrow;

        public delegate void OnGameOver();
        public static OnGameOver onGameOver;

        public delegate void LoadHighScore();
        public static LoadHighScore onLoadingHighScore;

        public delegate void OnScoreUpdate(int value);
        public static OnScoreUpdate updateScore;

        public delegate void OnArrowThrowen();
        public static OnArrowThrowen onArrowThrowen;

        public delegate void OnCheckDistance(Arrow arrow);
        public static OnCheckDistance onCheckDistance;        

    }
}
