using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
//using Yudiz.VRCricket.Core;

namespace Yudiz.VRArchery.UI
{

    public class PopOnClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private bool rotateRight;
        [SerializeField] private bool isrotating;

        private float rotateDuration = 3;

        private Vector3 hoverScale = new Vector3(1.45f, 1.45f, 1.45f); 
        public float hoverDuration = 0.2f;
        private Vector3 clickScale = new Vector3(0.5f, 0.5f, 0.5f);
        public float clickDuration = 0f; 

        private Vector3 originalScale; 

        private void Start()
        {
            originalScale = transform.localScale;
            RotateOnlyRotatableElements();
        }

        private void ChangeRotation()
        {
            if (rotateRight)
            {
                rotateRight = false;
                RotateOnlyRotatableElements();
            }
            else if (!rotateRight)
            {
                rotateRight = true;
                RotateOnlyRotatableElements();
            }
        }

        private void RotateOnlyRotatableElements()
        {
            if (isrotating)
            {
                RotateButton();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(hoverScale, hoverDuration)
                .SetEase(Ease.OutBack);

            rotateDuration = 1;
            ChangeRotation();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(originalScale, hoverDuration)
                .SetEase(Ease.OutBack);

            rotateDuration = 3;
            ChangeRotation();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //AudioManager.inst.PlayAudio(AudioManager.AudioName.Onclick);

            transform.DOScale(clickScale, clickDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    transform.DOScale(originalScale, clickDuration)
                            .SetEase(Ease.OutBack);
                });
        }

        public void RotateButton()
        {
            if (rotateRight)
            {
                transform.DORotate(new Vector3(0f, 0f, 360f), 360f / rotateDuration, RotateMode.WorldAxisAdd)
                    .SetSpeedBased()
                    .SetEase(Ease.Linear)
                    .SetLoops(-1);
            }
            else
            {
                transform.DORotate(new Vector3(0f, 0f, -360f), 360f / rotateDuration, RotateMode.WorldAxisAdd)
                    .SetSpeedBased()
                    .SetEase(Ease.Linear)
                    .SetLoops(-1);
            }
        }

    }
}