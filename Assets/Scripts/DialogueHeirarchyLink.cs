using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueHeirarchyLink : MonoBehaviour
{
    protected bool bool_endOfAudioPlayers;

    private int currentSceneBuildIndex, _sceneCurrentlyLoaded, currentDialoguePlayerIndex;

    protected int positionInHeirarchy;

    private List<GameObject> _childDialoguePlayers = new List<GameObject>();
    
    

    private void Awake()
    {
        currentDialoguePlayerIndex = 0;
        bool firstDialoguePlayerInitialized = false;
        bool_endOfAudioPlayers = false;
        _sceneCurrentlyLoaded = SceneManager.GetActiveScene().buildIndex;
        
        if (SceneManager.GetActiveScene().buildIndex < 19)
        {
            
            foreach (DialoguePlayer childObject in GetComponentsInChildren<DialoguePlayer>())
            {
                _childDialoguePlayers.Add(childObject.gameObject); //Obtain all child objects
            }
            
            _childDialoguePlayers = _childDialoguePlayers.OrderBy(childObject => childObject.name).ToList(); //Sort alphabetically

            for (int i = 0; i < _childDialoguePlayers.Count; i++)
            {
                if (!_childDialoguePlayers[i].name.StartsWith(_sceneCurrentlyLoaded.ToString()))
                {
                    _childDialoguePlayers[i].SetActive(false); //Deactivate dialoguePlayers for other levels
                }
                else //Runs if the build index indeed matches the beginning of the dialoguePlayer object name
                {
                    if (!firstDialoguePlayerInitialized)
                    {
                        _childDialoguePlayers[i].SetActive(true);
                        firstDialoguePlayerInitialized = true;
                    }
                    else
                    {
                        _childDialoguePlayers[i].SetActive(false);
                    }
                }
            }

        }
        
    }

    //IMPORTANT! 
    protected void setNextDialoguePlayer_Active()
    {
        try
        {

            if (_childDialoguePlayers.Count > (currentDialoguePlayerIndex + 1))
            {
                if (_childDialoguePlayers[currentDialoguePlayerIndex + 1].name.StartsWith(_sceneCurrentlyLoaded.ToString()))
                {
                    currentDialoguePlayerIndex++;

                    _childDialoguePlayers[currentDialoguePlayerIndex].SetActive(true);

                    
                    //if (FaceAnimation_Player1.expressionIndex > 0) //Used for DialoguePlayer scripts to reset the position of audio playback
                    //{
                    //    FaceAnimation_Player1.expressionIndex = 0;
                    //}

 
                    if ((currentDialoguePlayerIndex - 1) >= 0) //Subtracting because the index is incremented by this point
                    {
                        _childDialoguePlayers[currentDialoguePlayerIndex - 1].SetActive(false);
                    }

                    //else
                    //{
                    //    _childDialoguePlayers[currentDialoguePlayerIndex].SetActive(false);
                    //}
                }
            }
            //else if(_childDialoguePlayers.Count > 0)
            //{

            //    _childDialoguePlayers[currentDialoguePlayerIndex].SetActive(false);
            //}
            else
            {
                return;
            }
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

}
