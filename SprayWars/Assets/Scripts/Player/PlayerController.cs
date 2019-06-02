using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform Lift;

    Vector2 InputAxis;

    public float MovementSpeed = 5f;
    public float lerpRate = 0.5f;

    private void Update()
    {
        InputAxis = GameManager.Player.GetInput(GetComponent<PlayerProperties>().PlayerID);

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + InputAxis.x), 1.0f - Mathf.Exp(-MovementSpeed * Time.deltaTime));
        Lift.position = Vector3.Lerp(Lift.position, new Vector3(Lift.position.x, Lift.position.y + InputAxis.y, Lift.position.z), 1.0f - Mathf.Exp(-lerpRate * Time.deltaTime));

        Vector3 clammedPos = transform.position;
        clammedPos.z = Mathf.Clamp(clammedPos.z, -14f, -1f);
        transform.position = clammedPos;

        Vector3 clammedLift = Lift.position;
        clammedLift.y = Mathf.Clamp(clammedLift.y, 0, 13.25f);
        Lift.position = clammedLift;

    }
}
