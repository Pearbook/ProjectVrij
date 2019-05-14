using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Transform MyCamera;

    public float GraffitiRange;
    public LayerMask GraffitiMask;

    public GameObject GraffitiPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SprayGraffiti();
    }

    void SprayGraffiti()
    {
        RaycastHit hit;

        if(Physics.Raycast(MyCamera.position, MyCamera.forward, out hit, GraffitiRange, GraffitiMask))
        {
            if(hit.collider != null)
            {
                GameObject prefab = (GameObject)Instantiate(GraffitiPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}
