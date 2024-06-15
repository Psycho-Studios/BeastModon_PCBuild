using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Animates player1 face as well as its corresponding background
/// </summary>
public class FaceAnimation_Player1 : MonoBehaviour
{ 
    private bool bool_developmentMode;

    /// <summary>
    /// Is set when dialogue has its animation duration updated. Good for keeping logic in 
    /// dialogueAnimation() from running.
    /// </summary>
    private bool bool_dialogueAnimationDurationRefresh;

    /// <summary>
    /// Is true if the first conversation is hasn't began yet.
    /// </summary>
    private bool bool_waitingForFirstConversation;

    /// <summary>
    /// Reports updates to the current dialogue animation's duration to keep in sync with audio clip.
    /// Will return true while Damage, BeastMode or LetsGo animation is happening.
    /// Is set false when powerup animation has ended.
    /// </summary>
    public static bool bool_dialogueAnimationTimeExternallyUpdated;

    public static bool bool_player1Dead, bool_takingDamage, bool_healthPickupInProgress;

    /// <summary>
    /// Is true if damage reception is over and if Beast Mode is no longer active 
    /// </summary>
    public static bool bool_safeToContinueDialogueAnimation;
    
    /// <summary>
    /// Returns true if BeastMode, LetsGo, or Damage is currently initiated
    /// </summary>
    public static bool bool_dialogueAudioInterrupted;

    public static bool bool_letsGo_FaceCooldown, bool_beastMode_FaceCooldown, bool_damage_FaceCooldown;
    
    private int int_damageSoundIndex, _expressionIndex;
    
    /// <summary>
    /// When this field updates, dialogueAnimation() runs again. Useful for  
    /// </summary>
    public static int expressionIndex;

    /// <summary>
    /// Helpful for reporting player1's original face animations to other scripts before being interrupted, like DialoguePlayer when receiving damage
    /// </summary>
    public static int int_currentAnimationState; 

    public static float float_timePlayer1HaltedAnimation;

    private AudioSource audioSource_player1Face;
    private SpriteRenderer spriteRenderer_player1Face;
    private System.Random random;
    public AudioClip audioClip_player1Death;
    public AudioClip[] audioClip_Damage;

    /// <summary>
    /// Helpful for reporting player1's face animations to other scripts, like DialoguePlayer when receiving damage
    /// </summary>
    public static Animator animator_player1Face;

    private void Awake()
    {
        enableDevelopmentMode();
        animator_player1Face = this.gameObject.GetComponent<Animator>();
        audioSource_player1Face = this.gameObject.GetComponent<AudioSource>();
        spriteRenderer_player1Face = this.gameObject.GetComponent<SpriteRenderer>();
        _expressionIndex = 0;
        expressionIndex = 0;
        bool_dialogueAudioInterrupted = false;
        bool_dialogueAnimationTimeExternallyUpdated = false;
        this.random = new System.Random();
    }

    private void Start()
    {
        bool_player1Dead = false;
        bool_beastMode_FaceCooldown = false;
        bool_damage_FaceCooldown = false;
        bool_letsGo_FaceCooldown = false;
        bool_dialogueAnimationDurationRefresh = false;
        bool_waitingForFirstConversation = true;

        enableDevelopmentMode();

        StartCoroutine(dialogueAnimation());
    }

    private void Update()
    {
        if (_expressionIndex != expressionIndex) 
        {
            _expressionIndex = expressionIndex;

            StartCoroutine(dialogueAnimation());
        }
    }

    /// <summary>
    /// Calls the coroutine that slows player1 firing speed. Useful for calling from an animation event.
    /// </summary>
    public void restoreOriginalFiringSpeed()
    {
        StartCoroutine(triggerFiringSpeedEnd());
    }

