using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    static PlayerManager player;

    public static PlayerManager Player
    {
        get
        {
            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
                if (player == null)
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    player = obj.AddComponent<PlayerManager>();
                }
            }
            return player;
        }
    }

    public PlayerController Controller;
    public PlayerBehaviour Behaviour;

    public void EnablePlayer()
    {
        Controller.enabled = true;
        Behaviour.enabled = true;
    }

    public void DisablePlayer()
    {
        Controller.enabled = false;
        Behaviour.enabled = false;

        Controller.myRigidbody.velocity = new Vector3(0, Controller.myRigidbody.velocity.y, 0);
    }
}
