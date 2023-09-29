using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yudiz.VRArchery.CoreGameplay;
using Yudiz.VRArchery.UI;

namespace Yudiz.VRArchery.Managers
{

    public class GameController : MonoBehaviour
    {
        #region PUBLIC_VARS
        public static GameController inst;
        public bool canThrowArrow;
        public Arrow tempArrow;
        public Arrow currentArrow;
        public AsteroidData asteroidData;
        #endregion

        #region PRIVATE_VARS
        [Header("Bow Area")]
        [SerializeField] private Bow bow;

        [Header("Spwan Arrows List And its Pos")]
        public List<Arrow> allArrows;
        [SerializeField] private Arrow arrowPrefab;
        [SerializeField] private List<Transform> spwanArrowPoint;
        [SerializeField] private Transform distance;


        [Header("Arrow Force Area")]
        public float forcePower;
        [SerializeField] private float minValue;
        [SerializeField] private float maxValue;
        [SerializeField] private float enterRange;
        [SerializeField] private float exitRange;
        [SerializeField] private bool isSnap;
        private bool isGameOn = false;
        #endregion

        #region UNITY_CALLBACKS
        private void Awake()
        {
            inst = this;
        }

        private void OnEnable()
        {
            GameEvents.onArrowThrowen += AddForceToArrow;
            GameEvents.onCheckDistance += CheckValidThrowOrNot;
            //Arrow.OnThisArrowAddForce += AddForceToArrow;
            //Arrow.OnCollisionShouldResetOrNot += CheckValidThrowOrNot;
            GameEvents.spwanArrow += SpwanNewArrow;
        }

        private void OnDisable()
        {
            GameEvents.onArrowThrowen -= AddForceToArrow;
            GameEvents.onCheckDistance -= CheckValidThrowOrNot;
            //Arrow.OnThisArrowAddForce -= AddForceToArrow;
            //Arrow.OnCollisionShouldResetOrNot -= CheckValidThrowOrNot;
            GameEvents.spwanArrow -= SpwanNewArrow;
        }

        private void Start()
        {            
            isGameOn = true;
            //SpawnRandomAsteroid();
            StartCoroutine(GenerateAsteroids());
        }
        private void Update()
        {
            if (currentArrow == null) return;
            if (bow.isGrabingBow)
            {
                //bow.pointBetweenStartAndEnd.position = NearestPointOnFiniteLine(bow.arrowStartPoint.position, bow.arrowEndPoint.position, currentArrow.transform.position);
                //bow.UpdatePullingString(bow.pointBetweenStartAndEnd.localPosition);

                float distance = Vector3.Distance(currentArrow.transform.position, bow.pointBetweenStartAndEnd.transform.position);
                if (distance < enterRange)
                {
                    isSnap = true;
                    SnapIn();
                }
                else if (distance > exitRange)
                {
                    isSnap = false;
                    SnapOut();
                    //UnAssignArrow();
                }
                else if (distance < exitRange)
                {
                    if (isSnap)
                    {
                        SnapIn();
                    }
                    else
                    {
                        SnapOut();
                    }
                }
            }
            else
            {
                SnapOut();
            }
        }
        #endregion

        #region PUBLIC_FUNCTIONS
        public void GameOverPage()
        {
            ScreenManager.instance.ShowNextScreen(ScreenType.GameOverPage);
            GameEvents.onGameOver?.Invoke();
        }



        public void SpwanNewArrow()
        {
            for (int i = 0; i < spwanArrowPoint.Count; i++)
            {
                Arrow arrow = Instantiate(arrowPrefab, spwanArrowPoint[i].position, spwanArrowPoint[i].rotation);
                allArrows.Add(arrow);
            }
        }


