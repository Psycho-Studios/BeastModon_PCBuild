using System.Collections;
using System.Collections.Generic;
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
    private bool bool_dialogueOccuring,
        bool_dialogueIsPaused,
        bool_dialogueStarted;

    public float float_secondsUntilDialogue = 0; //Edit if dialogue happens later than the very beginning of a level

    /*
     * If using only a static variable, it cannot be edited in the inspector, only referenced from other scripts.
     * I'm not hard coding times to change Sylvester's face, it will be faster to try them manually and report 
     * them properly.
     */
    public float[] _timesToChangeFace; //Inspector values
    public static float[] timesToChangeFace; //Allows easy reference script-wise


    public int[] _faceExpressions;
    public static int[] faceExpressions;

    public int _buildIndexToPlaySoundsAt;
    public static int buildIndexToPlaySoundsAt;

    public AudioClip audioClip_dialogue;

    private AudioSource audioSource_parent;

    private void Awake()
    {
        buildIndexToPlaySoundsAt = _buildIndexToPlaySoundsAt;
        bool_dialogueStarted = false;
        if (SceneManager.GetActiveScene().buildIndex == _buildIndexToPlaySoundsAt)
        {
            audioSource_parent = this.gameObject.GetComponentInParent<AudioSource>();
        }
    }

    private void OnEnable()
    {   
        audioSource_parent.clip = audioClip_dialogue;

        timesToChangeFace = _timesToChangeFace;
        faceExpressions = _faceExpressions;

        FaceAnimation_Player1.expressionIndex = 0;

        FaceAnimation_Player1.bool_safeToContinueTalking = true;

    }

    void Start()
    {
        bool_dialogueOccuring = false;
        bool_dialogueIsPaused = false;
    }

    private void Update()
    {
        if (!bool_dialogueStarted //Makes this run once in this script's lifetime
        && float_secondsUntilDialogue >= 0)
        {
            bool_dialogueStarted = true;
            StartCoroutine(startConversation());
        }

        if (bool_dialogueOccuring)    //Pause conditions and object-deactivation
        {
            if (Health_Player1.bool_playerReactingToDamage
            && !bool_dialogueIsPaused)
            {
                bool_dialogueIsPaused = true;
            }

            if (bool_dialogueIsPaused
            && audioSource_parent.isPlaying)
            {
                audioSource_parent.Pause();
            }

            if (FaceAnimation_Player1.bool_safeToContinueTalking
            && bool_dialogueIsPaused)
            {
                bool_dialogueIsPaused = false;
            }

            if (!bool_dialogueIsPaused
            && !audioSource_parent.isPlaying
            && FaceAnimation_Player1.bool_safeToContinueTalking)
            {
                audioSource_parent.Play();
            }   

            if(audioSource_parent.time >= audioClip_dialogue.length) //Each audio clip file will have a back-and-forth conversation,
            {
                bool_dialogueOccuring = false;
                bool_dialogueIsPaused = false;
                bool_dialogueStarted = false;
                FaceAnimation_Player1.bool_safeToContinueTalking = false;
                setNextDialoguePlayer_Active(); //Loads the next dialoguePlayerObject in the heirarchy, disables this one;
            }
        }

        
    }

    private void levelAttempted() //Add a static field to the GameProperties object to determine this value; it will be saved per user file
    {

    }

    IEnumerator startConversation() //Begin audioPlayback
    {
        yield return new WaitForSeconds(float_secondsUntilDialogue);

        audioSource_parent.Play();

        bool_dialogueOccuring = true;
    }
}

