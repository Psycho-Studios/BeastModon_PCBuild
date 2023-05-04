using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{

    private AudioSource audioSource_musicPlayer;
    public AudioClip[] audioClips_soundtrack;

    private void Awake()
    {
        audioSource_musicPlayer = this.gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        //The song playing directly corresponds to the scene numbered in the Build Settings
        audioSource_musicPlayer.PlayOneShot(audioClips_soundtrack[SceneManager.GetActiveScene().buildIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