        public void CheckValidThrowOrNot(Arrow arrow)
        {
            if (tempArrow == arrow)
            {
                float dis = Vector3.Distance(distance.position, arrow.transform.position);
                Debug.Log("bunss" + dis);
                if (dis < 1)
                {
                    arrow.ResetArrowPos();
                    tempArrow = null;
                }
                else
                {
                    Debug.Log("is Remove");
                    allArrows.Remove(arrow);
                    Destroy(arrow, 5);
                    CheckGameOver();
                    tempArrow = null;
                }
            }
        }

        public void CheckGameOver()
        {
            if (allArrows.Count == 0)
            {
                Invoke(nameof(GameOverPage), 1f);
            }
        }

        public void AddForceToArrow()
        {
            forcePower = PullValue();
            //tempArrow = currentArrow;
            //forcePower = PullValue();
            bow.BowThrower(forcePower, currentArrow);
            //UnAssignArrow();
            ResetBowString();
        }

        [ContextMenu("Throw Arrow")]
        public void TestThrowArrow()
        {
            tempArrow = currentArrow;
            forcePower = PullValue();
            //BowThrower(forcePower);
            bow.BowThrower(forcePower, currentArrow);
            //UnAssignArrow();
        }

        public float PullValue()
        {
            float pullDirection = Vector3.Distance(bow.arrowStartPoint.position, bow.arrowEndPoint.position);
            float targetDirection = Vector3.Distance(bow.arrowStartPoint.position, bow.pointBetweenStartAndEnd.position);
            float pullValue = targetDirection / pullDirection;
            float t = Mathf.Clamp(pullValue, 0, 1);
            float streach = minValue + t * (maxValue - minValue);
            //Debug.Log("streach : " + streach);
            return streach;
        }

        public Vector3 NearestPointOnFiniteLine(Vector3 start, Vector3 end, Vector3 pnt)
        {
            var line = (end - start);
            var len = line.magnitude;
            line.Normalize();

            var v = pnt - start;
            var d = Vector3.Dot(v, line);
            d = Mathf.Clamp(d, 0f, len);
            return start + line * d;
        }

        public Vector3 NearestPointOnLine(Vector3 linePoint, Vector3 lineDirection, Vector3 point)
        {
            lineDirection.Normalize();//this needs to be a unit vector
                                      //var v = point - lineDirection;
                                      //var d = Vector3.Dot(v, lineDirection);
            return linePoint + Vector3.Project(point - linePoint, lineDirection);
            //return linePoint + lineDirection * d;
        }

        private void SpawnRandomAsteroid()
        {
            float xPos1 = Random.Range(asteroidData.asteroidXPos.x, asteroidData.asteroidXPos.y);
            float xpos2 = Random.Range(asteroidData.asteroidXPos2.x, asteroidData.asteroidXPos2.y);
            float yPos = Random.Range(asteroidData.asteroidYPos.x, asteroidData.asteroidYPos.y);
            float zPos = Random.Range(asteroidData.asteroidZPos.x, asteroidData.asteroidZPos.y);

            List<float> tempList = new();
            tempList.Add(xpos2);
            tempList.Add(xPos1);

            int xPos = Random.Range(0, tempList.Count);
            AsteriodItem asteriod = ObstaclePoolManager.Instance.RequestAsteriods().GetComponent<AsteriodItem>();
            asteriod.gameObject.transform.localPosition = new Vector3(tempList[xPos], yPos, zPos);
            asteriod.launched = true;

            //tempList = null;
            //tempList = null;
            //if (Time.time >= levelChangeTime)
            //{
            //    Debug.Log("LevelChanging");d
            //    ChangeAsteroidData();
            //}
        }                
        #endregion

