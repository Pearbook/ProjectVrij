using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform Lift;

    public bool AllowControl = true;

    [HideInInspector]
    public Vector2 InputAxis;

    public float MovementSpeed = 5f;
    public float lerpRate = 0.5f;

    private Vector3 prevPos;
    private Vector3 prevLiftPos;

    private void Start()
    {
        prevPos = transform.position;
        prevLiftPos = Lift.position;
    }

    private void Update()
    {
        if (GameManager.Gameplay.GameHasStarted)
        {
            if (transform.position != prevPos)
            {
                GetComponent<PlayerProperties>().isMoving = true;
                prevPos = transform.position;
            }
            else
                GetComponent<PlayerProperties>().isMoving = false;

            if (Lift.position != prevLiftPos)
            {
                GetComponent<PlayerProperties>().isMoving = true;
                prevLiftPos = Lift.position;
            }
            else
                GetComponent<PlayerProperties>().isMoving = false;


            if (GetComponent<PlayerProperties>().AllowControl)
            {
                InputAxis = GameManager.Player.GetInput(GetComponent<PlayerProperties>().PlayerID);

                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + InputAxis.x), 1.0f - Mathf.Exp(-MovementSpeed * Time.deltaTime));
                Lift.position = Vector3.Lerp(Lift.position, new Vector3(Lift.position.x, Lift.position.y + InputAxis.y, Lift.position.z), 1.0f - Mathf.Exp(-lerpRate * Time.deltaTime));
            }

            Vector3 clammedPos = transform.position;
            clammedPos.z = Mathf.Clamp(clammedPos.z, -14f, -1f);
            transform.position = clammedPos;

            Vector3 clammedLift = Lift.position;
            clammedLift.y = Mathf.Clamp(clammedLift.y, 0, 13.25f);
            Lift.position = clammedLift;
        }
    }
}
