using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Included with every projectile, this script handles OnTriggerEnter2D for them.
/// It also generates visual effects for damage dealt to an enemy.
/// </summary>
public abstract class ProjectileData : MonoBehaviour
{
    private bool bool_flameThrowerActive;
    protected bool bool_playerProjectile;
    public bool bool_isParentObject;

    private float float_timeSinceEnergy1Attack;

    private int timesCalledForExplosion;

    private string string_currentEnemyName; //This field is used by flamethrower logic

    private Collider2D collider2D_projectileCollider;
    private GameObject gameObject_damageEffect;
    protected List<Transform> list_childProjectiles;
    private ProjectileBehaviour_Player script_projectileBehaviour_Player;
    private SpriteRenderer spriteRenderer_projectile;
    protected Transform transform_parentObject;
    public Vector3 vector3_child_transformModifier;
    
    private System.Random random = new System.Random();

    protected void initialize_childObjectList()
    {
        this.spriteRenderer_projectile = this.gameObject.GetComponent<SpriteRenderer>();

        list_childProjectiles = new List<Transform>();
        
        for (int i = 0; i < this.transform.childCount; i++)
        {
            list_childProjectiles.Add(this.transform.GetChild(i));
            script_projectileBehaviour_Player = list_childProjectiles[i].gameObject.GetComponent<ProjectileBehaviour_Player>();
            script_projectileBehaviour_Player.transform_parentObject = this.transform;

            script_projectileBehaviour_Player.bool_isChildObject = true;
            script_projectileBehaviour_Player.bool_waitingForParent = false;
        }    
    }

    private void Update()
    {
        if (this.bool_flameThrowerActive
        && (Time.time - float_timeSinceEnergy1Attack >= 0.12f)
        && this.gameObject.name.Contains("Energy_1"))
        {
            this.bool_flameThrowerActive = false;
            float_timeSinceEnergy1Attack = Time.time;
            energy1_repeatingDamageEffect();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bool_playerProjectile)
        {
            switch (collision.tag)
            {
                //Determine which damageEffect to spawn
                case "Enemy":
                case "WeakPoint":
                {
                    //Calculate and generate the generic damage visual effect

                    //Set special weapon types damage effects
                    if (this.gameObject.name.Contains("ChargedShot")
                    || this.gameObject.name.Contains("Ballistic_Beast"))
                    {
                        GameObject damageEffect_blue = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                            default, //This spawn is not dependent on a WeaponValue, default value is 0
                            "VisualEffect_Ballistic_Beast"); //Damage effect for metal opponents
                        damageEffect_blue.transform.position = this.gameObject.transform.position + (this.gameObject.transform.);
                        damageEffect_blue.transform.rotation = Quaternion.Euler(0, 0, 0);
                        damageEffect_blue.SetActive(true);
                    }

                    if(this.gameObject.name.Contains("Energy_5"))
                    {
                            GameObject damageEffect_energyExplosion = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                                default,
                                "Explosion_2");
                            damageEffect_energyExplosion.transform.position = this.gameObject.transform.position;
                            damageEffect_energyExplosion.transform.rotation = Quaternion.Euler(0, 0, 0);
                            damageEffect_energyExplosion.SetActive(true);
                    }

                    //Set typical damage effect
                    if (!this.gameObject.name.Contains("Energy_1")
                    && !this.gameObject.name.Contains("Energy_5"))
                    {
                        retrieveDirectionalDamageEffect(collision);
                    }

                    //If the damage effect is properly generated, activate it
                    if(this.gameObject_damageEffect != null
                    && !this.gameObject.name.Contains("Energy_1")) 
                    {
                        this.gameObject_damageEffect.transform.position = this.gameObject.transform.position;
                        this.gameObject_damageEffect.SetActive(true);
                    }   

                    if(this.gameObject.name.Contains("Explosive")
                    || this.gameObject.name.Contains("Energy_2"))
                    {
                        beginExplosion(this.gameObject.name.ToString(), collision);
                        break;
                    }                    

                    //If projectile is a flamethrower, do not set it false
                    if (this.gameObject.name.Contains("Energy_1")
                    && !this.bool_flameThrowerActive)
                    {
                        this.bool_flameThrowerActive = true;
                        this.string_currentEnemyName = collision.name;
                    }
                    else if(!gameObject.name.Contains("Energy_Beast") //Disappears regardless of collisions
                    && !gameObject.name.Contains("Explosive_Beast") //Needs to be active for its entire heirarchy to exist uncorrupted
                    && !gameObject.name.Contains("ChargedShot") //Disappears regardless of collisions
                    && !gameObject.name.Contains("Energy_1"))
                    {
                        //Returns the projectile to the object pool if not the flamethrower
                        this.gameObject.SetActive(false); 
                    }
                    break;
                }

                case "EnemyProjectile":
                {
                    if (this.gameObject.name.Contains("Explosive")
                    || this.gameObject.name.Contains("Energy_2"))
                    {
                        beginExplosion(this.gameObject.name.ToString(), collision);
                        break;
                    }

                    //Set typical damage effect
                    if (!this.gameObject.name.Contains("Energy_1"))
                    {
                        retrieveDirectionalDamageEffect(collision);
                    }

                    //If properly generated, activate the damage effect
                    if (this.gameObject_damageEffect != null
                    && !this.gameObject.name.Contains("Energy_1"))
                    {
                        this.gameObject_damageEffect.SetActive(true);
                    }
                    break;
                }

                case "ProjectileBoundary":
                {
                    //Energy_Beast should not be affected by projectile boundaries
                    //This should run if object is standard or a child
                    if(!this.gameObject.name.Contains("Energy_Beast"))
                    {
                        if (this.gameObject.name.Contains("Tail")
                        && !gameObject.name.Contains("Puppeteer")) //Child object, tailgun projectile
                        {
                            this.transform.parent = this.transform_parentObject; //Restore previous heirarchy
                        }
                            this.gameObject.SetActive(false);
                        
                    }

                    break;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.gameObject.name.Contains("Energy_1"))
        {
            this.bool_flameThrowerActive = false;
        }
    }

