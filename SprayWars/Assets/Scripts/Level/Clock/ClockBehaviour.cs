using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockBehaviour : MonoBehaviour
{
    public Transform ClockHandle;
    public Image ClockUI;

    private void Update()
    {
        if (GameManager.Gameplay.timerIsOn)
        {
            ClockHandle.localEulerAngles = new Vector3(0, 0, Custom.map(GameManager.Gameplay.timer, 0, GameManager.Gameplay.MaxTime, 0, 360));
            ClockUI.fillAmount = Custom.map(GameManager.Gameplay.timer, 0, GameManager.Gameplay.MaxTime, 1, 0);
        }
    }
}
