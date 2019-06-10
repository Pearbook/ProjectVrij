using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBehaviour : MonoBehaviour
{
    public float FallSpeed;

    public Rigidbody MyBody;

    public Vector3 Size;

    public GameObject ImpactPrefab;

    private void FixedUpdate()
    {
        MyBody.velocity = new Vector3(0, -FallSpeed, 0);
        MyBody.angularVelocity = new Vector3(10,10,10);

        CheckForImpact();
    }

    void CheckForImpact()
    {
        Collider[] coll = Physics.OverlapBox(transform.position, Size);

        if(coll.Length > 0)
        {
            Instantiate(ImpactPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Size);
    }
}
