using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Sets rules for the game based on settings. Also handles pause game functionality.
/// </summary>
public class GameProperties : MonoBehaviour
{

    private bool[] phaseShiftLevel = { false, false, true, false, true, true, false, true, false, true, false, false, true };
    public float[] levelEndTimes;

    private int sceneIndex;

    /// <summary>
    /// Contains methods for saving, loading, and configurations. Uses nested GameData class for said operations.
    /// </summary>
    public static class DataManagement
    {
        public static int int_fileCurrentlyLoaded;

        public static GameData gameData_player1;

        public static void saveGame(int fileNumber)
        {

        }
        
        public static void loadGame(int fileNumber)
        {

        }

        /// <summary>
        /// Serializable data for the player, specific to each file
        /// </summary>
        [System.Serializable]
        public class GameData
        {
            public static string string_currentDifficulty = "Easy"; //Easy, Normal, Arcade
        }
    }

    /// <summary>
    /// Represents whether the game is paused or not.
    /// </summary>
	public static bool bool_isGamePaused;
   
    void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.SetResolution(1920, 1080, true, 60);
        DataManagement.int_fileCurrentlyLoaded = -1; //Default for when the main menu opens, is updated on level load
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
	void Start()
	{


        bool_isGamePaused = false;
        if (sceneIndex == 12)
        {
            PlayerMovement.bool_level_11 = true;
        }
        else
        {
            PlayerMovement.bool_level_11 = false;
        }
    }
	void Update () 
	{
#if UNITY_STANDALONE
        if(Input.GetKeyDown(ProjectileControls_Player1.keyCode_pauseGame))
        {
            pauseGame();
        }
#endif
        if(sceneIndex != 0
        && sceneIndex < 13)
        {
            if(Time.time >= levelEndTimes[sceneIndex]
            && phaseShiftLevel[sceneIndex])
            {

            }

            else if(Time.time >= levelEndTimes[sceneIndex])
            {
                LevelTransitionScript.markLevelEnd_true();
            }
        }
    }

    /// <summary>
    /// Called when there's a boss fight
    /// </summary>
    private void phaseShift()
    {
        //Do contextual stuff here, like change a song or something
    }


    /// <summary>
    /// Changes the game speed, mutes all audio, updates bool_isPaused. 
    /// </summary>
    private void pauseGame()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            switch (bool_isGamePaused)
            {
                case true:
                {

                    Time.timeScale = 1;
                    AudioListener.pause = false;
                    bool_isGamePaused = false;

                    break;
                }
                case false:
                {
                    Time.timeScale = 0;
                    AudioListener.pause = true;
                    bool_isGamePaused = true;

                    break;
                }
            }
        }
    }
}
