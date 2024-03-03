using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    new AudioSource audio;
    
    
    public void AudioInit()
    {
        audio = GetComponent<AudioSource>();
    }
    public void MakeSound(AudioClip sound,float volume)
    {
        audio.PlayOneShot(sound,volume);
    }
}
