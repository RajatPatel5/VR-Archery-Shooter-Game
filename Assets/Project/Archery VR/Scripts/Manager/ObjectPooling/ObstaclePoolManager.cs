using System.Collections.Generic;
using UnityEngine;

namespace Yudiz.VRArchery.Managers
{
    public class ObstaclePoolManager : Singleton<ObstaclePoolManager>
    {
        #region PUBLIC_VARS        
        public List<GameObject> asteroidPool;
        public List<GameObject> asteroidPrefabList;
        #endregion

        #region PRIVATE_VARS
        [SerializeField] GameObject asteroidContainer;
        [SerializeField] int totalAsteriods;

        #endregion

        #region UNITY_CALLBACKS
        public override void Awake()
        {
            base.Awake();
        }
        
        private void Start()
        {
            asteroidPool = GenerateAsteroids(totalAsteriods);            
        }
        #endregion

        #region STATIC_FUNCTIONS
        #endregion

        #region PUBLIC_FUNCTIONS
        public GameObject RequestAsteriods()
        {
            foreach (var item in asteroidPool)
            {
                if(item.activeInHierarchy == false)
                {
                    item.SetActive(true);
                    return item;
                }
            }
            int index = Random.Range(0, asteroidPrefabList.Count);
            GameObject newAsteriod = Instantiate(asteroidPrefabList[index]);
            newAsteriod.transform.parent = asteroidContainer.transform;            
            asteroidPool.Add(newAsteriod);
            return newAsteriod;
        }
        #endregion

        #region PRIVATE_FUNCTIONS
        private List<GameObject> GenerateAsteroids(int numOfAsteriods)
        {            
            for (int i = 0; i < numOfAsteriods; i++)
            {
                int index = Random.Range(0, asteroidPrefabList.Count);
                GameObject asteroidObj = Instantiate(asteroidPrefabList[index]);
                asteroidObj.transform.parent = asteroidContainer.transform;
                asteroidObj.SetActive(false);
                asteroidPool.Add(asteroidObj);
            }
            return asteroidPool;
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