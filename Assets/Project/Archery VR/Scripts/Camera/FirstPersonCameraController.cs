using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonCameraController : MonoBehaviour
{
    [SerializeField] BowController arrowController;
    [SerializeField] Transform playerBody;
    [SerializeField] float mouseSensitivity = 100f;   
    [SerializeField] float minAngle;
    [SerializeField] float maxAngle;

    private float rotationX;
    private float rotationY;


    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }
   
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;

        rotationY += mouseX;
        rotationX -= mouseY;
        rotationY = Mathf.Clamp(rotationY, minAngle, maxAngle);
        rotationX = Mathf.Clamp(rotationX, minAngle, maxAngle);
        
        //playerBody.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        //if (arrowController.currentArrow == null) return;
        //arrowController.currentArrow.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        transform.rotation = rotation;
    }

    //private void LateUpdate()
    //{
               
    //}

   
}


