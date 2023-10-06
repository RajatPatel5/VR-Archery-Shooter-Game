using System.Collections;
using TMPro;
using UnityEngine;
using Yudiz.VRArchery.Managers;

namespace Yudiz.VRArchery.CoreGameplay
{

    public class ScoreOnCube : MonoBehaviour
    {
        [SerializeField] int itsScore;
        [SerializeField] private GameObject pointObj;
        [SerializeField] private Transform pointPos;

        public void UpdateScore()
        {
            ScoreManager.instance.AddScore(itsScore);
            ShowGainPoint(itsScore);
            //switch (itsScore)
            //{
            //case 5:
            //    AudioManager.inst.PlayAudio(AudioManager.AudioName.GoodOne);
            //    break;
            //case 10:                    
            //    AudioManager.inst.PlayAudio(AudioManager.AudioName.GoodOne);
            //    break;
            //case 20:                    
            //    AudioManager.inst.PlayAudio(AudioManager.AudioName.GoodOne);
            //    break;
            //case 30:
            //    AudioManager.inst.PlayAudio(AudioManager.AudioName.GoodOne);
            //    break;
            //case 40:
            //    AudioManager.inst.PlayAudio(AudioManager.AudioName.GoodShort);
            //    break;
            //case 50:
            //    AudioManager.inst.PlayAudio(AudioManager.AudioName.GoodShort);
            //    break;
            //case 100:
            //    AudioManager.inst.PlayAudio(AudioManager.AudioName.Excellent);
            //    break;
            switch (itsScore)
            {
                case 5:
                case 10:
                case 20:
                case 30:
                    AudioManager.inst.PlayAudio(AudioManager.AudioName.GoodOne);
                    break;
                case 40:
                case 50:
                    AudioManager.inst.PlayAudio(AudioManager.AudioName.GoodShort);
                    break;
                case 100:
                    AudioManager.inst.PlayAudio(AudioManager.AudioName.Excellent);
                    break;
                default:
                    // Handle cases that don't match any of the specified values.
                    break;
            }        
        }

        public void ShowGainPoint(int value)
        {
            GameObject point = Instantiate(pointObj, pointPos.position, pointPos.rotation, pointPos);
            TextMeshPro point_text = point.GetComponent<TextMeshPro>();
            point_text.text = "+" + value.ToString();
            point_text.fontSize = 5;
            point_text.alignment = TextAlignmentOptions.Center;
            StartCoroutine(ShowGainPointAnimation(point_text, 1f));
        }

        private IEnumerator ShowGainPointAnimation(TextMeshPro text, float duration)
        {
            float time = Time.deltaTime;
            while (time < duration)
            {
                text.transform.Translate(Vector3.up * Time.deltaTime);
                text.alpha = Mathf.Lerp(text.alpha, 0, time / duration);
                yield return null;
            }
            text.alpha = 0;
            yield return new WaitForSeconds(0.2f);
            Destroy(text.gameObject);
        }
    }
}
