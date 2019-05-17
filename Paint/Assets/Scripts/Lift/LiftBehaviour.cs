using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftBehaviour : MonoBehaviour
{
    public Rigidbody MyRigidbody;

    public float MovementSpeed;

    private void FixedUpdate()
    {
        MyRigidbody.velocity = Vector3.up * MovementSpeed;
    }

}
