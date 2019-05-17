using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTriggerBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Gameplay.RespawnPlayer(other.gameObject);
    }
}
