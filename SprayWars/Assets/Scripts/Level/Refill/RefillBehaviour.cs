﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillBehaviour : MonoBehaviour
{
    public float Radius;
    public LayerMask PlayerMask;

    public float RefillRate;

    private int id;

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

            id = coll[0].GetComponentInParent<PlayerProperties>().PlayerID;

            if (id == 1)
                GameManager.Player.InRangeOne = true;

            if (id == 2)
                GameManager.Player.InRangeTwo = true;
        }
        else
        {
            if (id == 1)
                GameManager.Player.InRangeOne = false;

            if (id == 2)
                GameManager.Player.InRangeTwo = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