    IEnumerator triggerFiringSpeedEnd()
    {
        yield return new WaitForSeconds(5);
        ProjectileControls_Player1.bool_fastFiringActive = false;
        if(!bool_dialogueAudioInterrupted
        && !DialoguePlayer.bool_dialogueInProgress)
        {
            switch (GameProperties.DataManagement.GameData.string_currentDifficulty)
            {
                case "Easy":
                {
                    if (Health_Player1.int_life >= 100)
                    {
                        animator_player1Face.SetInteger("Expression", 0);
                    }
                    else
                    {
                        animator_player1Face.SetInteger("Expression", 1);
                    }
                    break;
                }
                case "Normal":
                {
                    if (Health_Player1.int_life == 100)
                    {
                        animator_player1Face.SetInteger("Expression", 0);
                    }
                    else
                    {
                        animator_player1Face.SetInteger("Expression", 1);
                    }
                    break;
                }
                case "Arcade":
                {
                    animator_player1Face.SetInteger("Expression", 0);
                    break;
                }
                default:
                {
                    if (Health_Player1.int_life >= 100)
                    {
                        animator_player1Face.SetInteger("Expression", 0);
                    }
                    else
                    {
                        animator_player1Face.SetInteger("Expression", 1);
                    }
                    break;
                }
            }

            bool_letsGo_FaceCooldown = false;
        }
    }

    /// <summary>
    /// Plays ship damage sounds, determines next face animation to play.
    /// Called at the beginning of damage animation via keyframe.
    /// </summary>
    public void executeDamageFeedback()
    {
        if (!Health_Player1.bool_playerReactingToDamage)
        {
            Health_Player1.bool_playerReactingToDamage = true;
            bool_safeToContinueDialogueAnimation = false;

            audioSource_player1Face.PlayOneShot(audioClip_Damage[this.random.Next(0, 6)]);

            if (Health_Player1.int_life == 50) //This will only run if Arcade mode is not selected
            {
                StatusScript_Player1.animator_faceBackground.SetInteger("Status", (int)E_StatusAnimationStates.Damage); //Damage Status
            }
        }
                
    }

    /// <summary>
    /// Called from SylvesterDeath animation when int_life is 0
    /// </summary>
    public void removeFace()
    {
        spriteRenderer_player1Face.enabled = false;
        audioSource_player1Face.PlayOneShot(audioClip_player1Death);
    }

    /// <summary>
    /// Face changes with this method, NOT audio.
    /// Runs in Start() method.
    /// Also runs in Update() method if expressionIndex is altered.
    /// Set expressionIndex to -1 to keep this method from running.
    /// The expressionIndex is incremented by 1 in the DialoguePlayer script.
    /// </summary>
    /// <returns></returns>
    public IEnumerator dialogueAnimation() 
    {
        if(bool_waitingForFirstConversation)
        {
            yield return new WaitForSeconds(DialoguePlayer.float_reportedSecondsUntilFirstConversation);
            bool_waitingForFirstConversation = false;
        }


        if (_expressionIndex >= 0
        && DialoguePlayer.faceExpressions[_expressionIndex] != -1) 
        {
            if (!bool_dialogueAnimationDurationRefresh)
            {
                while (!bool_safeToContinueDialogueAnimation) //Player is in a state of damage, Beast Mode, or LetsGo
                {
                    yield return null;
                }

                if (Health_Player1.int_life == 50
                    && (bool_developmentMode
                        || !GameProperties.DataManagement.GameData.string_currentDifficulty.Contains("Arcade"))) //Default difficulty is Easy
                {
                    FaceAnimation_Player1.animator_player1Face.SetInteger("Expression", DialoguePlayer.faceExpressions[_expressionIndex]);
                }

                yield return new WaitForSeconds(DialoguePlayer.faceExpressionDurations[_expressionIndex]); //Wait for the duration of the audio clip

            }

            bool_dialogueAnimationDurationRefresh = false;

            //If there wasn't a dialogue interruption... --OK
            if (!bool_dialogueAnimationTimeExternallyUpdated) 
            {
                ///If dialogue clips are exhausted, set face to corresponding health status
                if (_expressionIndex >= DialoguePlayer.faceExpressions.Length)
                {
                    if (!bool_dialogueAudioInterrupted)
                    {
                        animateFaceAfterEndOfDialogue();
                    }
                }
                
                ///Otherwise set face to damage if player is at 50 life and not in Arcade mode
                else if (Health_Player1.int_life == 50
                    && (bool_developmentMode 
                        || !GameProperties.DataManagement.GameData.string_currentDifficulty.Contains("Arcade"))) //Default difficulty is Easy
                {
                    animator_player1Face.SetInteger("Expression", (int)E_FaceExpressions.Damage);
                }
                
                ///If life isn't equal to 50, player1 cannot possibly be in danger due to lower difficulty
                ///level or death. Because death means a deactivated face sprite, it's safe to animate
                ///regardless. 
                else
                {
                    animator_player1Face.SetInteger("Expression", DialoguePlayer.faceExpressions[_expressionIndex]);
                }
            }

            //If there was a dialogue interruption...
            else if (bool_dialogueAnimationTimeExternallyUpdated)
            {
                //No overtaking animations occuring, cooldowns are over
                if (bool_safeToContinueDialogueAnimation) 
                {
                    if(Health_Player1.int_life == 50
                    && (bool_developmentMode
                        || !GameProperties.DataManagement.GameData.string_currentDifficulty.Contains("Arcade"))) //Default difficulty is Easy
                    {
                        animator_player1Face.SetInteger("Expression", (int)E_FaceExpressions.Damage);
                    }
                    
                    else
                    {
                        animator_player1Face.SetInteger("Expression", DialoguePlayer.faceExpressions[_expressionIndex]);
                        bool_dialogueAnimationTimeExternallyUpdated = false;
                        bool_dialogueAnimationDurationRefresh = true;
                    }
                    
                    StartCoroutine(dialogueAnimation());
                }
            }
        }     
    }

