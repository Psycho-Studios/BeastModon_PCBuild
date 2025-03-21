using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Player1 : MonoBehaviour
{
    private bool bool_player1Hit;
    public static bool bool_playerReactingToDamage, bool_enemyGameObjectsDealDamage;
    
    /// <summary>
    /// Is set true if the difficulty is not Arcade and the player's life reaches 50.
    /// </summary>
    public static bool bool_criticalStatus;
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
        FaceAnimation_Player1.bool_dialogueAudioInterrupted = true;
        FaceAnimation_Player1.bool_damage_FaceCooldown = true;
        FaceAnimation_Player1.float_timePlayer1HaltedAnimation = Time.time;
        FaceAnimation_Player1.int_currentAnimationState = FaceAnimation_Player1.expressionIndex;

        if(int_life == 50
        && !GameProperties.DataManagement.GameData.string_currentDifficulty.Contains("Arcade"))
        {
            bool_criticalStatus = true;
        }
        
        if (int_life == 0) //DamageIdle will be playing if not in Arcade mode
        {
            FaceAnimation_Player1.animator_player1Face.SetInteger("Expression", (int)E_FaceExpressions.Death);
            FaceAnimation_Player1.expressionIndex = -1;
            FaceAnimation_Player1.bool_player1Dead = true;

            GameObject gameObject_player1ExplosionClone = ObjectPool.objectPool_reference.getPooled_MiscellaneousObjects(
                Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Explosion_Player1Ship));

            gameObject_player1ExplosionClone.transform.position = this.gameObject.transform.position;
            gameObject_player1ExplosionClone.transform.rotation = this.gameObject.transform.rotation;

            gameObject_player1ExplosionClone.SetActive(true);


            this.gameObject.SetActive(false);
        }
        else 
        {         
            //Begin the Damage animation
            FaceAnimation_Player1.animator_player1Face.SetInteger("Expression", (int)E_FaceExpressions.Damage);
        }
    }

    /// <summary>
    /// After three seconds the player is no longer invincible
    /// </summary>
    /// <returns></returns>
    IEnumerator damageReceptionCooldown()
    {
        if (!ProjectileControls_Player1.bool_beastModeActive)
        {
            //StartCoroutine(FaceAnimation_Player1.delayedDialogue());

            yield return new WaitForSeconds(GameProperties.StatusInterruptionReporter.GetTimeUntilDialogueBegins(E_AudioInfluencers.Damage));
            
            bool_playerReactingToDamage = false;
            bool_player1Hit = false;
            if (int_life > 0) //Avoids collision possibilities after player death.
            {
                bool_enemyGameObjectsDealDamage = true; //Invulnerability ended
                FaceAnimation_Player1.bool_damage_FaceCooldown = false; //Face no longer in damaged state, triggers proper face animation
                GameProperties.StatusInterruptionReporter.bool_safeToContinueDialogueAnimation = true; //Audible and visible character conversation allowed
            }
        }
        else //Beast Mode is active, ProjectileControls_Player1 will disable invulnerability
        {
            bool_playerReactingToDamage = false;
            FaceAnimation_Player1.bool_damage_FaceCooldown = false;
            yield return new WaitForSeconds(GameProperties.StatusInterruptionReporter.GetTimeUntilDialogueBegins(E_AudioInfluencers.BeastMode));
            GameProperties.StatusInterruptionReporter.bool_safeToContinueDialogueAnimation = true;
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
