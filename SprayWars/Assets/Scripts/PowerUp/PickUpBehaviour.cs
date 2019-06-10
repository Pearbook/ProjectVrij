using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBehaviour : MonoBehaviour
{
    public GameObject CloudPowerUp;
    public GameObject RockPowerUp;

    private Vector3 dir;
    private bool isActive;

    public void PickUp(int id)
    {
        if (!isActive)
        {
            if (id == 1)
                dir = Vector3.left;
            if (id == 2)
                dir = Vector3.right;

            if (CloudPowerUp != null)
            {
                GameObject obj = (GameObject)Instantiate(CloudPowerUp, Vector3.zero, Quaternion.identity);
                obj.GetComponent<CloudBehaviour>().Direction = dir;
            }

            if (RockPowerUp != null)
            {
                GameObject obj = (GameObject)Instantiate(RockPowerUp, Vector3.zero, Quaternion.identity);
                obj.GetComponent<RockSpawnBehaviour>().Direction = dir;
            }

            isActive = true;
        }
        else
        {
            GameManager.Gameplay.PowerUps.RemovePowerUp(id);
        }
    }
}
