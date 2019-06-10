using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{

    [Header("Timer")]
    [Tooltip("Time in seconds.")]
    public float TimerLimit;

    private float timer;
    private float seconds;

    private bool startTimer;

    private void Start()
    {
        startTimer = true;
    }

    private void Update()
    {
        if (startTimer)
        {
            if (seconds >= TimerLimit)
            {
                DestroyObj();
            }
            else
            {
                UpdateTimer();
            }
        }
    }

    void DestroyObj()
    {
        Destroy(this.gameObject);
    }

    public void UpdateTimer()
    {
        timer += Time.deltaTime;
        seconds = timer % 60;
    }
}
