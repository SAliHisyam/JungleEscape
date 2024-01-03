using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class musicvolume : MonoBehaviour
{
    public AudioMixer _audio;
    

    public void SetVolumeMusic(float vol)
    {
        _audio.SetFloat("Music", vol);
    }
   

}