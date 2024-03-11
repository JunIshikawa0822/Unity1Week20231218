using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgm;
    public AudioSource se;
    
    
    
    public void MakeSound(AudioClip sound,float volume)
    {
        se.PlayOneShot(sound,volume);
    }

    public void BGMSliderOnValueChange(float value)
    {
        bgm.volume = value;
    }

    public void SESliderOnValueChange(float value)
    {
        se.volume = value;
    }
}
