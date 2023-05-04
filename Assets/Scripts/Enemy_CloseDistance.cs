using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_CloseDistance : MonoBehaviour
{
    private bool bool_distanceClosed, bool_distance_x_closed, bool_distance_y_closed;
    public float float_initialDistanceToClose_x, float_initialDistanceToClose_y, float_speed;
    private Vector3 vector3_originalPosition, vector3_translation;
    
    private void Awake()
    {
        vector3_translation = new Vector3(
            float_initialDistanceToClose_x,
            float_initialDistanceToClose_y,
            0);
    }

    private void Start()
    {
        float_speed *= 0.01f;
    }

    private void OnEnable()
    {
        vector3_originalPosition = this.gameObject.transform.position;
        bool_distanceClosed = false;
    }

    private void OnDisable()
    {
        bool_distanceClosed = false;
    }

    void Update()
    {
        if(!bool_distanceClosed)
        {           
            if (float_initialDistanceToClose_x > 0) //Distance to close is positive, enemy object moves to the right
            {
                this.gameObject.transform.Translate(this.gameObject.transform.right * float_speed);

                if (gameObject.transform.position.x >= (vector3_originalPosition + vector3_translation).x)
                {
                    bool_distance_x_closed = true;
                }
            }
            else if (float_initialDistanceToClose_x < 0) //Distance to close is negative, enemy object moves to the left
            {
                this.gameObject.transform.Translate(-this.gameObject.transform.right * float_speed); //'-' is used to invert direction

                if (gameObject.transform.position.x <= (vector3_originalPosition + vector3_translation).x)
                {
                    bool_distance_x_closed = true;
                }
            }
            else //In this case, there is no distance to close
            {
                bool_distance_x_closed = true;
            }

            if (float_initialDistanceToClose_y > 0) //Enemy is scripted to move up
            {
                this.gameObject.transform.Translate(this.gameObject.transform.up * float_speed);

                if (gameObject.transform.position.y >= (vector3_originalPosition + vector3_translation).y)
                {
                    bool_distance_y_closed = true;
                }
            }
            else if (float_initialDistanceToClose_y < 0) //Enemy is scripted to move down
            {
                this.gameObject.transform.Translate(-this.gameObject.transform.up * float_speed ); //'-' is used to invert direction

                if (gameObject.transform.position.y <= (vector3_originalPosition + vector3_translation).y)
                {
                    bool_distance_y_closed = true;
                }
            }
            else
            {
                bool_distance_y_closed = true;
            }

            if (bool_distance_x_closed
            && bool_distance_y_closed)
            {
                bool_distanceClosed = true;
            }
        }
        
    }
}
