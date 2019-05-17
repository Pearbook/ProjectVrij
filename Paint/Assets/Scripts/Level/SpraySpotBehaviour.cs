using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpraySpotBehaviour : MonoBehaviour
{
    public bool hasBeenSprayed;

    public void SprayThisSpot(int myID)
    {
        if (!hasBeenSprayed)
        {
            hasBeenSprayed = true;

            GameManager.Player.AddPlayerScore(myID, GameManager.Gameplay.ScorePerSpray);

            Destroy(this.gameObject);
        }
    }
}
