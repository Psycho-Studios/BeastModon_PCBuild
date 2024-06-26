using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVisualEffect : MonoBehaviour
{
    //This script assumes an audioClip is assigned to this object's audioSource in the inspector
    private bool bool_readyToRecycleHealthEffect, bool_recyclingObject;
    public bool bool_soundAfterAnimationEnd;
    private AudioSource audioSource;
    public AudioClip audioClip_healthUpSound, audioClip_beastModeUpSound;
    private AudioClip audioClip_originalSound;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        bool_recyclingObject = false;
        audioSource = this.gameObject.GetComponent<AudioSource>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        if(audioSource != null)
        { 
            audioClip_originalSound = audioSource.clip;
        }
    }

    private void Update()
    {
        if(bool_readyToRecycleHealthEffect) //Object effect is from a powerup
        {
            if(!this.audioSource.isPlaying)
            {
                this.audioSource.clip = audioClip_originalSound;
                bool_readyToRecycleHealthEffect = false;
                this.gameObject.SetActive(false);
            }
        }
       
        else if(bool_recyclingObject
        && this.bool_soundAfterAnimationEnd) //Audio completes before object removal
        {
            spriteRenderer.enabled = false;
            bool_recyclingObject = false;
            spriteRenderer.enabled = true;
            this.gameObject.SetActive(false);
        }
        
        else if (bool_recyclingObject
        && !this.bool_soundAfterAnimationEnd) //Audio playback is interrupted
        {
            spriteRenderer.enabled = false;
            if (!audioSource.isPlaying)
            {
                bool_recyclingObject = false;
                spriteRenderer.enabled = true;
                this.gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        if (audioSource != null
        && this.gameObject.name.Contains("PowerUp_HealthUp_VisualEffect")) 
        {
            audioSource.PlayOneShot(audioSource.clip);
            return;
        }
        else if (audioSource != null)
        {
            if(audioSource.clip != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }

    public void recycleGameObject()
    { 
        if (this.gameObject.name.Contains("PowerUp")
        && this.gameObject.name.Contains("VisualEffect"))
        {
            bool_readyToRecycleHealthEffect = true;
            return;
        }
        bool_recyclingObject = true;
    }

    /// <summary>
    /// Called from PowerUps.cs, updates a field in FaceAnimation_Player1.cs to allow appropriate face animations
    /// Face/Background must update only if player isn't already at full health
    /// </summary>
    /// <param name="healthBeforeHealing"></param>
    public void determineHealingSound(int healthBeforeHealing)
    {
        audioClip_originalSound = audioSource.clip;

        switch (GameProperties.DataManagement.GameData.string_currentDifficulty)
        {
            case "Easy":
            {
                if (healthBeforeHealing == 150)
                {
                    audioSource.clip = audioClip_beastModeUpSound;
                }
                else
                {
                    FaceAnimation_Player1.bool_healthPickupInProgress = true;
                }
                break;
            }
            case "Normal":
            {
                if (healthBeforeHealing == 100)
                {
                    audioSource.clip = audioClip_beastModeUpSound;
                }
                else
                {
                    FaceAnimation_Player1.bool_healthPickupInProgress = true;
                }
                break;
            }
            case "Arcade":
            {
                if (healthBeforeHealing == 50)
                {
                    audioSource.clip = audioClip_beastModeUpSound;
                }
                else
                {
                    FaceAnimation_Player1.bool_healthPickupInProgress = true;
                }
                break;
            }
            default:
            {
                if (healthBeforeHealing == 150)
                {
                    audioSource.clip = audioClip_beastModeUpSound;
                }
                else
                {
                    FaceAnimation_Player1.bool_healthPickupInProgress = true;
                }
                break;
            }
        }
    }
}