        #region PRIVATE_FUNCTIONS
        void SnapIn()
        {
            ArrowRotateTowardsBow();

            bow.pointBetweenStartAndEnd.position = NearestPointOnFiniteLine(bow.arrowStartPoint.position, bow.arrowEndPoint.position, currentArrow.transform.position);
            bow.UpdatePullingString(bow.pointBetweenStartAndEnd.localPosition);

            //var dir = bow.arrowStartPoint.position - bow.arrowEndPoint.position;
            //currentArrow.ModelArrow.transform.position = NearestPointOnLine(bow.pointBetweenStartAndEnd.position, dir, currentArrow.ModelArrow.transform.position);
            currentArrow.ModelArrow.transform.position = bow.pointBetweenStartAndEnd.position;

            currentArrow.xrGrabInteractable.trackRotation = false;
            bow.trajectoryLine.enabled = true;

            float force = PullValue();
            //bow.CalculateTrajectory(force, bow.trajectoryLine, currentArrow.GetComponent<Rigidbody>());
            //bow.CalculateHalfTrajectory(force, bow.trajectoryLine, currentArrow.GetComponent<Rigidbody>());
            bow.DrawProjection(force, bow.trajectoryLine, currentArrow.GetComponent<Rigidbody>());

            tempArrow = currentArrow;
            canThrowArrow = true;
        }

        void SnapOut()
        {
            bow.trajectoryLine.enabled = false;
            currentArrow.xrGrabInteractable.trackRotation = true;
            currentArrow.transform.position = currentArrow.ModelArrow.transform.position;
            currentArrow.ModelArrow.localPosition = Vector3.zero;
            ResetBowString();
            canThrowArrow = false;
        }

        void ArrowRotateTowardsBow()
        {
            Quaternion desiredRotation = Quaternion.LookRotation(bow.transform.forward, Vector3.up);
            float lerpSpeed = 0.8f;
            currentArrow.transform.rotation = Quaternion.Lerp(currentArrow.transform.rotation, desiredRotation, lerpSpeed);
        }

        private void ResetBowString()
        {
            bow.ResetStringPos();
        }
        #endregion

        #region CO-ROUTINES
        public IEnumerator GenerateAsteroids()
        {
            //while(true)
            //{
            //SpawnRandomAsteroid();
            //yield return new WaitForSeconds(SetRandomAsteroidSpawnTime());
            //Debug.Log("SPwaning");
            //}
            //yield return null;
            while (isGameOn)
            {
                SpawnRandomAsteroid();
                
                yield return new WaitForSeconds(SetRandomAsteroidSpawnTime());
            }
            //yield return new WaitForSeconds(asteroidData.timeForSpawningAsteroids);                

            //StartCoroutine(GenerateAsteroids());
        }

        private float SetRandomAsteroidSpawnTime()
        {
            float spawnTime = Random.Range(asteroidData.asteroidSpawnTime.x, asteroidData.asteroidSpawnTime.y);
            return spawnTime;
        }
        #endregion

        #region EVENT_HANDLERS
        #endregion

        #region UI_CALLBACKS
        #endregion
    }

    [System.Serializable]
    public class AsteroidData
    {
        public Vector2 asteroidXPos;
        public Vector2 asteroidXPos2;
        public Vector2 asteroidYPos;
        public Vector2 asteroidZPos;
        public Vector2 asteroidSpeed;
        public Vector2 asteroidSpawnTime;
    }
}

#region COMMENTS-REGION
//public void AssignArrow(Arrow arrow)
//{
//    currentArrow = arrow;
//}

//public void UnAssignArrow()
//{
//    currentArrow = null;
//    Invoke(nameof(ResetBowString), 0.3f);

//}

//private float Remap(float value, int fromMin, float fromMax, int toMin, int toMax)
//{
//    return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
//}
//if (audioSource.isPlaying == false && strength <= 0.01f)
//{
//    audioSource.Play();
//}

//private void PlayStringPullinSound()
//{
//    //Check if we have moved the string enought to play the sound unpause it
//    if (Mathf.Abs(strength - previousStrength) > stringSoundThreshold)
//    {
//        if (strength < previousStrength)
//        {
//            //Play string sound in reverse if we are pusing the string towards the bow
//            audioSource.pitch = -1;
//        }
//        else
//        {
//            //Play the sound normally
//            audioSource.pitch = 1;
//        }
//        audioSource.UnPause();
//    }
//    else
//    {
//        //if we stop moving Pause the sounds
//        audioSource.Pause();
//    }

