using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerPrefab;

    public List<PlayerProperties> AllPlayers;

    public List<GameObject> PlayerOnePrays;
    public List<GameObject> PlayerTwoPrays;

    public void GetStunned(int myID)
    {
        print("GET STUNNED BITCH");
        DisableContol(myID);
    }

    public void DisableContol(int myID)
    {
        AllPlayers[myID - 1].gameObject.GetComponent<PlayerMovement>().AllowControl = false;
    }

    public void AddPlayerScore(int myID, int amount)
    {
        AllPlayers[myID - 1].Score += amount;
    }

    public GameObject GetPlayerSpray(int myID)
    {
        if (myID == 1)
            return PlayerOnePrays[0];
        if(myID == 2)
            return PlayerTwoPrays[0];

        return null;
    }

    public  Vector3 GetInput(int myID)
    {
        Vector3 input = new Vector3();

        if (myID == 1)
        {
            return input = new Vector3
            {
                x = Input.GetAxis("Horizontal_p1"),
                y = 0,
                z = Input.GetAxis("Vertical_p1")
            };
        }
        else if (myID == 2)
        {
            return input = new Vector3
            {
                x = Input.GetAxis("Horizontal_p2"),
                y = 0,
                z = Input.GetAxis("Vertical_p2")
            };
        }

        return input;
    }
}
