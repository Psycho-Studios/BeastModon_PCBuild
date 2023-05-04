using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionScript : MonoBehaviour
{
    private bool bool_updateStopper;
    private static bool bool_levelEnd;
    private Animator animator_levelEndObject;
    private SpriteRenderer spriteRenderer_gameObject;


    private void Awake()
    {
        spriteRenderer_gameObject = gameObject.GetComponent<SpriteRenderer>();
        animator_levelEndObject = gameObject.GetComponent<Animator>();
        animator_levelEndObject.SetBool("LevelEnd", false);
        
    }

    private void Start()
    {
        bool_levelEnd = false;
        bool_updateStopper = false;
    }

    public static void markLevelEnd_true()
    {
        bool_levelEnd = true;
    }

    private void Update()
    {
        if (!bool_updateStopper)
        {
            if (bool_levelEnd)
            {
                animator_levelEndObject.SetBool("LevelEnd", true);
                bool_updateStopper = true;
            }
        }
    }

    public void loadNextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}