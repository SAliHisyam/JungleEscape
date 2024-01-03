using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class suaravolume : MonoBehaviour
{
    public AudioMixer _audio;


    public void SetVolumeSFX(float vol)
    {
        _audio.SetFloat("SFX", vol);
    }


}