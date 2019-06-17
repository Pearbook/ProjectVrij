using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource CanSource;
    public AudioSource EmptyCanSource;

    public void PlaySpraySound()
    {
        if(!CanSource.isPlaying)
            CanSource.Play();
    }

    public void StopSpraySound()
    {
        if (CanSource.isPlaying)
            CanSource.Stop();
    }

    public void PlayEmptySpraySound()
    {
        if (!EmptyCanSource.isPlaying)
            EmptyCanSource.Play();
    }
}
