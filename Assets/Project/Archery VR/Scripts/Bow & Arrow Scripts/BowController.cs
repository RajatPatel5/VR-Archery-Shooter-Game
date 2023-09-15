using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class BowController : MonoBehaviour
{
    [Header("Bow Area")]
    [SerializeField] private XRGrabInteractable xrGrabInteractable;
    [SerializeField] private Rigidbody bowRigidbody;
    [SerializeField] private LineRenderer bowString;
    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private Transform arrowStartPoint;
    [SerializeField] private Transform arrowEndPoint;
    [SerializeField] private Transform pointBetweenStartAndEnd;

    [Header("Spwan Arrows List And its Pos")]
    [SerializeField] private List<Transform> spwanArrowPoint;
    [SerializeField] private List<Arrow> allArrows;
    [SerializeField] private Transform distance;
    public Arrow tempArrow;
    public Arrow currentArrow;

    [Header("Arrow Force Area")]
    [SerializeField] private float forcePower;
    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;

    [Header("Ui Screen")]
    [SerializeField] private GamePlayScreen gamePlayScreen;
    [SerializeField] private GameOverCanvas gameOverScreen;

    private Vector3 initialArrowPos;
    private Vector3 initialPos;
    private Quaternion initialRotation;

    private void OnEnable()
    {
        Arrow.OnThisArrowAddForce += AddForceToArrow;
        Arrow.OnCollisionShouldResetOrNot += CheckValidThrowOrNot;
    }

    private void OnDisable()
    {
        Arrow.OnThisArrowAddForce -= AddForceToArrow;
        Arrow.OnCollisionShouldResetOrNot -= CheckValidThrowOrNot;
    }

    private void Start()
    {
        initialPos = transform.position;
        initialRotation = transform.rotation;
        initialArrowPos = pointBetweenStartAndEnd.localPosition;
        xrGrabInteractable.selectEntered.AddListener(OnGrabingBow);
        xrGrabInteractable.selectExited.AddListener(OnLeavingBOwCall);
    }

    private void OnLeavingBOwCall(SelectExitEventArgs arg0)
    {
        bowRigidbody.isKinematic = false;
        Invoke(nameof(ResetBowPos), 0.5f);
    }

    private void OnGrabingBow(SelectEnterEventArgs arg0)
    {
        bowRigidbody.isKinematic = true;
    }

    public void GameOverPage()
    {
        ScreenManager.instance.ShowNextScreen(ScreenType.GameOverPage);
        ScoreManager.instance.CheckPlayerHighScore(gamePlayScreen);
        ScoreManager.instance.ConnectGamePlayAndGameOverScore(gameOverScreen);
        ResetBowPos();
        //GamePlayScreen.inst.currentScore.text = "Score : " + 0;
    }

    void ResetBowPos()
    {
        transform.SetPositionAndRotation(initialPos, initialRotation);
    }

    public void SpwanNewArrow()
    {
        for (int i = 0; i < spwanArrowPoint.Count; i++)
        {
            Arrow arrow = Instantiate(arrowPrefab, spwanArrowPoint[i].position, spwanArrowPoint[i].rotation);
            allArrows.Add(arrow);
        }
    }

    public void AssignArrow(Arrow arrow)
    {
        currentArrow = arrow;
    }

    public void UnAssignArrow()
    {
        currentArrow = null;
        ResetStringPos();       
    }

    public void ResetStringPos()
    {
        pointBetweenStartAndEnd.localPosition = initialArrowPos;
        UpdatePullingString(pointBetweenStartAndEnd.localPosition);
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
                arrow.ArrorDestroyer();
                if (allArrows.Count == 0)
                {
                    Invoke(nameof(GameOverPage), 1f);
                }
                tempArrow = null;
            }
        }
    }

    //public void CheckValidThrowOrNot()
    //{

    //    float dis = Vector3.Distance(distance.position, tempArrow.transform.position);
    //    Debug.Log("bunss" + dis);
    //    if (dis < 1f)
    //    {            
    //        tempArrow.ResetArrowPos();
    //        tempArrow = null;
    //    }
    //    else
    //    {
    //        Debug.Log("is Remove");
    //        allArrows.Remove(tempArrow);
    //        Debug.Log("Arrow List Count: " + allArrows.Count);
    //        tempArrow = null;
    //    }
    //}


    public void BowThrower(float forcePower)
    {
        tempArrow = currentArrow;
        currentArrow.transform.position = currentArrow.ModelArrow.transform.position;
        currentArrow.ModelArrow.localPosition = Vector3.zero;
        Vector3 force = currentArrow.transform.forward * forcePower;
        currentArrow.Thrower(force);
        UnAssignArrow();
        //Invoke(nameof(CheckValidThrowOrNot), 2f);
    }

    public void AddForceToArrow(Arrow arrow)
    {
        if (currentArrow == arrow)
        {
            forcePower = PullValue();
            BowThrower(forcePower);
        }
    }

    [ContextMenu("Throw Arrow")]
    public void TestThrowArrow()
    {
        forcePower = PullValue();
        BowThrower(forcePower);
    }

    public float PullValue()
    {
        float pullDirection = Vector3.Distance(arrowStartPoint.position, arrowEndPoint.position);
        float targetDirection = Vector3.Distance(arrowStartPoint.position, pointBetweenStartAndEnd.position);
        float pullValue = targetDirection / pullDirection;
        float t = Mathf.Clamp(pullValue, 0, 1);
        float streach = minValue + t * (maxValue - minValue);
        Debug.Log("streach : " + streach);
        //return endPointPos;
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
    public void UpdatePullingString(Vector3 updatedString)
    {
        Vector3 linePosition = updatedString;
        bowString.SetPosition(1, linePosition);
    }
}
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