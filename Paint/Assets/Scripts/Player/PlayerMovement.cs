using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Serializable]
    public class MovementSettings
    {
        public float MovementSpeed;
    }

    [Serializable]
    public class GroundCheckSettings
    {
        public float GroundCheckRadius = 0.5f;
        public float Offset = 0.5f;
        public LayerMask GroundMask;
    }

    public Rigidbody myRigidbody;

    public MovementSettings movementSettings = new MovementSettings();
    public GroundCheckSettings groundCheckSettings = new GroundCheckSettings();

    private bool isGrounded;

    private void FixedUpdate()
    {
        // Check if the player is grounded or not.
        GroundCheck();

        // Get player input.
        Vector3 input = GetInput();

        // Move the player by adding force to the rigidbody.
        myRigidbody.AddForce(input * movementSettings.MovementSpeed, ForceMode.Impulse);
    }

    private Vector3 GetInput()
    {
        Vector3 input = new Vector3
        {
            x = Input.GetAxis("Horizontal"),
            y = 0,
            z = Input.GetAxis("Vertical")
        };

        return input;
    }

    private void GroundCheck()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, groundCheckSettings.GroundCheckRadius, Vector3.down, out hit, groundCheckSettings.Offset, groundCheckSettings.GroundMask, QueryTriggerInteraction.Ignore))
            isGrounded = true;
        else
            isGrounded = false;

        /*
        if (isJumping)
            isJumping = false;*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - groundCheckSettings.Offset, transform.position.z), groundCheckSettings.GroundCheckRadius);
    }
}
