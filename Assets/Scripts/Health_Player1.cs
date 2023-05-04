using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Player1 : MonoBehaviour
{
    private bool bool_player1Hit;
    public static bool bool_playerReactingToDamage, bool_enemyGameObjectsDealDamage;
    public static int int_life;
    private Collider2D collider2D_player1;
    public GameObject gameObject_shipExplosion;
    private SpriteRenderer spriteRenderer_player1Ship;
    private void Awake()
    {
        collider2D_player1 = this.gameObject.GetComponent<Collider2D>();
        spriteRenderer_player1Ship = this.gameObject.GetComponent<SpriteRenderer>();

        switch (GameProperties.DataManagement.GameData.string_currentDifficulty)
        {
            case "Easy":
                {
                    int_life = 150;
                    break;
                }
            case "Normal":
                {
                    int_life = 100;
                    break;
                }
            case "Arcade":
                {
                    int_life = 50;
                    break;
                }
            default:
                {
                    int_life = 150;
                    break;
                }
        }
    }
    private void Start()
    {
        bool_enemyGameObjectsDealDamage = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bool_enemyGameObjectsDealDamage)
        {
            switch (collision.tag)
            {
                case "Enemy":
                case "EnemyProjectile":
                case "WeakPoint":
                {
                    if (!ProjectileControls_Player1.bool_beastModeActive
                    && int_life != 0
                    && !bool_player1Hit)
                    {
                        bool_player1Hit = true;
                        bool_enemyGameObjectsDealDamage = false;
                        receiveDamage();
                    }
                    break;
                }
            }
        }
    }

    /*Called when player1 receives damage initially, this triggers the damage animation, which makes calls
    to methods written in FaceAnimation_Player1. Those will determine whether the player stays in their
    damaged state or recovers their original animation status.*/
    public void receiveDamage()
    {
        int_life -= 50;
        updatePlayer1WeaponValue();
        HUD_Player1.bool_rankLowerRequest_player1 = true; //Animate HUD
        StartCoroutine(damageReceptionCooldown()); //Invincibility
        if (int_life == 0) //DamageIdle will be playing if not in Arcade mode
        {
            FaceAnimation_Player1.animator_player1Face.SetInteger("Expression", -1);
            
            GameObject gameObject_player1ExplosionClone = ObjectPool.objectPool_reference.getPooled_MiscellaneousObjects(
                Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Explosion_Player1Ship));

            gameObject_player1ExplosionClone.transform.position = this.gameObject.transform.position;
            gameObject_player1ExplosionClone.transform.rotation = this.gameObject.transform.rotation;
            
            this.gameObject.SetActive(false);
        }
        else
        {
            //Record previous animation state for a switch statement
            FaceAnimation_Player1.int_currentAnimationState = FaceAnimation_Player1.animator_player1Face.GetInteger("Expression");

            //Begin the Damage animation
            FaceAnimation_Player1.animator_player1Face.SetInteger("Expression", 1);
        }
    }

    /// <summary>
    /// After three seconds the player is no longer invincible
    /// </summary>
    /// <returns></returns>
    IEnumerator damageReceptionCooldown()
    {
        Debug.Log("Current Life:" + int_life.ToString());
        yield return new WaitForSeconds(3.0f);
        bool_playerReactingToDamage = false;
        bool_player1Hit = false;
        if (int_life > 0) //Avoids collision possibilities after player death.
        {
            bool_enemyGameObjectsDealDamage = true;
            FaceAnimation_Player1.bool_safeToContinueTalking = true;
        }
    }

    private void updatePlayer1WeaponValue()
    {
        if (ProjectileControls_Player1.array_int_weaponStrength[
            ProjectileControls_Player1.int_weaponIndex_player1] > 0)
        {
            ProjectileControls_Player1.array_int_weaponStrength[
                ProjectileControls_Player1.int_weaponIndex_player1] -= 1; //Lessen weapon rank

            ProjectileControls_Player1.int_weaponValue_player1 = ProjectileControls_Player1.int_weaponIndex_player1
                + (5 * ProjectileControls_Player1.array_int_weaponStrength[
                    ProjectileControls_Player1.int_weaponIndex_player1]);
        }
    }
}
