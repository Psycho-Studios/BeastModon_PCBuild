using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// The parent gameObject will house the AudioSource. This script must be attached to every child
/// object of the DialoguePlayer. The build index is used to seperate the levels' respective audio
/// tracks. Enter this value in the inspector to specify the level this child gameObject will exist
/// in.
/// </summary>
public class DialoguePlayer : DialogueHeirarchyLink
{
    /// <summary>
    /// If this field is ever true (new dialogue clip being loaded, player paused the game), dialogue playback halts to allow proper operation.
    /// </summary>
    private bool bool_dialogueIsPaused;

    /// <summary>
    /// Dialogue has started for the current child_DialoguePlayer object.
    /// </summary>
    private bool bool_firstConversationTriggered;
    
    /// <summary>
    /// Player is struck by an enemy projectile. This is used to determine if the player's face should change.
    /// </summary>
    private bool bool_player1Damaged;

    /// <summary>
    /// If this field is true, a dialogue clip is currently loaded.
    /// </summary>
    public static bool bool_dialogueInProgress;

    public static bool player1Dead;

    /*
     * If using only a static variable, it cannot be edited in the inspector, only referenced from other scripts.
     * I'm not hard coding times to change Sylvester's face, it will be faster to try them manually and report 
     * them properly.
     */
    private float float_timeAnimationInterrupted, float_originalTimeFromCurrentDialogueIndex,
        float_timeDialogueSlotWasInitiallyPlayed;

    /// <summary>
    /// Updated when the player's dialogue is interrupted by a power up or enemy contact.
    /// </summary>
    private float float_timeLeftInCurrentDialogueAnimation;

    /// <summary>
    /// Edit if dialogue happens later than the very beginning of a level. Set to -1 to disable.
    /// </summary>
    public float float_secondsUntilFirstConversation = 0;

    /// <summary>
    /// Time until the first conversation is triggered. This is a static field to prevent multiple triggers.
    /// </summary>
    public static float float_reportedSecondsUntilFirstConversation = 0;
    
    public float[] _faceExpressionDurations; //Inspector values
    public static float[] faceExpressionDurations; //Allows easy reference script-wise

    public int[] _faceExpressions;
    public static int[] faceExpressions;

    public int _buildIndexToPlaySoundsAt;
    public static int buildIndexToPlaySoundsAt;

    /// <summary>
    /// Refers to the current position in the array of audioClips_characterResponses
    /// </summary>
    public static int dialogueArrayIndex;

    /// <summary>
    /// Dialogue clip that the parent gameObject will play
    /// </summary>
    public AudioClip audioClip_dialogue;

    public AudioClip[] audioClips_characterResponses;

    private AudioSource audioSource_parent;

