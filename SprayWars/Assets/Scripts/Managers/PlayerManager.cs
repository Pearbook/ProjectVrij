using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerPrefab;

    public List<PlayerProperties> AllPlayers;
    public List<PlayerAudio> PlayerAudio;

    [Header ("Brushes")]
    public Texture2D PlayerOneBrush;
    public Texture2D PlayerTwoBrush;

    [Header ("Beams")]
    public List<GameObject> BeamPrefabs;

    [Header("Lines")]
    public List<LineRenderer> PlayerLines;

    public void DisableContol()
    {
        for(int i = 0; i < AllPlayers.Count; ++i)
        {
            AllPlayers[i].AllowControl = false;
            AllPlayers[i].AllowPainting = false;
        }
    }

    public void EnableControl()
    {
        for (int i = 0; i < AllPlayers.Count; ++i)
        {
            AllPlayers[i].AllowControl = true;
            AllPlayers[i].AllowPainting = true;
        }
    }

    public Texture2D GetPlayerBrush(int myID)
    {
        if (myID == 1)
            return PlayerOneBrush;
        if (myID == 2)
            return PlayerTwoBrush;

        return null;
    }

    public  Vector2 GetInput(int myID)
    {
        Vector3 input = new Vector3();

        if (myID == 1)
        {
            return input = new Vector3
            {
                x = -Input.GetAxis("Horizontal_p1"),
                y = Input.GetAxis("Vertical_p1")
            };
        }
        else if (myID == 2)
        {
            return input = new Vector3
            {
                x = Input.GetAxis("Horizontal_p2"),
                y = Input.GetAxis("Vertical_p2")
            };
        }

        return input;
    }
}
