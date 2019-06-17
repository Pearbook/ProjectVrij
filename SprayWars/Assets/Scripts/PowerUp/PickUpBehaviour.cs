using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBehaviour : MonoBehaviour
{
    public GameObject CloudPowerUp;
    public GameObject RockPowerUp;

    public float Lifetime;

    private Vector3 dir;
    private bool isActive;

    private int id;

    private void Start()
    {
        StartCoroutine(WaitForRemove(Lifetime));
    }

    public void PickUp(int id)
    {
        this.id = id;

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

            GameManager.Audio.PlayPickUpAudio(id);

            GameManager.Gameplay.PowerUps.RemovePowerUp(id);

            Destroy(this.gameObject);
        }
        else
        {
            
        }
    }

    IEnumerator WaitForRemove(float time)
    {
        var i = 0.0f;
        var rate = 1.0f / time;

        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
