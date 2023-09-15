using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow : MonoBehaviour
{
    [SerializeField] private CapsuleCollider frontCollider;
    [SerializeField] private BoxCollider backCollider;
    [SerializeField] private TrailRenderer trialLine;
    [SerializeField] private Rigidbody arrowRB;

    private bool isReadyToThrow = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool dropByMistake;

    public XRGrabInteractable xrGrabInteractable;
    public Transform ModelArrow;
    public bool isThrown;
    public static event Action<Arrow> OnThisArrowAddForce;
    public static event Action<Arrow> OnCollisionShouldResetOrNot;


    void Start()
    {
        isThrown = true;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        frontCollider = GetComponent<CapsuleCollider>();
        backCollider = GetComponent<BoxCollider>();
        arrowRB = GetComponent<Rigidbody>();
        frontCollider.enabled = false;
        backCollider.enabled = true;
        xrGrabInteractable.selectEntered.AddListener(OnGrabingArrow);
        xrGrabInteractable.selectExited.AddListener(OnLeavingArrowCall);
    }

    private void FixedUpdate()
    {
        LookTowardsItsVelocity();
        CheckArrowIsResetOrShoot();
    }

    private void LookTowardsItsVelocity()
    {
        if (isReadyToThrow)
        {
            transform.rotation = Quaternion.LookRotation(arrowRB.velocity);
        }
    }

    private void CheckArrowIsResetOrShoot()
    {
        if (!isThrown)
        {
            OnCollisionShouldResetOrNot?.Invoke(this);
        }
    }

    private void OnGrabingArrow(SelectEnterEventArgs arg0)
    {
        frontCollider.enabled = false;
        backCollider.enabled = true;
        arrowRB.isKinematic = true;
        isReadyToThrow = false;
        isThrown = true;
        trialLine.enabled = false;
        dropByMistake = true;
        xrGrabInteractable.trackRotation = true;
    }

    private void OnLeavingArrowCall(SelectExitEventArgs arg0)
    {
        frontCollider.enabled = true;
        backCollider.enabled = true;
        OnThisArrowAddForce?.Invoke(this);
        arrowRB.isKinematic = false;
        Invoke(nameof(CheckDropArrowByMistake), 0.8f);
        xrGrabInteractable.trackRotation = true;
    }

    public void ResetArrowPos()
    {
        transform.SetPositionAndRotation(initialPosition, initialRotation);
        isThrown = true;
    }

    void CheckDropArrowByMistake()
    {
        if (dropByMistake)
            transform.SetPositionAndRotation(initialPosition, initialRotation);
    }

    public void Thrower(Vector3 force)
    {
        trialLine.enabled = true;
        arrowRB.isKinematic = false;
        arrowRB.AddForce(force, ForceMode.Impulse);
        isReadyToThrow = true;
        dropByMistake = false;
        //ArrorDestroyer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ScoreOnCube score = collision.collider.GetComponent<ScoreOnCube>();
        if (score)
        {
            score.UpdateScore();
            arrowRB.isKinematic = true;
            isThrown = false;
        }

        else if (collision.collider.CompareTag("Table"))
        {
            isReadyToThrow = false;
            arrowRB.velocity = Vector3.zero;
            isThrown = false;
        }

        else if (collision.collider.CompareTag("Ground"))
        {
            Debug.Log("is Grounded");
            isReadyToThrow = false;
            arrowRB.velocity = Vector3.zero;
            isThrown = false;
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            isReadyToThrow = false;
            arrowRB.velocity = Vector3.zero;
            isThrown = false;
        }
    }

    public void ArrorDestroyer()
    {
        Destroy(gameObject, 5f);
    }
}
