using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillBehaviour : MonoBehaviour
{
    public float Radius;
    public LayerMask PlayerMask;

    public float RefillRate;

    private void Update()
    {
        CheckForPlayer();
    }

    void CheckForPlayer()
    {
        Collider[] coll = Physics.OverlapSphere(transform.position, Radius, PlayerMask);

        if(coll.Length > 0)
        {
            coll[0].GetComponentInParent<PlayerProperties>().RefillPaint(RefillRate);
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
