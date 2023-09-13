using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowClampArrow : MonoBehaviour
{
    [SerializeField] BowController bow;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform movePoint;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("IsTriggerEnter");
        Arrow arrow = other.gameObject.GetComponent<Arrow>();
        if (arrow)
        {
            bow.AssignArrow(arrow);
            arrow.xrGrabInteractable.trackRotation = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Arrow arrow = other.gameObject.GetComponent<Arrow>();
        if (bow.currentArrow == null) return;
        if (bow.currentArrow == arrow)
        {
            //arrow.xrGrabInteractable.trackRotation = false;
            Debug.Log("IsTrigger Stay");
            ArrowRotateTowardsBow();
            movePoint.position = bow.NearestPointOnFiniteLine(startPoint.position, endPoint.position, bow.currentArrow.transform.position);
            bow.UpdatePullingString(movePoint.localPosition);
            Vector3 dir = startPoint.position - endPoint.position;
            bow.currentArrow.ModelArrow.transform.position = bow.NearestPointOnLine(startPoint.position, dir, bow.currentArrow.ModelArrow.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Arrow arrow = other.gameObject.GetComponent<Arrow>();
        if (bow.currentArrow == null) return;
        if (bow.currentArrow == arrow)
        {
            bow.currentArrow.transform.position = bow.currentArrow.ModelArrow.transform.position;
            bow.currentArrow.ModelArrow.localPosition = Vector3.zero;
            arrow.xrGrabInteractable.trackRotation = true;
            bow.UnAssignArrow();
            Debug.Log("IsTriggerExit");
        }
    }

    void ArrowRotateTowardsBow()
    {
        Quaternion desiredRotation = Quaternion.LookRotation(bow.transform.forward, Vector3.up);
        float lerpSpeed = 0.8f;
        bow.currentArrow.transform.rotation = Quaternion.Lerp(bow.currentArrow.transform.rotation, desiredRotation, lerpSpeed);
    }
}

// Calculate the desired rotation (look rotation) based on your bow's rotation
// Lerp the rotation of the arrow towards the desired rotation
// Adjust the speed of the rotation

//Debug.Log("isRotating");
//timeCount = 0f;
//while (timeCount < duration)
//{
//    Debug.Log("Is In while");            
//    bow.currentArrow.transform.rotation = Quaternion.Lerp(bow.currentArrow.transform.rotation, bow.transform.rotation, timeCount / duration);
//    timeCount += Time.deltaTime;
//    //bow.currentArrow.transform.rotation = rotateArrow;
//}
////bow.currentArrow.transform.rotation = bow.transform.rotation;

//if (bow.currentArrow == null) return;
//bow.ResetPos();
//if (bow.currentArrow == null) return;
//bow.ResetPos();
//bow.SpwanNewArrow();
//private void ResetKinematic()
//{
//    if(movePoint.position == initialPos)
//    rb.isKinematic = false;
//}

//if (bow.currentArrow == null)
//{
//    bow.ResetPos();
//}


//movePoint.position = other.gameObject.transform.position;
//distance = Vector3.Distance(endPoint.position, movePoint.position);
//if (distance > 0.3f)
//{
//    rb.isKinematic = true;
//    other.gameObject.GetComponent<Arrow>().InstantDestroy();
//    if (bow.currentArrow == null) return;
//    bow.ResetPos();
//    bow.SpwanNewArrow();
//    //ResetKinematic();
//}        