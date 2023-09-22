using System.Collections;
using UnityEngine;
using TMPro;
using Yudiz.VRArchery.Managers;

namespace Yudiz.VRArchery.UI
{
    public class CountDownCanvas : BaseScreen
    {

        [SerializeField] private int countDownTime;
        [SerializeField] private TMP_Text countDowntext;

        private void OnEnable()
        {
            GameEvents.countDown += StartTimer;
        }

        private void OnDisable()
        {
            GameEvents.countDown -= StartTimer;
        }

        public void StartTimer()
        {
            StartCoroutine(nameof(CountDownTostart));
        }


        IEnumerator CountDownTostart()
        {
            while (countDownTime > 0)
            {
                countDowntext.text = countDownTime.ToString();
                yield return new WaitForSeconds(1);
                countDownTime--;
            }

            countDowntext.text = "GO !";

            yield return new WaitForSeconds(1);

            Debug.Log("Start The Game in CountDown");
            countDowntext.text = "";
            ScreenManager.instance.ShowNextScreen(ScreenType.GamePlayCanvas);
            countDownTime = 3;
            GameEvents.spwanArrow?.Invoke();
            Debug.Log("CountDown Resetted");
        }
    }
}
