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
    public static int int_currentAnimationState, expressionIndex;

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
        _expressionIndex = 0;
        expressionIndex = _expressionIndex;
        this.random = new System.Random();
    }

    private void Start()
    {
        bool_player1Dead = false;

        //if(dialogueEnabled) {_expressionIndex = 0}
        StartCoroutine(dialogueAnimation());
    }

    private void Update()
    {
        if(_expressionIndex != expressionIndex) //Should fire after incrementExpressionIndex
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
                case 3: //Conversation animation
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

    IEnumerator dialogueAnimation() //Runs in this file's Start() method, will not run if expressionIndex is <= -1
    {
        if (_expressionIndex >= 0
        && DialoguePlayer.faceExpressions[expressionIndex] != -1) 
        {
            yield return new WaitForSeconds(DialoguePlayer.timesToChangeFace[_expressionIndex]);

            animator_player1Face.SetInteger("Expression", DialoguePlayer.faceExpressions[_expressionIndex]);
        }
        else
        {
            returnFaceToAppropriateAnimation();
        }
    }

    private void returnFaceToAppropriateAnimation() //Once the dialogue is over, this is called, and the face is sent to initial expression.
    {
        switch(Health_Player1.int_life)
        {
            case 50:
            {
                if (HUD_Player1.bool_beastModeRequest_player1
                || ProjectileControls_Player1.bool_beastModeActive)
                {
                    animator_player1Face.SetInteger("Expression", 2);
                }
                else
                {
                    animator_player1Face.SetInteger("Expression", 1);
                }
                
                break;
            }
            case 100:
            case 150:
            {
                if(HUD_Player1.bool_beastModeRequest_player1
                || ProjectileControls_Player1.bool_beastModeActive)
                {
                    animator_player1Face.SetInteger("Expression", 2);
                }
                else
                {
                    animator_player1Face.SetInteger("Expression", 0);
                }
                
                break;
            }
        }
    }

    public static void incrementExpressionIndex()
    {
        expressionIndex++;
    }

    
}
