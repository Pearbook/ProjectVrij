using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform Target;

    public float Speed;

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(new Vector3(0, transform.position.y, 0), new Vector3(0, Target.position.y, 0), Speed);
    }
}
