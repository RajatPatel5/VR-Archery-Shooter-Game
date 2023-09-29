using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Yudiz.VRArchery.Managers;

namespace Yudiz.VRArchery.CoreGameplay
{
    public class Arrow : MonoBehaviour
    {
        #region PUBLIC_VARS
        public XRGrabInteractable xrGrabInteractable;
        public Transform ModelArrow;
        public bool isColliding;

        [Header("Particles")]
        public ParticleSystem trailParticle;
        public ParticleSystem hitParticle;
        public TrailRenderer trailRenderer;
        public TrailRenderer modelTrialRenderer;
        //public static event Action<Arrow> OnThisArrowAddForce;
        //public static event Action<Arrow> OnCollisionShouldResetOrNot;
        #endregion

        #region PRIVATE_VARS
        //[SerializeField] private BoxCollider frontCollider;
        //[SerializeField] private SphereCollider backCollider;        
        [SerializeField] private Rigidbody arrowRB;
        private bool isReadyToThrow = false;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        //private bool dropByMistake;
        #endregion


        #region UNITY_CALLBACKS
        private void Start()
        {
            isColliding = true;
            modelTrialRenderer.enabled = false;
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            //frontCollider = GetComponent<BoxCollider>();
            //backCollider = GetComponent<SphereCollider>();
            arrowRB = GetComponent<Rigidbody>();
            //frontCollider.enabled = false;
            //backCollider.enabled = true;
            xrGrabInteractable.selectEntered.AddListener(OnGrabingArrow);
            xrGrabInteractable.selectExited.AddListener(OnLeavingArrowCall);

        }

        private void FixedUpdate()
        {
            LookTowardsItsVelocity();
            CheckArrowIsResetOrShoot();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collide WIth : " + collision.collider.gameObject.name);
            ScoreOnCube score = collision.collider.GetComponent<ScoreOnCube>();            
            if (score)
            {
                score.UpdateScore();                
                SetPhysics(false);
                //isReadyToThrow = false;
                //arrowRB.velocity = Vector3.zero;
                //GameController.inst.tempArrow = null;
                GameController.inst.allArrows.Remove(this);
                GameController.inst.CheckGameOver();
                //Destroy(gameObject, 10);
                //isColliding = false;
                ArrowParticles(false);
            }       

            else if (collision.collider.CompareTag("Ground"))
            {
                Debug.Log("is Grounded");
                ArrowParticles(false);
                isReadyToThrow = false;
                arrowRB.velocity = Vector3.zero;
                isColliding = false;
            }
            else if (collision.collider.CompareTag("Wall"))
            {
                ArrowParticles(false);
                isReadyToThrow = false;
                arrowRB.velocity = Vector3.zero;
                isColliding = false;
            }          
        }
        #endregion


        #region PRIVATE_FUNCTIONS
        private void LookTowardsItsVelocity()
        {
            if (isReadyToThrow)
            {
                transform.rotation = Quaternion.LookRotation(arrowRB.velocity);
            }
        }

        private void CheckArrowIsResetOrShoot()
        {
            if (!isColliding)
            {
                //OnCollisionShouldResetOrNot?.Invoke(this);
                GameEvents.onCheckDistance?.Invoke(this);
            }
        }      

        private void OnGrabingArrow(SelectEnterEventArgs arg0)
        {            
            SetPhysics(false);
            isReadyToThrow = false;
            isColliding = true;
            xrGrabInteractable.trackRotation = true;
            GameController.inst.currentArrow = this;
            if (arg0.interactorObject is XRDirectInteractor)
            {
                arg0.interactorObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        private void OnLeavingArrowCall(SelectExitEventArgs arg0)
        {
            SetPhysics(true);
            CheckThrowable();
            xrGrabInteractable.trackRotation = true;
            GameController.inst.currentArrow = null;
            arg0.interactorObject.transform.GetChild(0).gameObject.SetActive(true);
        }

        [ContextMenu("THis Arrow ")]
        private void TakeThisArrow()
        {
            GameController.inst.currentArrow = this;
        }

        private void SetPhysics(bool usePhysics)
        {
            arrowRB.useGravity = usePhysics;
            arrowRB.isKinematic = !usePhysics;
        }

        void ArrowParticles(bool release)
        {
            if (release)
            {
                trailParticle.Play();
                trailRenderer.emitting = true;
            }
            else
            {
                trailParticle.Stop();
                hitParticle.Play();
                trailRenderer.emitting = false;
            }
        }
        #endregion


        #region PUBLIC_FUNCTIONS
        public void ResetArrowPos()
        {
            ArrowParticles(false);
            transform.SetPositionAndRotation(initialPosition, initialRotation);
            isColliding = true;
        }
        
        public void Thrower(Vector3 force)
        {
            //trialLine.enabled = true;
            ArrowParticles(true);
            SetPhysics(true);            
            arrowRB.AddForce(force, ForceMode.Impulse);
            isReadyToThrow = true;
            modelTrialRenderer.enabled = true;
            //dropByMistake = false;
            //ArrorDestroyer();
        }

        public void CheckThrowable()
        {
            if(GameController.inst.canThrowArrow)
            {
                GameEvents.onArrowThrowen?.Invoke();
            }
            else
            {
                Invoke(nameof(ResetArrowPos), 0.3f);
            }
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
