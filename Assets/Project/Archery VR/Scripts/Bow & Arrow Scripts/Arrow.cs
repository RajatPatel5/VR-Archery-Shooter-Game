using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow : MonoBehaviour
{
    Rigidbody arrowRB;
    XRGrabInteractable xrGrabInteractable;
    CapsuleCollider frontCollider;
    BoxCollider backCollider;

    public static event Action<Arrow> OnThisArrowAddForce;

    void Start()
    {        
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
        frontCollider = GetComponent<CapsuleCollider>();
        backCollider = GetComponent<BoxCollider>();
        arrowRB = GetComponent<Rigidbody>();        
        frontCollider.enabled = false;
        backCollider.enabled = true;
        xrGrabInteractable.selectEntered.AddListener(OnGrabingArrow);
        xrGrabInteractable.selectExited.AddListener(OnLeavingArrowCall);
    }

    private void OnGrabingArrow(SelectEnterEventArgs arg0)
    {
        arrowRB.isKinematic = true;
    }

    private void OnLeavingArrowCall(SelectExitEventArgs arg0)
    {
        OnThisArrowAddForce?.Invoke(this);
        arrowRB.isKinematic = false;
    }

    public void Thrower(Vector3 force)
    {
        arrowRB.isKinematic = false;
        frontCollider.enabled = true;
        backCollider.enabled = true;
        arrowRB.AddForce(force, ForceMode.Impulse);      
    }

    private void OnCollisionEnter(Collision collision)
    {
        ScoreOnCube score = collision.collider.GetComponent<ScoreOnCube>();
        if (score)
        {
            score.UpdateScore();
            arrowRB.isKinematic = true;
            ArrorDestroyer();
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            arrowRB.isKinematic = false;
            //ArrorDestroyer();
        }
        else if (collision.collider.CompareTag("Table"))
        {
            arrowRB.isKinematic = false;
            //ArrorDestroyer();
        }
    }
    public void InstantDestroy()
    {
        Destroy(gameObject);
    }

    public void ArrorDestroyer()
    {
        Destroy(gameObject, 5f);
    }
}
