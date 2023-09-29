using UnityEngine;
using System.Collections.Generic;
using Yudiz.VRArchery.Managers;

namespace Yudiz.VRArchery.CoreGameplay
{
    public class AsteriodItem : MonoBehaviour
    {
        #region PUBLIC_VARS
        [HideInInspector] public bool launched = false;
        //public bool isBlasted;
        #endregion

        #region PRIVATE_VARS
        //[SerializeField] Health health;
        //[SerializeField] float pointsForDestroying;
        [SerializeField] bool isRotatable;
        [SerializeField] float rotationAngle;

        //[SerializeField] float asteroidDamage;
        //[SerializeField] GameObject asteroidsChunk;
        //[SerializeField] MeshRenderer asteroidMesh;

        //[Header("Asteroid Damage")]
        //[SerializeField] List<ParticleSystem> particlePausingList;
        //[SerializeField] ParticleSystem explosionParticle;
        #endregion

        #region UNITY_CALLBACKS       
        private void Update()
        {
            LaunchAsteriod();            
        }

        //private void OnTriggerEnter(Collider other)        
        //{            
        //    var collideObj = other.gameObject.GetComponent<SpaceShip>();
        //    if(collideObj != null && !isBlasted)
        //    {                
        //        collideObj.GetDamage(asteroidDamage);
        //    }
        //}

        //private void OnEnable()
        //{
        //    //asteroidMesh.enabled = true;
        //    isBlasted = false;
        //}
        #endregion

        #region STATIC_FUNCTIONS
        #endregion

        #region PUBLIC_FUNCTIONS
        //public void GetDamage(float amount)
        //{
        //    health.ReduceCurrentHp(amount);
        //    if(health.health <= 0 && !isBlasted)
        //    { 
        //        asteroidMesh.enabled = false;
        //        isBlasted = true;
        //        PauseAllParticle();
        //        explosionParticle.Play();
        //        Debug.Log("Object Dead");
        //        ScoreManager.Instance.AddScore(pointsForDestroying);                
        //        //Hide();                
        //        Debug.Log("paricle Called");
        //    }
        //}

        //private async void DestoryAsteroidChunk(GameObject gameObject)
        //{
        //    await System.Threading.Tasks.Task.Delay(5000);
        //    Destroy(gameObject);
        //}
        //#endregion

        //#region PRIVATE_FUNCTIONS
        //private void PauseAllParticle()
        //{
        //    foreach (var item in particlePausingList)
        //    {
        //        item.Stop();
        //    }
        //}

        private void LaunchAsteriod()
        {
            if (launched)
            {
                //asteroidMesh.enabled = true;
                transform.localPosition -= (Vector3.back) * Random.Range(GameController.inst.asteroidData.asteroidSpeed.x, GameController.inst.asteroidData.asteroidSpeed.y) * Time.deltaTime;
                if (isRotatable)
                {
                    float xRot = Random.Range(0, 360);
                    float yRot = Random.Range(0, 360);
                    float zRot = Random.Range(0, 360);
                    transform.Rotate(new Vector3(xRot, yRot, zRot) * rotationAngle * Time.deltaTime);
                }
                if (transform.localPosition.z >= 1000)
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