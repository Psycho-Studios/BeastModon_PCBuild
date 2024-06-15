using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyHealth_Generic : EnemyData
{
	private bool bool_selfDestructCalled;
	public bool bool_isParentObject;
	private int int_totalDamage, int_originalLifePoints;
	public GameObject gameObject_explosionVisualEffect;

	private void Awake() 
	{
		bool_bossEnemy = false;
		bool_defeated = false;
		int_totalDamage = 0;
	}

    private void OnEnable()
    {
		int_originalLifePoints = int_lifePoints;
    }

	private void Update()
	{
		if (int_lifePoints <= 0
		&& bool_selfDestructCalled) //Did this gameobject explode as an attack? (Nathaniel, etc.)
		{
			bool_defeated = true;
			if (this.gameObject.name.Contains("Nathaniel"))
            {
				this.gameObject_explosionVisualEffect = ObjectPool.objectPool_reference.getPooled_LevelSpecificObjects(
					Convert.ToInt32(E_SpawnableObjects.TestingScene.Nathaniel_Explosion));

				if (!this.gameObject_explosionVisualEffect.Equals(null))
				{
					this.gameObject_explosionVisualEffect.transform.position = this.gameObject.transform.position;
					this.gameObject_explosionVisualEffect.transform.rotation = Quaternion.identity;
					int_lifePoints = int_originalLifePoints;
					this.gameObject_explosionVisualEffect.SetActive(true);
				}
				
			}

			//Leaving a comment here to emphasize space for more GameObjects filtered by name
			bool_defeated = false;
			bool_selfDestructCalled = false;
			this.gameObject.SetActive(false);
		}
		else if (int_lifePoints <= 0)
		{
			bool_defeated = true;

			//Logic for Spy enemies, the explosion is only a larger version of the player death explosion
            if (this.gameObject.name.Contains("Walk_n_Spy")
            || this.gameObject.name.Contains("Jump_n_Spy"))
            {
                this.gameObject_explosionVisualEffect = ObjectPool.objectPool_reference.getPooled_MiscellaneousObjects(
                    Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Explosion_WalknSpy));
            }

            if (!this.gameObject_explosionVisualEffect.Equals(null)) //We want to retrieve a pooled explosion via name
			{
                this.gameObject_explosionVisualEffect = ObjectPool.objectPool_reference.getPooled_MiscellaneousObjects(
                    Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Explosion_1),
                    this.gameObject_explosionVisualEffect.name);

                this.gameObject_explosionVisualEffect.transform.position = this.gameObject.transform.position;
				this.gameObject_explosionVisualEffect.transform.rotation = Quaternion.identity;
				this.gameObject_explosionVisualEffect.SetActive(true);
			}

			int_lifePoints = int_originalLifePoints; //GameObjects are reused via the object pool, enabling them with 0 life will spawn them inactive
			bool_defeated = false;
			this.gameObject.SetActive(false);
		}
	}

    private void OnTriggerEnter2D(Collider2D playerProjectile)
	{
		if(playerProjectile.tag.Contains("PlayerProjectile") //Every player projectile does damage
		&& !bool_defeated) //The enemy is still alive
        {
			switch(playerProjectile.name)
            {
				case "Projectile_Ballistic_1(Clone)":
                {
					int_totalDamage = 4;
					break;
                }
				case "Projectile_Ballistic_2(Clone)":
				{
					int_totalDamage = 5;
					break;
				}
				case "Projectile_Ballistic_3(Clone)":
				{
					int_totalDamage = 6;
					break;
				}
				case "Projectile_Ballistic_4(Clone)":
				{
					int_totalDamage = 7;
					break;
				}
				case "Projectile_Ballistic_5(Clone)":
				{
					int_totalDamage = 7;
					break;
				}
				case "Ballistic_Beast(Clone)":
				{
					int_totalDamage = 125;
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
					int_totalDamage = 2;
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
					int_totalDamage = 3;
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
					int_totalDamage = 4;
					break;
                }

				case "Projectile_Melee_1(Clone)":
				{
					int_totalDamage = 7;
					break;
				}
				case "Projectile_Melee_2(Clone)":
				{
					int_totalDamage = 10;
					break;
				}
				case "Projectile_Melee_3(Clone)":
				case "Projectile_Melee_3_Shield(Clone)":
				case "Projectile_Melee_4_Shield(Clone)":
				case "Projectile_Melee_4_Side(Clone)":
				case "Projectile_Melee_4_Tail_Lower(Clone)":
				{
					int_totalDamage = 11;
					break;
				}
				case "Projectile_Melee_5_Shield(Clone)":
				case "Projectile_Melee_5_Side(Clone)":
				case "Projectile_Melee_5_Tail_Lower(Clone)":
				case "Projectile_Melee_5_Tail_Upper(Clone)":
				{
					int_totalDamage = 12;
					break;
				}
				case "Projectile_Melee_Beast_Shield(Clone)":
				case "Projectile_Melee_Beast_Side(Clone)":
				case "Projectile_Melee_Beast_Tail_Lower(Clone)":
				case "Projectile_Melee_Beast_Tail_Upper(Clone)":
				{
					int_totalDamage = 25;
					break;
				}
				case "Projectile_Explosive_1(Clone)":
				{
					int_totalDamage = 12;
					break;
				}
				case "Projectile_Explosive_2(Clone)":
				{
					int_totalDamage = 14;
					break;
				}
				case "Projectile_Explosive_3(Clone)":
				{
					int_totalDamage = 15;
					break;
				}
				case "Projectile_Explosive_4(Clone)":
				{
					int_totalDamage = 23;
					break;
				}
				case "Projectile_Explosive_Beast_Collider_1(Clone)":
				case "Projectile_Explosive_Beast_Collider_2(Clone)":
				case "Projectile_Explosive_Beast_Collider_3(Clone)":
				{
					int_totalDamage = 35;
					break;
				}
				case "Projectile_Energy_1":
				{
					int_totalDamage = 6;
					break;
				}
				case "Projectile_Energy_2(Clone)":
				{
					int_totalDamage = 25;
					break;
				}
				case "Projectile_Energy_3(Clone)":
				{
					int_totalDamage = 30;
					break;
				}
				case "Projectile_Energy_4(Clone)":
				{
					int_totalDamage = 45;
					break;
				}
				case "Projectile_Energy_5(Clone)":
				{
					int_totalDamage = 250;
					break;
				}
				case "Projectile_Energy_Beast(Clone)":
				{
					int_totalDamage = 1000;
					break;
				}
				case "ChargedShot(Clone)":
				{
					int_totalDamage = 125;
					break;
				}
				case "FloorSlam(Clone)":
				{
					if (this.bool_slammableEnemy)
					{
						this.int_lifePoints = 0;
					}
					else if (this.bool_slammableWall)
					{
						//Add logic for walls here, idea is for them to sink downward
					}
					else
					{
						int_totalDamage = 20;
					}
					break;
				}
			}
			receiveDamage(int_totalDamage);
        }
	}

	protected override void receiveDamage(int totalDamage)
	{
   		int_lifePoints -= totalDamage;
		int_totalDamage = 0; //Set to 0 again to avoid the damage being done at once getting stacked with the previous projectile's damage
	}

	public void event_explode_selfDestruct()
    {
		this.int_lifePoints = 0;
		bool_selfDestructCalled = true;
    }
}