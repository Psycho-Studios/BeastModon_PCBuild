using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_ExplosiveBeast_SetPosition : MonoBehaviour
{
    public bool bool_isThisShakingUpAndDown;
    private bool bool_shakeUpwards;
    //Not including z at the moment as this (Beast Modon) is a 2D game.
    public float float_extraDistance_x, float_extraDistance_y,
        float_timeUntilNextShake, float_timeSinceLastShake;

    void Start()
    {
        float_timeUntilNextShake = 0.0625f;
        this.gameObject.transform.position = new Vector3(
            Camera.main.transform.position.x + float_extraDistance_x,
            Camera.main.transform.position.y + float_extraDistance_y,
            0.0f);
    }
    private void Update()
    {
        //This set of 'if' statements is for the fifth Beast Weapon
        if(bool_isThisShakingUpAndDown
        &&(( Time.time - float_timeSinceLastShake) >= float_timeUntilNextShake))
        {
            float_timeSinceLastShake = Time.time;
            if(bool_shakeUpwards)
            {
                this.gameObject.transform.position = new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y + 1f,
                    this.gameObject.transform.position.z);
                bool_shakeUpwards = false;
            }
            else
            {
                this.gameObject.transform.position = new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y - 1f,
                    this.gameObject.transform.position.z);
                bool_shakeUpwards = true;
            }
        }
    }
}