    private void Awake()
    {
        buildIndexToPlaySoundsAt = _buildIndexToPlaySoundsAt;
        bool_dialogueInProgress = false;
        bool_dialogueIsPaused = false;
        bool_firstConversationTriggered = false;
        player1Dead = false;

        float_originalTimeFromCurrentDialogueIndex = 0.0f;
        float_timeDialogueSlotWasInitiallyPlayed = 0.0f;
        float_timeLeftInCurrentDialogueAnimation = 0.0f;
        float_timeAnimationInterrupted = 0.0f;
        float_reportedSecondsUntilFirstConversation = float_secondsUntilFirstConversation;



        if (SceneManager.GetActiveScene().buildIndex == _buildIndexToPlaySoundsAt)
        {
            audioSource_parent = this.gameObject.GetComponentInParent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        float_timeAnimationInterrupted = 0.0f;
        dialogueArrayIndex = 0;
        //Every time a gameObject is set active the first clip will be
        //the conversation starter
        audioSource_parent.clip = audioClips_characterResponses[dialogueArrayIndex];

        faceExpressionDurations = _faceExpressionDurations;
        faceExpressions = _faceExpressions;

        //FaceAnimation_Player1.expressionIndex = 0;

        if(!FaceAnimation_Player1.bool_dialogueAudioInterrupted 
        && !FaceAnimation_Player1.bool_player1Dead)
        {
            FaceAnimation_Player1.bool_safeToContinueDialogueAnimation = true;
        }
    }

    private void Update()
    {
        if (!bool_dialogueInProgress
        && !bool_firstConversationTriggered
        && float_secondsUntilFirstConversation >= 0)
        {
            bool_firstConversationTriggered = true;
            StartCoroutine(startConversation()); //Only called once in this script's lifetime
        }

        if (bool_firstConversationTriggered)
        {
            if (audioSource_parent.isPlaying) //Pause conditions and object-deactivation
            {
                if (FaceAnimation_Player1.bool_dialogueAudioInterrupted //Player1 cannot have dialogue while animation is playing
                && !bool_dialogueIsPaused)
                {
                    bool_dialogueIsPaused = true;
                    audioSource_parent.Pause();
                    float_timeAnimationInterrupted = Time.time;

                    //Subtract time player was struck from the sum of beginning and duration of audio clip for time left to continue playing
                    //Update the currently selected faceExpression's duration
                    float_timeLeftInCurrentDialogueAnimation = (
                        float_timeDialogueSlotWasInitiallyPlayed + float_originalTimeFromCurrentDialogueIndex) - float_timeAnimationInterrupted;
                    if(Health_Player1.int_life > 0)
                    {
                        faceExpressionDurations[FaceAnimation_Player1.expressionIndex] = float_timeLeftInCurrentDialogueAnimation;
                    }
                    FaceAnimation_Player1.bool_dialogueAnimationTimeExternallyUpdated = true;
                }
            }
            
            else if (bool_dialogueIsPaused
            && !audioSource_parent.isPlaying)
            {
                if (!player1Interrupted())//No more reacting to damage, dialogue can continue
                {
                    bool_dialogueIsPaused = false;
                    audioSource_parent.UnPause();
                }
            }

            else
            {
                if (bool_dialogueInProgress)
                {
                    if (GameProperties.bool_isGamePaused)
                    {
                        bool_dialogueInProgress = false;
                        audioSource_parent.Pause();
                    }

                    else if (audioSource_parent.time == 0) //Current audio clip has ended
                    {
                        bool_dialogueInProgress = false;
                        FaceAnimation_Player1.expressionIndex++; //Will trigger new dialogue animation, or keep the same damage animation

                        if (++dialogueArrayIndex < audioClips_characterResponses.Length) //Load next clip from current dialogue object
                        {
                            audioSource_parent.clip = audioClips_characterResponses[dialogueArrayIndex];
                            
                            //Reset dialogue variables
                            bool_dialogueInProgress = true;
                            float_timeDialogueSlotWasInitiallyPlayed = Time.time;
                            float_originalTimeFromCurrentDialogueIndex = faceExpressionDurations[FaceAnimation_Player1.expressionIndex];
                            
                            this.audioSource_parent.Play();
                        }
                        else //Is there another dialoguePlayer object to activate?
                        {
                            this.audioSource_parent.Stop();
                            setNextDialoguePlayer_Active(); //Loads the next dialoguePlayerObject in the heirarchy;
                            this.gameObject.SetActive(false); //This line will only run for the last piece of dialogue
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns whether animation is currently running due to enemy contact/power ups. If cooldowns are over, dialogue is allowed to continue.
    /// </summary>
    /// <returns></returns>
    private bool player1Interrupted()
    {
        if (!FaceAnimation_Player1.bool_beastMode_FaceCooldown //Read-only
            && !FaceAnimation_Player1.bool_damage_FaceCooldown //Read-only
            && !FaceAnimation_Player1.bool_letsGo_FaceCooldown //Read-only
            && !Health_Player1.bool_playerReactingToDamage
            && !player1Dead)
        {
            if (FaceAnimation_Player1.bool_dialogueAudioInterrupted) //Dialogue should continue if above conditions are met
            {
                FaceAnimation_Player1.bool_dialogueAudioInterrupted = false;
            }
            return false;
        }
        return true;
    }
    
    private void levelAttempted() //Add a static field to the GameProperties object to determine this value; it will be saved per user file
    {

    }
    
    IEnumerator startConversation() //Begin audioPlayback
    {
        yield return new WaitForSeconds(float_secondsUntilFirstConversation);
        if (player1Interrupted())
        {
            bool_dialogueIsPaused = true;
            if (!bool_firstConversationTriggered)
            {
                yield return new WaitForSeconds(float_secondsUntilFirstConversation - float_timeAnimationInterrupted);
                //The goal above is to finish the initial wait for the first dialogue clip to play.
            }
        }
        else
        {
            bool_dialogueInProgress = true;
            float_timeDialogueSlotWasInitiallyPlayed = Time.time;
            float_originalTimeFromCurrentDialogueIndex = faceExpressionDurations[FaceAnimation_Player1.expressionIndex];
            audioSource_parent.Play();
        }
    }
}