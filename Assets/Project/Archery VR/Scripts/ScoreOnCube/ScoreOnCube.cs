using System.Collections;
using TMPro;
using UnityEngine;
using Yudiz.VRArchery.Managers;

namespace Yudiz.VRArchery.CoreGameplay
{

    public class ScoreOnCube : MonoBehaviour
    {
        [SerializeField] int itsScore;
        [SerializeField] private  GameObject pointObj;
        [SerializeField] private Transform pointPos;

        public void UpdateScore()
        {
            ScoreManager.instance.AddScore(itsScore);
            ShowGainPoint(itsScore);
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
