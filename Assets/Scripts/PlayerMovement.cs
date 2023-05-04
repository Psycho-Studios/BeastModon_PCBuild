using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private bool bool_speedChangeInProgress;
    public static bool bool_level_11;
    private float float_clampFactor;
    public static float float_shipSpeed;
    public float float_xMin, float_xMax, float_yMin, float_yMax;
    public float[] zoomStartTimes, zoomLevels;
    private Animator animator_player1Ship;
    private Camera sceneCamera;
    private Rigidbody2D rigidbody2D_playerShip;

    private void Awake()
    {
        rigidbody2D_playerShip = this.gameObject.GetComponent<Rigidbody2D>();
        animator_player1Ship = this.gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        float_shipSpeed = 11;
        if (bool_level_11)
        {
            //Animate camera here
        }
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        
        switch(buildIndex) //Depending on the level, the camera will be farther out, and the player movement is contingent upon that
        {
            //Cramped levels
            case 0:
            {
                float_xMin = -32.57f;
                float_xMax = -9.61f;
                float_yMin = -1.54f;
                float_yMax = 8.5f;
                break;
            }
            //Spacious levels
            case 2:
            {
                float_xMin = -39.0f;
                float_xMax = -8.0f;
                float_yMin = -5.0f;
                float_yMax = 9.0f;
                break;
            }
            
            default:
            {
                break;
            }
            //New mode?
        }
    }

    private void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetKeyDown(ProjectileControls_Player1.keyCode_changeSpeed)
        && !bool_speedChangeInProgress)
        {
            changeSpeed();
        }
#endif
    }

    //This method handles movement in this script.
    void FixedUpdate()
    {
        //These tell the computer to represent input via numbers, gives said numbers variables
        float input_horizontal = Input.GetAxis("Horizontal");  
        float input_vertical = Input.GetAxis("Vertical");

        //Make those numbers into a movement by using this code
        Vector3 vector3_movementDifference = new Vector3(
            input_horizontal,
             input_vertical,
              0.0f);
        // Then, access Rigidbody's velocity component (already there, predefined and all), give it an adjustable speed
        rigidbody2D_playerShip.velocity = vector3_movementDifference * float_shipSpeed;
        
        if (Input.GetAxis("Vertical") > 0
        || Input.GetKey(ProjectileControls_Player1.keyCode_moveUp))
        {
            animator_player1Ship.SetInteger("State_Player1Ship", 1);
        }
        else if (Input.GetAxis("Vertical") < 0
        || Input.GetKey(ProjectileControls_Player1.keyCode_moveDown))
        {
            animator_player1Ship.SetInteger("State_Player1Ship", -1);
        }
        else if (Input.GetAxis("Vertical") == 0)
        {
            animator_player1Ship.SetInteger("State_Player1Ship", 0);
        }


        if (!bool_level_11)
        {
            //Set how far in the screen your character can move
            rigidbody2D_playerShip.position = new Vector3(
                Mathf.Clamp(
                    rigidbody2D_playerShip.position.x,
                    float_xMin,
                    float_xMax), 
                Mathf.Clamp(rigidbody2D_playerShip.position.y,
                    float_yMin,
                    float_yMax),
                0.0f);
        }
        else if (bool_level_11)
        {
            //Set how far in the screen your character can move
            rigidbody2D_playerShip.position = new Vector3(
                Mathf.Clamp(rigidbody2D_playerShip.position.x,
                    float_xMin + float_clampFactor,
                    float_xMax - float_clampFactor), 
                Mathf.Clamp(rigidbody2D_playerShip.position.y,
                    float_yMin + (float_clampFactor / 5.0f),
                    float_yMax - (float_clampFactor * 0.78f)),
                0.0f);
        }
    }

    private void changeSpeed()
    {
        bool_speedChangeInProgress = true;
        float_shipSpeed += 5;
        if (float_shipSpeed == 26)
        {
            float_shipSpeed = 6;
        }
        bool_speedChangeInProgress = false;
    }
}
