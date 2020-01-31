using UnityEngine.Audio;
using System;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public Audio[] soundFxs;

    void Awake()
    {
        foreach(Audio au in soundFxs)
        {
            au.source = gameObject.AddComponent<AudioSource>();
            au.source.clip = au.clip;

            au.source.volume = au.volume;
            au.source.pitch = au.pitch;
        }
    }
    
    public void Play(string name)
    {
        Audio au = Array.Find(soundFxs, audio => audio.name == name);
        au.source.Play();
    }

}
