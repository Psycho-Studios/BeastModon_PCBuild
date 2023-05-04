using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayOnEnable : MonoBehaviour
{
    private AudioSource audioSource_speaker;
    public AudioClip audioClip_secondaryAudio; //Write logic to make this usable under a specific condition
    
    private void Awake()
    {
        this.audioSource_speaker = this.gameObject.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        this.audioSource_speaker.Play();
    }
}
