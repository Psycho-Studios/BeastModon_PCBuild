using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class DialogueHeirarchyLink : MonoBehaviour
{
    private int currentSceneBuildIndex, _sceneCurrentlyLoaded, currentDialoguePlayerIndex;

    protected int positionInHeirarchy;

    private List<GameObject> _childDialoguePlayers = new List<GameObject>();
    
    

    private void Awake()
    {
        currentDialoguePlayerIndex = 0;
        bool firstDialoguePlayerInitialized = false;
        _sceneCurrentlyLoaded = SceneManager.GetActiveScene().buildIndex;
        
        if (SceneManager.GetActiveScene().buildIndex < 19)
        {
            
            foreach (GameObject childObject in GetComponentsInChildren<GameObject>())
            {
                _childDialoguePlayers.Add(childObject); //Obtain all child objects
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
                        break;
                    }
                }
            }

        }
        
    }

    protected void setNextDialoguePlayer_Active()
    {
        if (!_childDialoguePlayers[++currentDialoguePlayerIndex].name.StartsWith(_sceneCurrentlyLoaded.ToString()))
        {
            _childDialoguePlayers[currentDialoguePlayerIndex].SetActive(true);
        }

        _childDialoguePlayers[currentDialoguePlayerIndex - 1].SetActive(false);
    }

}
