using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerHandSwitcher : MonoBehaviour
{
    private XRDirectInteractor directInteractor;
    private GameObject controllerHand;

    private void Awake()
    {
        directInteractor = GetComponent<XRDirectInteractor>();
        controllerHand = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        if (directInteractor != null)
        {
            directInteractor.selectEntered.AddListener(OnInteractorUsed);
            directInteractor.selectExited.AddListener(OnInteractorReleased);
        }
    }
    private void OnDisable()
    {
        if (directInteractor != null)
        {
            directInteractor.selectEntered.RemoveListener(OnInteractorUsed);
            directInteractor.selectExited.RemoveListener(OnInteractorReleased);
        }
    }

    private void OnInteractorReleased(SelectExitEventArgs arg0)
    {
        controllerHand.gameObject.SetActive(true);
    }

    private void OnInteractorUsed(SelectEnterEventArgs arg0)
    {
        controllerHand.gameObject.SetActive(false);
    }
}