    private void energy1_repeatingDamageEffect()
    {
        if (this.string_currentEnemyName.Contains("Metal"))
        {
            this.gameObject_damageEffect = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                default,
                "VisualEffect_EnemyDamage_Metal_Sides");
        }
        else if(this.string_currentEnemyName.Contains("Flesh"))
        {
            this.gameObject_damageEffect = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                default,
                "VisualEffect_EnemyDamage_Flesh_Sides");
        }
        this.gameObject_damageEffect.transform.position = this.gameObject.transform.position + new Vector3(
            this.random.Next(0, 3),
            0,
            0);
        this.gameObject_damageEffect.transform.rotation = Quaternion.Euler(0, 0, 0);
        this.gameObject_damageEffect.SetActive(true);

    }

    //Meant for projectiles with longer bodies
    private void retrieveDirectionalDamageEffect(Collider2D collision_enemy)
    {
        if (this.gameObject.name.Contains("Explosive_Beast"))
        {
            this.gameObject_damageEffect = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                default,
                "Explosion_1");
            this.gameObject_damageEffect.transform.position = collision_enemy.transform.position;
            this.gameObject_damageEffect.transform.rotation = collision_enemy.transform.rotation;
        }
        else if (collision_enemy.gameObject.name.Contains("Metal"))
            {
                ///Projectile on the under side
                if (this.gameObject.transform.position.y < (collision_enemy.gameObject.transform.position.y - collision_enemy.bounds.size.y / 2))
                {
                    this.gameObject_damageEffect = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                        default,
                        "VisualEffect_EnemyDamage_Metal_Under");
                    this.gameObject_damageEffect.transform.position = new Vector3(
                        this.gameObject.transform.position.x,
                        collision_enemy.transform.position.y - (collision_enemy.bounds.size.y / 2),
                        this.gameObject.transform.position.z);
                }
                //Projectile on the right side
                else if (this.gameObject.transform.position.x > collision_enemy.gameObject.transform.position.x
                    && this.gameObject.transform.position.y < (collision_enemy.transform.position.y + collision_enemy.bounds.size.y / 2)
                    && this.gameObject.transform.position.y > (collision_enemy.transform.position.y - collision_enemy.bounds.size.y / 2))
                {
                    this.gameObject_damageEffect = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                        default,
                        "VisualEffect_EnemyDamage_Metal_Sides");
                    gameObject_damageEffect.transform.position = new Vector3(
                        collision_enemy.transform.position.x + (collision_enemy.bounds.size.x / 2),
                        this.gameObject.transform.position.y,
                        this.gameObject.transform.position.z);
                    gameObject_damageEffect.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                //Projectile on the left side
                else if (this.gameObject.transform.position.x < collision_enemy.gameObject.transform.position.x //Right of projectile
                && this.gameObject.transform.position.y < (collision_enemy.transform.position.y + collision_enemy.bounds.size.y / 2)
                && this.gameObject.transform.position.y > (collision_enemy.transform.position.y - collision_enemy.bounds.size.y / 2))
                {
                    this.gameObject_damageEffect = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                        default,
                        "VisualEffect_EnemyDamage_Metal_Sides");
                    gameObject_damageEffect.transform.position = new Vector3(
                        collision_enemy.transform.position.x - (collision_enemy.bounds.size.x / 2),
                        this.gameObject.transform.position.y,
                        this.gameObject.transform.position.z);
                }
                //Projectile on the top side
                else if (this.gameObject.transform.position.y > (collision_enemy.gameObject.transform.position.y + collision_enemy.bounds.size.y / 2)) //Above projectile
                {
                    this.gameObject_damageEffect = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                        default,
                        "VisualEffect_EnemyDamage_Metal_Under");
                    this.gameObject_damageEffect.transform.position = new Vector3(
                          this.gameObject.transform.position.x,
                          collision_enemy.transform.position.y + (collision_enemy.bounds.size.y / 2),
                          this.gameObject.transform.position.z);
                }
            }
        else if (collision_enemy.gameObject.name.Contains("Flesh"))
            {
                ///Projectile on the under side
                if (this.gameObject.transform.position.y < (collision_enemy.gameObject.transform.position.y - collision_enemy.bounds.size.y / 2))
                {
                    this.gameObject_damageEffect = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                        default,
                        "VisualEffect_EnemyDamage_Flesh_Under");
                    this.gameObject_damageEffect.transform.position = new Vector3(
                        this.gameObject.transform.position.x,
                        collision_enemy.transform.position.y - (collision_enemy.bounds.size.y / 2),
                        this.gameObject.transform.position.z);
                }
                //Projectile on the right side
                else if (this.gameObject.transform.position.x > collision_enemy.gameObject.transform.position.x
                    && this.gameObject.transform.position.y < (collision_enemy.transform.position.y + collision_enemy.bounds.size.y / 2)
                    && this.gameObject.transform.position.y > (collision_enemy.transform.position.y - collision_enemy.bounds.size.y / 2))
                {
                    this.gameObject_damageEffect = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                        default,
                        "VisualEffect_EnemyDamage_Flesh_Sides");
                    gameObject_damageEffect.transform.position = new Vector3(
                        collision_enemy.transform.position.x + (collision_enemy.bounds.size.x / 2),
                        this.gameObject.transform.position.y,
                        this.gameObject.transform.position.z);
                    gameObject_damageEffect.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                //Projectile on the left side
                else if (this.gameObject.transform.position.x < collision_enemy.gameObject.transform.position.x //Right of projectile
                && this.gameObject.transform.position.y < (collision_enemy.transform.position.y + collision_enemy.bounds.size.y / 2)
                && this.gameObject.transform.position.y > (collision_enemy.transform.position.y - collision_enemy.bounds.size.y / 2))
                {
                    this.gameObject_damageEffect = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                        default,
                        "VisualEffect_EnemyDamage_Flesh_Sides");
                    gameObject_damageEffect.transform.position = new Vector3(
                        collision_enemy.transform.position.x - (collision_enemy.bounds.size.x / 2),
                        this.gameObject.transform.position.y,
                        this.gameObject.transform.position.z);
                }
                //Projectile on the top side
                else if (this.gameObject.transform.position.y > (collision_enemy.gameObject.transform.position.y + collision_enemy.bounds.size.y / 2)) //Above projectile
                {
                    this.gameObject_damageEffect = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                        default,
                        "VisualEffect_EnemyDamage_Flesh_Under");
                    this.gameObject_damageEffect.transform.position = new Vector3(
                          this.gameObject.transform.position.x,
                          collision_enemy.transform.position.y + (collision_enemy.bounds.size.y / 2),
                          this.gameObject.transform.position.z);
                }
            }

    }

    private void beginExplosion(string projectileName, Collider2D collision_enemy)
    {
        switch(projectileName) //Determine which explosion to spawn
        {
            case "Projectile_Explosive_2(Clone)":
            case "Projectile_Explosive_4(Clone)":
            {
                this.gameObject_damageEffect = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                default,
                "Explosion_3");
                break;
            }
            default:
            {
                this.gameObject_damageEffect = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
                default,
                "Explosion_1");
                break;
            }
        }

        ///Projectile on the under side
        if (this.gameObject.transform.position.y < (collision_enemy.gameObject.transform.position.y - collision_enemy.bounds.size.y / 2))
        {
            this.gameObject_damageEffect.transform.position = new Vector3(
                this.gameObject.transform.position.x,
                collision_enemy.transform.position.y - (collision_enemy.bounds.size.y / 2),
                this.gameObject.transform.position.z);
        }
        //Projectile on the right side
        else if (this.gameObject.transform.position.x > collision_enemy.gameObject.transform.position.x
            && this.gameObject.transform.position.y < (collision_enemy.transform.position.y + collision_enemy.bounds.size.y / 2)
            && this.gameObject.transform.position.y > (collision_enemy.transform.position.y - collision_enemy.bounds.size.y / 2))
        {
            gameObject_damageEffect.transform.position = new Vector3(
                collision_enemy.transform.position.x + (collision_enemy.bounds.size.x / 2),
                this.gameObject.transform.position.y,
                this.gameObject.transform.position.z);
            gameObject_damageEffect.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        //Projectile on the left side
        else if (this.gameObject.transform.position.x < collision_enemy.gameObject.transform.position.x //Right of projectile
            && this.gameObject.transform.position.y < (collision_enemy.transform.position.y + collision_enemy.bounds.size.y / 2)
            && this.gameObject.transform.position.y > (collision_enemy.transform.position.y - collision_enemy.bounds.size.y / 2))
        {
            gameObject_damageEffect.transform.position = new Vector3(
                collision_enemy.transform.position.x - (collision_enemy.bounds.size.x / 2),
                this.gameObject.transform.position.y,
                this.gameObject.transform.position.z);
        }
        //Projectile on the top side
        else if (this.gameObject.transform.position.y > (collision_enemy.gameObject.transform.position.y + collision_enemy.bounds.size.y / 2)) //Above projectile
        {
            this.gameObject_damageEffect.transform.position = new Vector3(
                  this.gameObject.transform.position.x,
                  collision_enemy.transform.position.y + (collision_enemy.bounds.size.y / 2),
                  this.gameObject.transform.position.z);
        }
        else
        {
            this.gameObject_damageEffect.transform.position = new Vector3(
                  this.gameObject.transform.position.x,
                  collision_enemy.transform.position.y + (collision_enemy.bounds.size.y / 2),
                  this.gameObject.transform.position.z);
        }

        timesCalledForExplosion++;
        Debug.Log($"timesCalledForExplosion = {timesCalledForExplosion}");
        gameObject_damageEffect.SetActive(true);
        this.gameObject.SetActive(false); //If this method is being called, the projectile is no more.
    }

    public abstract void launchProjectile();
}
