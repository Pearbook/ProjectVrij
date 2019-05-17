using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBehaviour : MonoBehaviour
{
    public float MovementSpeed = 0.15f;

    public float TowerHeight;

    public void Update()
    {
        //transform.position = transform.position + Vector3.down * MovementSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, -TowerHeight, 0), MovementSpeed);
    }
}
