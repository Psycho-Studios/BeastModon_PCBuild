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
        bool_playerTookDamage;

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

        if(SceneManager.GetActiveScene().buildIndex == _buildIndexToPlaySoundsAt)
        {
            audioSource_parent = this.gameObject.GetComponentInParent<AudioSource>();
            audioSource_parent.clip = audioClip_dialogue;
        }

        timesToChangeFace = _timesToChangeFace;
        faceExpressions = _faceExpressions;
    }

    void Start()
    {
        bool_dialogueOccuring = false;
        bool_dialogueIsPaused = false;
    }

    private void Update()
    {
        if(Health_Player1.bool_playerReactingToDamage
        && !bool_dialogueIsPaused)
        {
            bool_playerTookDamage = true;
            bool_dialogueIsPaused = true;
        }
       
        if(bool_dialogueIsPaused
        && audioSource_parent.isPlaying)
        {
            audioSource_parent.Pause();
        }

        if(!Health_Player1.bool_playerReactingToDamage
        && bool_dialogueIsPaused)
        {
            bool_dialogueIsPaused = false;
        }

        if(!bool_dialogueIsPaused
        && !audioSource_parent.isPlaying
        && bool_playerTookDamage)
        {
            bool_playerTookDamage = false;
            audioSource_parent.Play();
        }

        if (bool_dialogueOccuring)
        {
            if(audioSource_parent.time >= audioClip_dialogue.length)
            {
                bool_dialogueOccuring = false;
                bool_dialogueIsPaused = false;
                bool_playerTookDamage = false;
                setNextDialoguePlayer_Active(); //Loads the next dialoguePlayerObject in the heirarchy, disables this one;
            }
        }

        /*
        If float_timesToChangeFaceExpressions has at least one value, this index will represent Sylvester's face expression
        in correspondence to it (doesn't affect sound)
        */
            StartCoroutine(startConversation());
    }

    private void levelAttempted() //Add a static field to the GameProperties object to determine this value; it will be saved per user file
    {

    }


    IEnumerator startConversation() //Begin audioPlayback
    {
        yield return new WaitForSeconds(float_secondsUntilDialogue);
        //Debug.Log("Dialogue_${positionInHeirarchy} beginning.    positionInHeirarchy is a variable you should create to hold this value.
        //if(dialogueEnabled){Enclose below logic here}

        audioSource_parent.Play();
        bool_dialogueOccuring = true;
    }
}

