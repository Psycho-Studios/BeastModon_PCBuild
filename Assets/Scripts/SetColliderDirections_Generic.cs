using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColliderDirections_Generic : MonoBehaviour
{
    public E_ColliderDirections colliderDirection;

    public bool bool_genericEnemy;
    public bool bool_bossEnemy;

    [HideInInspector]
    public EnemyHealth_Generic script_enemyHealth_Generic;

    public void Awake()
    {
        script_enemyHealth_Generic = GetComponentInParent<EnemyHealth_Generic>();
        switch (gameObject.tag)
        {
            case "EnemyNorthCollider_Metal":
            case "EnemyNorthCollider_Flesh":
            {
                this.colliderDirection = E_ColliderDirections.North;
                break;
            }
            
            case "EnemyEastCollider_Metal":
            case "EnemyEastCollider_Flesh":
            {
                this.colliderDirection = E_ColliderDirections.East;
                break;
            }
            case "EnemySouthCollider_Metal":
            case "EnemySouthCollider_Flesh":
            {
                this.colliderDirection = E_ColliderDirections.South;
                break;
            }
            case "EnemyWestCollider_Metal":
            case "EnemyWestCollider_Flesh":
            {
                this.colliderDirection = E_ColliderDirections.West;
                break;
            }
            case "EnemyCenterCollider_Metal":
            case "EnemyCenterCollider_Flesh":
            {
                this.colliderDirection = E_ColliderDirections.Center;
                break;
            }
            
        }
    }

    public void OnTriggerEnter2D(Collider2D collidingObject)
    {

        if (!script_enemyHealth_Generic.bool_defeated) //The enemy is still alive
        {
            switch (collidingObject.tag)
            {
                case "Projectile_Ballistic_1(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 4;
                    break;
                }
                case "Projectile_Ballistic_2(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 5;
                    break;
                }
                case "Projectile_Ballistic_3(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 6;
                    break;
                }
                case "Projectile_Ballistic_4(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 7;
                    break;
                }
                case "Projectile_Ballistic_5(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 7;
                    break;
                }
                case "Ballistic_Beast(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 125;
                    break;
                }
                case "Projectile_Tail_1_Right":
                case "Projectile_Tail_1_Left":

                case "Projectile_Tail_2_-5Degrees":
                case "Projectile_Tail_2_-5Degrees_Double":
                case "Projectile_Tail_2_0Degrees":
                case "Projectile_Tail_2_0Degrees_Double":
                case "Projectile_Tail_2_5Degrees":
                case "Projectile_Tail_2_5Degrees_Double":
                case "Projectile_Tail_2_175Degrees":
                case "Projectile_Tail_2_175Degrees_Double":
                case "Projectile_Tail_2_180Degrees":
                case "Projectile_Tail_2_180Degrees_Double":
                case "Projectile_Tail_2_185Degrees":
                case "Projectile_Tail_2_185Degrees_Double":
                {
                    script_enemyHealth_Generic.int_totalDamage = 2;
                    break;
                }

                case "Projectile_Tail_3_-5Degrees":
                case "Projectile_Tail_3_-5Degrees_Double":
                case "Projectile_Tail_3_0Degrees":
                case "Projectile_Tail_3_0Degrees_Double":
                case "Projectile_Tail_3_5Degrees":
                case "Projectile_Tail_3_5Degrees_Double":
                case "Projectile_Tail_3_175Degrees":
                case "Projectile_Tail_3_175Degrees_Double":
                case "Projectile_Tail_3_180Degrees":
                case "Projectile_Tail_3_180Degrees_Double":
                case "Projectile_Tail_3_185Degrees":
                case "Projectile_Tail_3_185Degrees_Double":
                {
                    script_enemyHealth_Generic.int_totalDamage = 3;
                    break;
                }
                case "Projectile_Tail_4_Right":
                case "Projectile_Tail_4_Right_Double": //PowerUp_LetsGo causes this object's existence
                case "Projectile_Tail_4_Left":
                case "Projectile_Tail_4_Left_Double":

                case "Projectile_Tail_5_0Degrees":
                case "Projectile_Tail_5_45Degrees":
                case "Projectile_Tail_5_85Degrees":
                case "Projectile_Tail_5_95Degrees":
                case "Projectile_Tail_5_135Degrees":
                case "Projectile_Tail_5_180Degrees":
                case "Projectile_Tail_Beast_Ballistic_2":
                case "Projectile_Tail_Beast_Ballistic_2_2":
                case "Projectile_Tail_Beast_Ballistic_3":
                case "Projectile_Tail_Beast_Ballistic_3_2":
                case "Projectile_Tail_Beast_Tail_4":
                case "Projectile_Tail_Beast_Tail_4_2":
                case "Projectile_Tail_Beast_Tail_4_3":
                case "Projectile_Tail_Beast_Tail_4_4":
                case "Projectile_Tail_Beast_Ballistic_2_Double":
                case "Projectile_Tail_Beast_Ballistic_2_2_Double":
                case "Projectile_Tail_Beast_Ballistic_3_Double":
                case "Projectile_Tail_Beast_Ballistic_3_2_Double":
                case "Projectile_Tail_Beast_Tail_4_Double":
                case "Projectile_Tail_Beast_Tail_4_2_Double":
                case "Projectile_Tail_Beast_Tail_4_3_Double":
                case "Projectile_Tail_Beast_Tail_4_4_Double":
                {
                    script_enemyHealth_Generic.int_totalDamage = 4;
                    break;
                }

                case "Projectile_Melee_1(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 7;
                    break;
                }
                case "Projectile_Melee_2(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 10;
                    break;
                }
                case "Projectile_Melee_3(Clone)":
                case "Projectile_Melee_3_Shield(Clone)":
                case "Projectile_Melee_4_Shield(Clone)":
                case "Projectile_Melee_4_Side(Clone)":
                case "Projectile_Melee_4_Tail_Lower(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 11;
                    break;
                }
                case "Projectile_Melee_5_Shield(Clone)":
                case "Projectile_Melee_5_Side(Clone)":
                case "Projectile_Melee_5_Tail_Lower(Clone)":
                case "Projectile_Melee_5_Tail_Upper(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 12;
                    break;
                }
                case "Projectile_Melee_Beast_Shield(Clone)":
                case "Projectile_Melee_Beast_Side(Clone)":
                case "Projectile_Melee_Beast_Tail_Lower(Clone)":
                case "Projectile_Melee_Beast_Tail_Upper(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 25;
                    break;
                }
                case "Projectile_Explosive_1(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 12;
                    break;
                }
                case "Projectile_Explosive_2(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 14;
                    break;
                }
                case "Projectile_Explosive_3(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 15;
                    break;
                }
                case "Projectile_Explosive_4(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 23;
                    break;
                }
                case "Projectile_Explosive_Beast_Collider_1(Clone)":
                case "Projectile_Explosive_Beast_Collider_2(Clone)":
                case "Projectile_Explosive_Beast_Collider_3(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 35;
                    break;
                }
                case "Projectile_Energy_1":
                {
                    script_enemyHealth_Generic.int_totalDamage = 6;
                    break;
                }
                case "Projectile_Energy_2(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 25;
                    break;
                }
                case "Projectile_Energy_3(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 30;
                    break;
                }
                case "Projectile_Energy_4(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 45;
                    break;
                }
                case "Projectile_Energy_5(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 250;
                    break;
                }
                case "Projectile_Energy_Beast(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 1000;
                    break;
                }
                case "ChargedShot(Clone)":
                {
                    script_enemyHealth_Generic.int_totalDamage = 125;
                    break;
                }
                case "FloorSlam(Clone)":
                {
                    if (this.script_enemyHealth_Generic.bool_slammableEnemy)
                    {
                        this.script_enemyHealth_Generic.int_lifePoints = 0;
                    }
                    else if (this.script_enemyHealth_Generic.bool_slammableWall)
                    {
                        //Add logic for walls here, idea is for them to sink downward
                    }
                    else
                    {
                        script_enemyHealth_Generic.int_totalDamage = 20;
                    }
                    break;
                }
            }
            script_enemyHealth_Generic.calculateDamage(script_enemyHealth_Generic.int_totalDamage);
        }
    }
}
