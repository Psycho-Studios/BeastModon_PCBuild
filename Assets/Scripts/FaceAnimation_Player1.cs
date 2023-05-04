using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animates player1 face as well as its corresponding background
/// </summary>
public class FaceAnimation_Player1 : MonoBehaviour
{
    public static bool bool_takingDamage;
    public static bool bool_player1Dead;
    public static bool bool_safeToContinueTalking;

    //Calculated in randomizeDamageSound
    private int int_damageSoundIndex, _expressionIndex;
    public static int int_currentAnimationState;

    //Helpful for reporting player1's face animations to other scripts
    public static Animator animator_player1Face;

    public AudioClip audioClip_player1Death;
    public AudioClip[] audioClip_Damage;
    private AudioSource audioSource_player1Face;
    private SpriteRenderer spriteRenderer_player1Face;
    private System.Random random;

    private void Awake()
    {
        animator_player1Face = this.gameObject.GetComponent<Animator>();
        audioSource_player1Face = this.gameObject.GetComponent<AudioSource>();
        spriteRenderer_player1Face = this.gameObject.GetComponent<SpriteRenderer>();
        this.random = new System.Random();
    }

    private void Start()
    {
        bool_player1Dead = false;
        bool_safeToContinueTalking = true;

        //if(dialogueEnabled) {_expressionIndex = 0}
        StartCoroutine(dialogueAnimation());
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
        {
            switch (GameProperties.DataManagement.GameData.string_currentDifficulty)
            {
                case "Easy":
                {
                    if (Health_Player1.int_life >= 100)
                    {
                        FaceAnimation_Player1.animator_player1Face.SetInteger("Expression", 0);
                    }
                    else
                    {
                        FaceAnimation_Player1.animator_player1Face.SetInteger("Expression", 1);
                    }
                    break;
                }
                case "Normal":
                {
                    if (Health_Player1.int_life == 100)
                    {
                        FaceAnimation_Player1.animator_player1Face.SetInteger("Expression", 0);
                    }
                    else
                    {
                        FaceAnimation_Player1.animator_player1Face.SetInteger("Expression", 1);
                    }
                    break;
                }
                case "Arcade":
                {
                    FaceAnimation_Player1.animator_player1Face.SetInteger("Expression", 0);
                    break;
                }
                default:
                {
                    if (Health_Player1.int_life >= 100)
                    {
                        FaceAnimation_Player1.animator_player1Face.SetInteger("Expression", 0);
                    }
                    else
                    {
                        FaceAnimation_Player1.animator_player1Face.SetInteger("Expression", 1);
                    }
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Plays ship damage sounds, determines next face animation to play.
    /// </summary>
    public void executeDamageFeedback()
    {
        if (!Health_Player1.bool_playerReactingToDamage)
        {
            Health_Player1.bool_playerReactingToDamage = true;
            bool_safeToContinueTalking = false;
            
            audioSource_player1Face.PlayOneShot(audioClip_Damage[this.random.Next(0, 6)]);
            switch (int_currentAnimationState)
            {
            case 0: //Idle animation last being played
                {
                    if (Health_Player1.int_life != 50)
                    {
                        /*Anything less than 100 means danger, and DamageIdle animation has the same integer as Damage.
                        In Arcade mode the player starts at 50, meaning they die without warning when they take damage.
                        No need to add a use case for 0 int_life because the SpriteRenderer is disabled at that point.*/
                        animator_player1Face.SetInteger("Expression", int_currentAnimationState);
                    }
                    else
                    {
                        StatusScript_Player1.animator_faceBackground.SetInteger("Status", 1); //Damage Status
                    }
                    break;
                }
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

    IEnumerator dialogueAnimation()
    {
        if (_expressionIndex >= 0) //-1 will imply there are no faces to make
        {
            //Remember, expressions can change multiple times during the playback of any audioClip
            yield return new WaitForSeconds(DialoguePlayer.timesToChangeFace[_expressionIndex]);
            animator_player1Face.SetInteger("Expression", DialoguePlayer.faceExpressions[_expressionIndex]);
            _expressionIndex++;
            if(DialoguePlayer.timesToChangeFace.Length == _expressionIndex) //End of the dialogue player child-object's dialogue
            {
                //Switch to the next dialogueBeingPlayed
            }
        }
    }
}
