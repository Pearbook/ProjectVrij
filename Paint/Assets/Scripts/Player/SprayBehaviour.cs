using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayBehaviour : MonoBehaviour
{
    public GameObject SprayParticle;

    private void Start()
    {
        if (SprayParticle != null)
            Instantiate(SprayParticle, transform.position, Quaternion.identity);
    }
}
