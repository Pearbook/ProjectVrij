using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockBehaviour : MonoBehaviour
{
    public Transform ClockHandle;

    private void Update()
    {
        if (GameManager.Gameplay.timerIsOn)
            ClockHandle.localEulerAngles = new Vector3(0, 0, Custom.map(GameManager.Gameplay.timer, 0, GameManager.Gameplay.MaxTime, 0, 360));
    }
}