//}








//public void AddForceToArrow(Arrow arrow)
//{
//    if (currentArrow == arrow)
//    {
//        forcePower = PullValue();
//        //tempArrow = currentArrow;
//        //forcePower = PullValue();
//        bow.BowThrower(forcePower, currentArrow);
//        //UnAssignArrow();
//        ResetBowString();
//    }
//}
//public void UpdatePullingString(Vector3 updatedString)
//{
//    Vector3 linePosition = updatedString;
//    bowString.SetPosition(1, linePosition);
//}

//public void BowThrower(float forcePower)
//{
//    tempArrow = currentArrow;
//    currentArrow.transform.position = currentArrow.ModelArrow.transform.position;
//    currentArrow.ModelArrow.localPosition = Vector3.zero;
//    Vector3 force = currentArrow.transform.forward * forcePower;
//    currentArrow.Thrower(force);
//    UnAssignArrow();        
//}

//Rigidbody arrowRb = temp.GetComponent<Rigidbody>();
//arrowRb.isKinematic = false;
////arrowRb.AddForce(new Vector3(0f, 0f, 1500f));
//arrowRb.velocity = Camera.main.transform.forward * Mathf.Abs(endPointPos) * 100f;
//temp = null;    



//float updateString = UpdateString(t);
//pullAmount = NearestPointOnFiniteLine(arrowStartPoint.position, arrowEndPoint.position, pointBetweenStartAndEnd.position);
//UpdatePullingString(pullAmount);
//float t = 0.0001f * Time.deltaTime;
//float endArrowPos = ArrowToEndPos(t);
//Vector3 tempPos = new Vector3(0, 0, endArrowPos);
//float d = Vector3.Distance(currentArrow.transform.position, arrowStartPoint.position);
//forcePower = Remap(d, 10f, 50f, 0f, -0.2f);

//IEnumerator ChangeArrowPosOnTap(float strachStringEndPoint, float arrowPosEndPoint)
//{
//    float time = 0;
//    while (time < duration)
//    {
//        //float strachString = Mathf.Lerp(-0.2f, strachStringEndPoint, time / duration);
//        float changeArrowPos = Mathf.Lerp(0f, arrowPosEndPoint, time / duration);
//        //bowString.SetPosition(1, new Vector3(0f, 0f, strachString));
//        Vector3 tempPos = new Vector3(0, 0, changeArrowPos);
//        currentArrow.transform.localPosition = tempPos;
//        PullValue(tempPos);
//        time += Time.deltaTime;
//        yield return null;
//    }
//}

//private float UpdateString(float t)
//{
//    t = Mathf.Clamp01(t);
//    float minValue = -0.5f;
//    float maxValue = -0.2f;
//    pullAmount = minValue + t * (maxValue - minValue);
//    Debug.Log("PullAmount" + pullAmount);
//    return pullAmount;
//}


//private float ArrowToEndPos(float t)
//{
//    //t = Mathf.Clamp01(t);

//}
//public float Remap(float unscaledNum, float minAllowed, float maxAllowed, float min, float max)
//{
//    return (maxAllowed - minAllowed) * (unscaledNum - min) / (max - min) + minAllowed;
//}


//if (canThrow)
//{
//    textPower.text = "Power:" + forcePower.ToString();

//}
//else
//{
//    textPower.text = "Power: 0";
//}

//if (canThrow && forcePower < maxForcePower)
//{
//    forcePower += Time.deltaTime * forcePowerSpeed;
//}

//if (canThrow && Input.GetKeyDown(KeyCode.Space))
//{
//    //StopCoroutine(ChangeArrowPosOnTap(pullAmount, endPointPos));
//    canThrow = false;
//    //StopCoroutine(inital);
//    BowThrower(forcePower);
//    ResetPos();
//    forcePower = 0f;
//}
#endregion
