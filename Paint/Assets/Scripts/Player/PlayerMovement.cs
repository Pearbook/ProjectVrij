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

        [Header("Jumping")]
        public float JumpForce;
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
    public GroundCheckSettings groundCheckSettings = new GroundCheckSettings();

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
            
    }

    private void FixedUpdate()
    {
        // Check if the player is grounded or not.
        GroundCheck();

        // Get player input.
        Vector3 input = GameManager.Player.GetInput(GetComponent<PlayerProperties>().PlayerID);

        // Move the player by adding force to the rigidbody.
        myRigidbody.AddForce(input * movementSettings.MovementSpeed, ForceMode.Impulse);

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
    }
}
