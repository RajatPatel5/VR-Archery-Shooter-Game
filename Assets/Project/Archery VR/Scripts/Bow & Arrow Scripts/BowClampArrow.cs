using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowClampArrow : MonoBehaviour
{
    [SerializeField] BowController bow;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform movePoint;
    Vector3 initialPos;

    private void Start()
    {
        //initialPos = movePoint.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("IsTriggerEnter");
        Arrow arrow = other.gameObject.GetComponent<Arrow>();
        if (arrow)
        {
            bow.AssignArrow(arrow);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        Arrow arrow = other.gameObject.GetComponent<Arrow>();
        if (bow.currentArrow == null) return;
        if (bow.currentArrow == arrow)
        {
            Debug.Log("IsTrigger");
            movePoint.position = bow.NearestPointOnFiniteLine(startPoint.position, endPoint.position, bow.currentArrow.transform.position);
            bow.UpdatePullingString(movePoint.localPosition);
            Vector3 dir = startPoint.position - endPoint.position;
            bow.currentArrow.transform.position = bow.NearestPointOnLine(startPoint.position, dir, bow.currentArrow.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Arrow arrow = other.gameObject.GetComponent<Arrow>();
        if (bow.currentArrow == null) return;
        if (bow.currentArrow == arrow)
        {
            bow.UnAssignArrow();
            Debug.Log("IsTriggerExit");
        }
    }

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
}

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