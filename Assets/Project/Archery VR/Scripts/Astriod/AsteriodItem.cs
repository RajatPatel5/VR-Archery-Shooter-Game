using UnityEngine;
using System.Collections.Generic;
using Yudiz.VRArchery.Managers;

namespace Yudiz.VRArchery.CoreGameplay
{
    public class AsteriodItem : MonoBehaviour
    {
        #region PUBLIC_VARS
        [HideInInspector] public bool launched = false;
        #endregion

        #region PRIVATE_VARS

        [SerializeField] bool isRotatable;
        [SerializeField] float rotationAngle;
        private float initialPos;

        #endregion

        #region UNITY_CALLBACKS       
        private void Update()
        {
            LaunchAsteriod();
        }

        private void OnEnable()
        {
            initialPos = transform.localPosition.z;
            //if (launched)
            //{
            //    //asteroidMesh.enabled = true;
            //    //transform.localPosition -= Random.Range(GameController.inst.asteroidData.asteroidSpeed.x, GameController.inst.asteroidData.asteroidSpeed.y) * Time.deltaTime * Vector3.back;
            //    float y = transform.localPosition.y;
            //    float x = transform.localPosition.x;
            //    float z = Mathf.Lerp(initialPos, 1500, Random.Range(GameController.inst.asteroidData.asteroidSpeed.x, GameController.inst.asteroidData.asteroidSpeed.y) * Time.deltaTime);
            //    transform.localPosition = new Vector3(x, y, z);
            //    //launched = false;
            //}
        }
        #endregion


        #region PRIVATE_FUNCTIONS      
        private void LaunchAsteriod()
        {
            //if (launched)
            //{
            //    //asteroidMesh.enabled = true;
            //    transform.localPosition -= Random.Range(GameController.inst.asteroidData.asteroidSpeed.x, GameController.inst.asteroidData.asteroidSpeed.y) * Time.deltaTime * Vector3.back;
            //    //transform.localPosition = Vector3.Lerp(initialPos, new Vector3(0, 0, 1500), Random.Range(GameController.inst.asteroidData.asteroidSpeed.x, GameController.inst.asteroidData.asteroidSpeed.y));
            //    //launched = false;

            if (launched)
            {
                //asteroidMesh.enabled = true;
                transform.localPosition += Random.Range(GameController.inst.asteroidData.asteroidSpeed.x, GameController.inst.asteroidData.asteroidSpeed.y) * Time.deltaTime * Vector3.forward;
                //float y = transform.localPosition.y;
                //float x = transform.localPosition.x;
                //float z = Mathf.Lerp(initialPos, 1500, Random.Range(GameController.inst.asteroidData.asteroidSpeed.x, GameController.inst.asteroidData.asteroidSpeed.y) * Time.deltaTime);
                //transform.localPosition = new Vector3(x, y, z);
                //launched = false;


                if (isRotatable)
                {
                    float xRot = Random.Range(0, 360);
                    float yRot = Random.Range(0, 360);
                    float zRot = Random.Range(0, 360);
                    transform.Rotate(new Vector3(xRot, yRot, zRot) * rotationAngle * Time.deltaTime);
                }
                if (transform.localPosition.z >= 1200)
                {
                    launched = false;
                    Hide();
                }
            }

        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
        #endregion

        #region CO-ROUTINES
        #endregion

        #region EVENT_HANDLERS
        #endregion

        #region UI_CALLBACKS
        #endregion
    }
}