using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public AudioClip BackgroundSound;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.clip = BackgroundSound;
        audioSource.Play();
    }
}