    /// <summary>
    /// Once the current dialogue object exhausts its audio clip, this is called, and the face is sent to initial expression.
    /// </summary>
    private void animateFaceAfterEndOfDialogue()
    {
        if (bool_developmentMode)
        {
            if (HUD_Player1.bool_beastModeRequest_player1
                || ProjectileControls_Player1.bool_beastModeActive)
            {
                animator_player1Face.SetInteger("Expression", (int)E_FaceExpressions.BeastMode);
            }
            else if (Health_Player1.int_life >= 100)
            {
                animator_player1Face.SetInteger("Expression", (int)E_FaceExpressions.Idle);
            }
            else
            {
                animator_player1Face.SetInteger("Expression", (int)E_FaceExpressions.Damage); 
            }
        }
        else
        {
            switch (Health_Player1.int_life)
            {
                case 50:
                {
                    if (HUD_Player1.bool_beastModeRequest_player1
                    || ProjectileControls_Player1.bool_beastModeActive) //Beast Mode active during conversation
                    {
                        animator_player1Face.SetInteger("Expression", (int)E_FaceExpressions.BeastMode);
                    }
                    else if (!GameProperties.DataManagement.GameData.string_currentDifficulty.Contains("Arcade"))
                    {
                        animator_player1Face.SetInteger("Expression", (int)E_FaceExpressions.Damage); //Any other difficulty means danger at 50 life
                    }
                    else
                    {
                        animator_player1Face.SetInteger("Expression", (int)E_FaceExpressions.Idle); //Difficulty is Arcade, so no danger at 50 life
                    }
                    break;
                }
                case 100:
                case 150:
                {
                    if (HUD_Player1.bool_beastModeRequest_player1
                    || ProjectileControls_Player1.bool_beastModeActive)
                    {
                        animator_player1Face.SetInteger("Expression", (int)E_FaceExpressions.BeastMode);
                    }
                    else
                    {
                        animator_player1Face.SetInteger("Expression", (int)E_FaceExpressions.Idle);
                    }

                    break;
                }
            }
        }
    }    

    /// <summary>
    /// Sets a flag that implies development mode is active.
    /// </summary>
    private void enableDevelopmentMode()
    {
        Debug.Log("Development mode enabled. Be sure to update the commented Arcade difficulty logic above.");
        bool_developmentMode = true;
    }
}
