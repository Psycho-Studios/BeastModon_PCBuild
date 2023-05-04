using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Only needed if the enemy is holding a weapon that fires projectiles.
/// If enemy uses itself or has an animation that includes gunfire, this
/// class is not needed.
/// This script will likely be reused by other game objects.
/// </summary>
public class EnemyWeapon_Gyrogat : MonoBehaviour
{
    private bool bool_readyToFire;
    public bool bool_flip_x, bool_flip_y, bool_flip_z;
    public float float_degreesToRotate;
    public float float_timeUntilNextShot;
    private float float_timeLastFiredShot;
    private Animator animator_GyrogatGatlingGun;
    public AudioClip audioClip_projectileSound;
    private AudioSource audioSource_GyrogatGatlingGun;

    private void Awake()
    {
        this.animator_GyrogatGatlingGun = this.gameObject.GetComponent<Animator>();
        this.audioSource_GyrogatGatlingGun = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - float_timeLastFiredShot >= float_timeUntilNextShot)
        {
            float_timeLastFiredShot = Time.time;
            this.animator_GyrogatGatlingGun.SetBool("FireWeapon", true);
        }
    }

    //Used during the gunfire animation to simulate projectile exiting from gun barrel
    public void fireWeapon()
    {
        int x = Convert.ToInt32(E_SpawnableObjects.Tutorial.Gyrogat_Projectile_Flesh);
        GameObject gameObject_projectile = ObjectPool.objectPool_reference.getPooled_LevelSpecificObjects(
            Convert.ToInt32(E_SpawnableObjects.Tutorial.Gyrogat_Projectile_Flesh));
        gameObject_projectile.transform.position = this.gameObject.transform.position;
        
        if(bool_flip_x)
        {
            gameObject_projectile.transform.rotation = Quaternion.Euler(180.0f, 0.0f, 0.0f);
        }
        else if(bool_flip_y)
        {
            gameObject_projectile.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        else if(bool_flip_z)
        {
            gameObject_projectile.transform.rotation = Quaternion.Euler(180.0f, 0.0f, 180.0f);
        }
        else if (float_degreesToRotate != 0)
        {
            gameObject_projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, float_degreesToRotate);
        }
        else
        {
            gameObject_projectile.transform.rotation = Quaternion.identity;
        }

        gameObject_projectile.SetActive(true);
        this.audioSource_GyrogatGatlingGun.PlayOneShot(this.audioClip_projectileSound);
        this.animator_GyrogatGatlingGun.SetBool("FireWeapon", false);
    }
}
