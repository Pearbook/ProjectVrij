using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioSource> BucketAudio;
    public List<AudioSource> PowerUpSource;

    public void PlayRefillAudio(int id)
    {
        if (!BucketAudio[id - 1].isPlaying)
            BucketAudio[id - 1].Play();
    }

    public void StopRefillAudio(int id)
    {
        if (BucketAudio[id - 1].isPlaying)
            BucketAudio[id - 1].Stop();
    }

    public void PlayPickUpAudio(int id)
    {
        if (!PowerUpSource[id - 1].isPlaying)
            PowerUpSource[id - 1].Play();
    }
}
