using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    private int int_healthBeforeHealing;
    private string string_collisionTag;
    private AudioSource _audiosource;
    private GameObject gameObject_itemAcquiredEffect;

    private void Awake()
    {
        this._audiosource = this.gameObject.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Contains("Player1")
        || collision.tag.Contains("Player2"))
        {
            this.string_collisionTag = collision.tag;
            gameObject_itemAcquiredEffect = null;

            if (this.gameObject.name.Contains("PowerUp_RankUp"))
            {
                ProjectileControls_Player1.int_ranksAvailable += 1;
                gameObject_itemAcquiredEffect = ObjectPool.objectPool_reference.getPooled_MiscellaneousObjects(
                    Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Effect_RankUp));
            }
            else if (this.gameObject.name.Contains("PowerUp_BeastModeUp"))
            {
                ProjectileControls_Player1.int_beastModesAvailable += 1;
                gameObject_itemAcquiredEffect = ObjectPool.objectPool_reference.getPooled_MiscellaneousObjects(
                    Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Effect_BeastModeUp));
            }
            else if (this.gameObject.name.Contains("PowerUp_HealthUp"))
            {
                gameObject_itemAcquiredEffect = ObjectPool.objectPool_reference.getPooled_MiscellaneousObjects(
                    Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Effect_HealthUp));
                
                int_healthBeforeHealing = Health_Player1.int_life;
                
                switch (GameProperties.DataManagement.GameData.string_currentDifficulty)
                {
                    case "Easy":
                    {
                        if (Health_Player1.int_life == 150)
                        {
                            ProjectileControls_Player1.int_beastModesAvailable += 1;
                        }
                        else
                        {
                            Health_Player1.int_life += 50;
                        }
                        break;
                    }
                    case "Normal":
                    {
                        if (Health_Player1.int_life == 100)
                        {
                            ProjectileControls_Player1.int_beastModesAvailable += 1;
                        }
                        else
                        {
                            Health_Player1.int_life += 50;
                        }
                        break;
                    }
                    case "Arcade":
                    {
                        if (Health_Player1.int_life == 50)
                        {
                            ProjectileControls_Player1.int_beastModesAvailable += 1;
                        }
                        else
                        {
                            Health_Player1.int_life += 50;
                        }
                        break;
                    }
                    default:
                    {
                        if (Health_Player1.int_life == 150)
                        {
                            ProjectileControls_Player1.int_beastModesAvailable += 1;
                        }
                        else
                        {
                            Health_Player1.int_life += 50;
                        }
                        break;
                    }
                }
            }
            else if (this.gameObject.name.Contains("PowerUp_LetsGo"))
            {
                gameObject_itemAcquiredEffect = ObjectPool.objectPool_reference.getPooled_MiscellaneousObjects(
                    Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Effect_LetsGo));
            }

            if (gameObject_itemAcquiredEffect != null)
            {
                gameObject_itemAcquiredEffect.transform.position = collision.transform.position +  new Vector3(
                    1.0f,
                    0,
                    0);
                gameObject_itemAcquiredEffect.transform.rotation = collision.transform.rotation;

                if (gameObject_itemAcquiredEffect.name.Contains("PowerUp_HealthUp_VisualEffect"))
                {
                    gameObject_itemAcquiredEffect.GetComponent<DamageVisualEffect>().determineHealingSound(int_healthBeforeHealing);
                }

                if (gameObject_itemAcquiredEffect.name.Contains("PowerUp_LetsGo_VisualEffect")
                && collision.tag.Contains("Player1")) //Add logic for player2
                {
                    FaceAnimation_Player1.animator_player1Face.SetInteger("Expression", 10); //Animation calls method in FaceAnimation_Player1
                    ProjectileControls_Player1.bool_fastFiringActive = true;
                }
                gameObject_itemAcquiredEffect.SetActive(true);
            }
            this.gameObject.SetActive(false);

        }
        //else if (collision.tag.Contains("Player2"))
        //{
        //    if (this.gameObject.name.Contains("HealthUp"))
        //    {

        //    }
        //    if (this.gameObject.name.Contains("RankUp"))
        //    {

        //    }
        //    if (this.gameObject.name.Contains("BeastModeUp"))
        //    {

        //    }
        //}
    }

}
