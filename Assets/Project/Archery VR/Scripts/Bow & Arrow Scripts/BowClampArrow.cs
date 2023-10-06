using UnityEngine;
using Yudiz.VRArchery.Managers;

namespace Yudiz.VRArchery.CoreGameplay
{
    public class BowClampArrow : MonoBehaviour
    {
        [SerializeField] private Bow bow;
        [SerializeField] private GameController gameController;

        private void Start()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            Arrow arrow = other.gameObject.GetComponent<Arrow>();
            if (arrow)
            {
                //bow.trajectoryLine.enabled = true;
                //gameController.AssignArrow(arrow);
                arrow.xrGrabInteractable.trackRotation = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Arrow arrow = other.gameObject.GetComponent<Arrow>();
            if (gameController.currentArrow == null) return;
            if (gameController.currentArrow == arrow)
            {
                //Debug.Log("IsTrigger Stay");
                ArrowRotateTowardsBow();
                bow.pointBetweenStartAndEnd.position = gameController.NearestPointOnFiniteLine(bow.arrowStartPoint.position, bow.arrowEndPoint.position, gameController.currentArrow.transform.position);
                bow.UpdatePullingString(bow.pointBetweenStartAndEnd.localPosition);
                var dir = bow.arrowStartPoint.position - bow.arrowEndPoint.position;
                gameController.currentArrow.ModelArrow.transform.position = gameController.NearestPointOnLine(bow.arrowStartPoint.position, dir, gameController.currentArrow.ModelArrow.transform.position);
                gameController.forcePower = gameController.GetForceValue();
                //bow.CalculateTrajectory(gameController.forcePower, bow.trajectoryLine, other.GetComponent<Rigidbody>());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Arrow arrow = other.gameObject.GetComponent<Arrow>();
            if (gameController.currentArrow == null) return;
            if (gameController.currentArrow == arrow)
            {
                bow.trajectoryLine.enabled = false;
                gameController.currentArrow.transform.position = gameController.currentArrow.ModelArrow.transform.position;
                gameController.currentArrow.ModelArrow.localPosition = Vector3.zero;
                arrow.xrGrabInteractable.trackRotation = true;
                //gameController.UnAssignArrow();
                //Debug.Log("IsTriggerExit");
            }
        }

        void ArrowRotateTowardsBow()
        {
            Quaternion desiredRotation = Quaternion.LookRotation(bow.transform.forward, Vector3.up);
            float lerpSpeed = 0.8f;
            gameController.currentArrow.transform.rotation = Quaternion.Lerp(gameController.currentArrow.transform.rotation, desiredRotation, lerpSpeed);
        }
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