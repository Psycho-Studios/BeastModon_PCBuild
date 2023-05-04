using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TraversalSounds : MonoBehaviour
{
    private AudioSource audioSource_gameObject;
    public AudioClip audioClip_traversalSound1, audioClip_traversalSound2, audioClip_traversalSound3;

    private void Awake()
    {
        audioSource_gameObject = this.gameObject.GetComponent<AudioSource>();

    }

    //Use these methods for animation events
    public void makeTraversalSound1() 
    {
        if (this.gameObject.activeInHierarchy)
        {
            audioSource_gameObject.PlayOneShot(audioClip_traversalSound1);
        }
        
    }

    public void makeTraversalSound2() 
    {
        if (this.gameObject.activeInHierarchy)
        {
            audioSource_gameObject.PlayOneShot(audioClip_traversalSound2);
        }
    }

    public void makeTraversalSound3()
    {
        if (this.gameObject.activeInHierarchy)
        {
            audioSource_gameObject.PlayOneShot(audioClip_traversalSound3);
        }
    }
}
