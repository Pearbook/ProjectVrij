using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Serializable]
    public class MovementSettings
    {
        [Header ("Moving")]
        public float MovementSpeed;
        public float SlowdownRate;

        [HideInInspector]
        public float currentTargetSpeed;

        [Header("Jumping")]
        public float JumpForce;
        public float AirMovementSpeed;
    }

    [Serializable]
    public class BounceSettings
    {
        public float BounceCheckRadius;
        public Vector3 BounceCheckSize;
        public Vector3 Offset;
        public LayerMask BounceMask;
    }

    [Serializable]
    public class GroundCheckSettings
    {
        public float GroundCheckRadius = 0.5f;
        public Vector3 Offset;
        public LayerMask GroundMask;
    }

    public Rigidbody myRigidbody;

    public MovementSettings movementSettings = new MovementSettings();
    public BounceSettings bounceSettings = new BounceSettings();
    public GroundCheckSettings groundCheckSettings = new GroundCheckSettings();

    public bool AllowControl = true;

    public float TurnSpeed;
    public bool ignoreY;

    private bool isGrounded;

    private bool jump;
    private bool isJumping;

    private bool isButtonPressed;

    private void Update()
    {
        if (GetComponent<PlayerProperties>().PlayerID == 1)
        {
            ButtonPress("Jump_p1");
        }
        else if (GetComponent<PlayerProperties>().PlayerID == 2)
        {
            ButtonPress("Jump_p2");
        }

        //rotates rigidbody to face its current velocity public void RotateToVelocity(float turnSpeed, bool ignoreY) {
        Vector3 dir; if (ignoreY) dir = new Vector3(myRigidbody.velocity.x, 0f, myRigidbody.velocity.z); else dir = myRigidbody.velocity;

        if (dir.magnitude > 0.1)
        {
            Quaternion dirQ = Quaternion.LookRotation(dir);
            Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, dir.magnitude * TurnSpeed * Time.deltaTime);
            myRigidbody.MoveRotation(slerp);
        }
    }

    private void FixedUpdate()
    {
        // Check if the player is grounded or not.
        GroundCheck();

        BounceCheck();

        UpdateDesiredTargetSpeed();

        
        if (AllowControl)
        {
            // Get player input.
            Vector3 input = GameManager.Player.GetInput(GetComponent<PlayerProperties>().PlayerID);

            // Move the player by adding force to the rigidbody.
            myRigidbody.AddForce(input * movementSettings.currentTargetSpeed, ForceMode.Impulse);
        }

        if (isGrounded)
        {
            myRigidbody.drag = movementSettings.SlowdownRate;

            if (jump)
            {
                myRigidbody.drag = 0;

                myRigidbody.AddForce(new Vector3(0, movementSettings.JumpForce, 0), ForceMode.Impulse);
                isJumping = true;
            }
        }
        else
        {
            myRigidbody.drag = 0;
        }

        jump = false;
    }

    void UpdateDesiredTargetSpeed()
    {
        if (isGrounded)
            movementSettings.currentTargetSpeed = movementSettings.MovementSpeed;
        else
            movementSettings.currentTargetSpeed = movementSettings.AirMovementSpeed;
    }

    private void GroundCheck()
    {
        //RaycastHit hit;

        Collider[] coll = Physics.OverlapSphere(transform.position + groundCheckSettings.Offset, groundCheckSettings.GroundCheckRadius, groundCheckSettings.GroundMask);

        if(coll.Length > 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isJumping)
            isJumping = false;
    }

    private void BounceCheck()
    {
        Collider[] coll = Physics.OverlapBox(transform.position + bounceSettings.Offset, bounceSettings.BounceCheckSize/2, Quaternion.identity, bounceSettings.BounceMask);

        if (coll.Length > 0)
        {
            myRigidbody.AddForce(new Vector3(0, movementSettings.JumpForce, 0), ForceMode.Impulse);
            //GameManager.Player.GetStunned(coll[0].GetComponent<PlayerProperties>().PlayerID);
        }
    }

    void ButtonPress(string key)
    {
        if (Input.GetAxisRaw(key) != 0)
        {
            if (isButtonPressed == false)
            {

                if (!jump)
                    jump = true;

                isButtonPressed = true;
            }
        }
        if (Input.GetAxisRaw(key) == 0)
        {
            isButtonPressed = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x + groundCheckSettings.Offset.x, transform.position.y + groundCheckSettings.Offset.y, transform.position.z + groundCheckSettings.Offset.z), groundCheckSettings.GroundCheckRadius);

        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(new Vector3(transform.position.x + bounceSettings.Offset.x, transform.position.y + bounceSettings.Offset.y, transform.position.z + bounceSettings.Offset.z), bounceSettings.BounceCheckRadius);
        Gizmos.DrawWireCube(transform.position + bounceSettings.Offset, bounceSettings.BounceCheckSize);
    }
}
