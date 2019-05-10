using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Serializable]
    public class MovementSettings
    {
        public float ForwardSpeed = 8.0f;
        public float StrafeSpeed = 4.0f;
        public float BackwardSpeed = 4.0f;

        public float ForwardAirSpeed = 4;
        public float StrafeAirSpeed = 1;
        public float BackwardAirSpeed = 0.5f;

        public float AirControl = 2;
        public float JumpForce = 30.0f;

        public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));

        public float SlowDownRate = 5.0f;

        [HideInInspector] public float CurrentTargetSpeed = 8f;

    }

    [Serializable]
    public class GroundCheckSettings
    {
        public float GroundCheckRadius = 0.5f;
        public float Offset = 0.5f;
        public LayerMask GroundMask;
    }

    public Camera Cam;

    public MovementSettings movementSettings = new MovementSettings();
    public GroundCheckSettings groundCheckSettings = new GroundCheckSettings();
    public MouseLook mouseLook = new MouseLook();

    private bool isGrounded;
    private Vector3 groundContactNormal;

    private bool jump;
    private bool isJumping;

    [HideInInspector]
    public Rigidbody myRigidbody;

    public bool Grounded
    {
        get { return isGrounded; }
    }

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        mouseLook.Init(transform, Cam.transform);
    }

    private void Update()
    {
        RotateView();

        if (Input.GetKeyDown(KeyCode.Space) && !jump)
            jump = true;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Vector2 input = GetInput();

        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon))
        {
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = Cam.transform.forward * input.y + Cam.transform.right * input.x;
            desiredMove = Vector3.ProjectOnPlane(desiredMove, groundContactNormal).normalized;

            desiredMove.x = desiredMove.x * movementSettings.CurrentTargetSpeed;
            desiredMove.z = desiredMove.z * movementSettings.CurrentTargetSpeed;
            desiredMove.y = desiredMove.y * movementSettings.CurrentTargetSpeed;

            if (myRigidbody.velocity.sqrMagnitude < (movementSettings.CurrentTargetSpeed * movementSettings.CurrentTargetSpeed))
            {
                myRigidbody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
            }

        }

        if (isGrounded)
        {
            myRigidbody.drag = movementSettings.SlowDownRate;

            if(jump)
            {
                myRigidbody.drag = 0;
              
                // myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, 0f, myRigidbody.velocity.z);
                myRigidbody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                isJumping = true;
            }
        }
        else
        {
            myRigidbody.drag = 0;
        }

        jump = false;
    }

    private Vector2 GetInput()
    {
        Vector2 input = new Vector2
            {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            };
        UpdateDesiredTargetSpeed(input);
        return input;
    }

    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        mouseLook.LookRotation(transform, Cam.transform);

        if (isGrounded)
        {
            // Rotate the rigidbody velocity to match the new direction that the character is looking
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
             myRigidbody.velocity = velRotation * myRigidbody.velocity;
        }
    }

    private float SlopeMultiplier()
    {
        float angle = Vector3.Angle(groundContactNormal, Vector3.up);
        return movementSettings.SlopeCurveModifier.Evaluate(angle);
    }



    private float AirMultiplier()
    {

        if(isGrounded)
            return movementSettings.AirControl;
        else
            return 0.3f;
    }


    public void UpdateDesiredTargetSpeed(Vector2 input)
    {
        if (isGrounded)
        {
            if (input.x > 0 || input.x < 0)
            {
                //strafe
                movementSettings.CurrentTargetSpeed = movementSettings.StrafeSpeed;
            }
            if (input.y < 0)
            {
                //backwards
                movementSettings.CurrentTargetSpeed = movementSettings.BackwardSpeed;
            }
            if (input.y > 0)
            {
                //forwards
                //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                movementSettings.CurrentTargetSpeed = movementSettings.ForwardSpeed;
            }
        }
        else
        {
            if (input.x > 0 || input.x < 0)
            {
                //strafe
                movementSettings.CurrentTargetSpeed = movementSettings.StrafeAirSpeed;
            }
            if (input.y < 0)
            {
                //backwards
                movementSettings.CurrentTargetSpeed = movementSettings.BackwardAirSpeed;
            }
            if (input.y > 0)
            {
                //forwards
                //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                movementSettings.CurrentTargetSpeed = movementSettings.ForwardAirSpeed;
            }
        }
    }

    private void GroundCheck()
    {
        RaycastHit hit;

        if(Physics.SphereCast(transform.position, groundCheckSettings.GroundCheckRadius, Vector3.down, out hit, groundCheckSettings.Offset, groundCheckSettings.GroundMask, QueryTriggerInteraction.Ignore))
        {
            isGrounded = true;
            groundContactNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            groundContactNormal = Vector3.up;
        }

        if (isJumping)
            isJumping = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - groundCheckSettings.Offset, transform.position.z), groundCheckSettings.GroundCheckRadius);
    }
}
