using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class volumesettings : MonoBehaviour
{
    public AudioMixer _audio;

    public void SetVolume(float vol)
    {
        _audio.SetFloat("Music", vol);
    }
   
    
}  
