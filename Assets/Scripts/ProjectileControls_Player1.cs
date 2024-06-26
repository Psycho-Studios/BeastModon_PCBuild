using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*This class is attached to the Player1_Ship object.
It's used for shooting; when the player pushes the 
shoot key, the appropriate sound is played and the
shot is spawned where the transform of the appropriate
gun barrel is.
Every projectile has a different sound, damage, and sprite.*/
public class ProjectileControls_Player1 : MonoBehaviour
{
//Primitive Data Types
    private bool bool_player1NextShotReady,
        bool_weaponSlotReassigning, bool_rankUpgradeInProgress,
        bool_energy4_rightAngle, bool_tail5_subAngles,
        bool_Weapons_28_or_29_beastProjectilesFired;//For beast weapons 28 and 29. Allows immediate shooting for them.
    public static bool bool_beastModeActive, bool_weaponStrengthArrayPopulated,
        bool_fastFiringActive;
    
    private float float_timeLastFiredShot, float_rankUpgradeTime,
        float_beastModeActivationTime;
    public float[] fireRates;

    private int int_temporaryWeaponValueHolder, int_firingSpeedModifier = 1;
    //Make this private after testing
    public static int int_beastModesAvailable = 25, int_ranksAvailable = 25, int_weaponIndex_player1, //!!!!Change beast modes and ranks available to 0!!!!!!
        int_currentLevel, int_weaponValue_player1;
    public static int[] array_int_weaponStrength;

//GameObjects
    private Animator animator_gunfireSide, animator_gunfireUnder,
        animator_gunfireBack, animator_flameThrower,
        animator_SylvesterFace, animator_SylvesterStatus;
   
    public AudioClip[] audioClips_gunfireSounds;
    public AudioClip audioClip_beastMode, audioClip_beastModeWeak,
        audioClip_rankUp, audioClip_cyclePrevious,
        audioClip_cycleNext;
    
    private AudioSource audioSource_gunfireSpeaker, audioSource_flameThrower,
        audioSource_weaponIndicator, audioSource_SylvesterFace;
    
    private Collider2D collider2D_ship;

    public GameObject gameObject_gunfireSide, gameObject_gunfireUnder,
        gameObject_gunfireBack, gameObject_flameThrower;
    private GameObject gameObject_player1HUD, gameObject_weaponIndicator,
        gameObject_SylvesterFace,
        gameObject_player1Projectile_1, gameObject_player1Projectile_2, gameObject_player1Projectile_3,
        gameObject_player1Projectile_4, gameObject_player1Projectile_5, gameObject_player1Projectile_6,
        gameObject_player1Projectile_7, gameObject_player1Projectile_8; //Needed for Beast Mode influence
    
    private SpriteRenderer spriteRenderer_gunfireSide, spriteRenderer_gunfireUnder,
        spriteRenderer_gunfireBack;
    
    private Transform transform_player1Ship, transform_gunfireSide,
        transform_gunfireUnder, transform_gunfireBack;

    public static KeyCode 
        keyCode_chargeShot = KeyCode.LeftShift,
        keyCode_fireWeapon = KeyCode.Space,
        keyCode_rankUp = KeyCode.V,
        keyCode_beastMode = KeyCode.R,
        keyCode_changeSpeed = KeyCode.Tab,
        keyCode_cycleWeaponPrevious = KeyCode.F,
        keyCode_cycleWeaponNext = KeyCode.G,
        keyCode_pauseGame = KeyCode.T,
        keyCode_moveUp = KeyCode.W,
        keyCode_moveDown = KeyCode.A,
        keyCode_moveLeft = KeyCode.S,
        keyCode_moveRight = KeyCode.D;

    private Vector3 vector3_melee_3_spawnDifference, vector3_melee_5_spawnDifference;

    //Scripts
    private SelectWeapon_Player1 script_SelectWeapon_Player1;

    private void Awake()
    {
        /*Using a prefab will change the reference itself to what's in the folders,
         NOT the gameObject in the scene. That being said, since there is no
        connection to the HUD from the player ship, it must be searched for.*/
        gameObject_player1HUD = GameObject.Find("Player1_HUD");

        collider2D_ship = gameObject.GetComponent<Collider2D>();

        //Ship Object values set here
        audioSource_gunfireSpeaker = gameObject.GetComponent<AudioSource>();
        
        //gunfireSide Object values set here
        animator_gunfireSide = gameObject_gunfireSide.GetComponent<Animator>();
        spriteRenderer_gunfireSide = gameObject_gunfireSide.GetComponent<SpriteRenderer>();
        transform_gunfireSide = gameObject_gunfireSide.GetComponent<Transform>();
        
        //gunfireUnder Object values set here
        animator_gunfireUnder = gameObject_gunfireUnder.GetComponent<Animator>();
        spriteRenderer_gunfireUnder = gameObject_gunfireUnder.GetComponent<SpriteRenderer>();
        transform_gunfireUnder = gameObject_gunfireUnder.GetComponent<Transform>();

        //gunfireBack Object values set here
        animator_gunfireBack = gameObject_gunfireBack.GetComponent<Animator>();
        spriteRenderer_gunfireBack = gameObject_gunfireBack.GetComponent<SpriteRenderer>();
        transform_gunfireBack = gameObject_gunfireBack.GetComponent<Transform>();

        //FlameThrower GameObject here
        animator_flameThrower = gameObject_flameThrower.GetComponent<Animator>();
        audioSource_flameThrower = gameObject_flameThrower.GetComponent<AudioSource>();

        //WeaponIndicator values set here
        //Obtain script to call method to animate weapon change

        script_SelectWeapon_Player1 = gameObject_player1HUD.GetComponentInChildren<SelectWeapon_Player1>();
        audioSource_weaponIndicator = script_SelectWeapon_Player1.gameObject.GetComponent<AudioSource>();
        
        //Sylvester's facial expressions are made accessible here
        gameObject_SylvesterFace = GameObject.Find("SylvesterFace");
        animator_SylvesterFace = gameObject_SylvesterFace.GetComponent<Animator>();
        audioSource_SylvesterFace = gameObject_SylvesterFace.GetComponent<AudioSource>();

        //Sylvester's background status effect is obtained here
        animator_SylvesterStatus = GameObject.Find("Status").GetComponent<Animator>();
        }

    private void Start() 
    {
       // int_beastModesAvailable = 0; //Logic should be placed here for carry-over beast modes (Easy difficulty)
        bool_weaponStrengthArrayPopulated = false;
        bool_fastFiringActive = false;
        float_timeLastFiredShot = Time.time;
        float_beastModeActivationTime = 500000;
        bool_Weapons_28_or_29_beastProjectilesFired = true;
        spriteRenderer_gunfireSide.enabled = false;
        spriteRenderer_gunfireUnder.enabled = false;
        spriteRenderer_gunfireBack.enabled = false;
        bool_energy4_rightAngle = true; //Determines if projectile_energy_4 is fired non-diagonal
        int_weaponIndex_player1 = 0;
        int_currentLevel = SceneManager.GetActiveScene().buildIndex;
        vector3_melee_3_spawnDifference = new Vector3(-1.45f, 0, 0);
        vector3_melee_5_spawnDifference = new Vector3(-1.45f, 0.25f, 0);
        Health_Player1.bool_enemyGameObjectsDealDamage = true;
        switch (int_currentLevel)
        {
            case 1: //Level 1, Let It Roar
            {
                array_int_weaponStrength = new int[5] {0,0,0,0,0};
                break;
            }
            case 2: //Level 2, The Fang Shards
            {
                array_int_weaponStrength = new int[5] {0,1,0,0,0};
                break;
            }
            case 3: //Level 3, Poseidon
            {
                array_int_weaponStrength = new int[5] {1,1,0,0,0};
                break;
            }
            case 4: //Level 4, Drowning
            {
                array_int_weaponStrength = new int[5] {1,1,1,0,0};
                break;
            }
            case 6: //Level 5, Cave Busting
            {
                array_int_weaponStrength = new int[5] {1,1,1,1,0};
                break;
            }
            case 7: //Level 6, Shiver
            {
                array_int_weaponStrength = new int[5] {1,1,1,1,1};
                break;
            }
            case 8: //Level 7, The Chase
            {
                array_int_weaponStrength = new int[5] {2,1,1,1,1};
                break;
            }
            case 9: //Level 8, Derelict
            {
                array_int_weaponStrength = new int[5] {2,1,2,1,1};
                break;
            }
            case 11: //Level 9, Fruitless
            {
                array_int_weaponStrength = new int[5] {2,1,2,2,1};
                break;
            }
            case 12: //Level 10, Graduation
            {
                array_int_weaponStrength = new int[5] {2,2,2,2,1};
                break;
            }
            case 14: //Level 11, The Amazon
            {
                array_int_weaponStrength = new int[5] {2,2,2,2,2};
                break;
            }
            default:
            {
                array_int_weaponStrength = new int[5] {0,0,0,0,0};
                break;
            }
        }

        bool_weaponStrengthArrayPopulated = true;
        int_weaponValue_player1 = int_weaponIndex_player1 + (5 * array_int_weaponStrength[int_weaponIndex_player1]);
    }

    private void Update()
    {
        if(!bool_beastModeActive) //Constantly check weapon strength array, update weapon value accordingly
        {
            int_weaponValue_player1 = int_weaponIndex_player1 + (5 * array_int_weaponStrength[int_weaponIndex_player1]);
        }

        if(bool_fastFiringActive
        && int_firingSpeedModifier != 2)
        {
            int_firingSpeedModifier = 2;
        }
        else if(!bool_fastFiringActive
        && int_firingSpeedModifier != 1)
        {
            int_firingSpeedModifier = 1;
        }

        if(!HUD_Player1.bool_rankUpRequest_player1)
        {
            bool_rankUpgradeInProgress = false;
        }

#if UNITY_STANDALONE
        if (Input.GetKey(keyCode_fireWeapon)
        || (Input.GetMouseButton(0))
        && !GameProperties.bool_isGamePaused
        && int_weaponIndex_player1 != 1 //No tail weapon being used means it's memory-safe to increase the fire rate.
        && bool_Weapons_28_or_29_beastProjectilesFired) //Player has not shot with ExplosiveBeast nor EnergyBeast
        {
            if(calculateTimePassedAfterShot() >= (fireRates[int_weaponValue_player1] / int_firingSpeedModifier)) 
            {
                float_timeLastFiredShot = Time.time;
                fireBaseWeapons();
            }
        }

        else if (Input.GetKey(keyCode_fireWeapon)
        || (Input.GetMouseButton(0))
        && !GameProperties.bool_isGamePaused
        && int_weaponIndex_player1 == 1)  //Tail weapons being used means more objects to spawn at a time. 
        {
            if (calculateTimePassedAfterShot() >= (fireRates[int_weaponValue_player1])) //Fire rate will not be modified.
            {
                float_timeLastFiredShot = Time.time;
                fireBaseWeapons();
            }
        }

        else if (Input.GetKey(keyCode_fireWeapon)
        || (Input.GetMouseButton(0))
        && (ProjectileControls_Player1.int_weaponValue_player1 == 28
            || ProjectileControls_Player1.int_weaponValue_player1 == 29)
        && !GameProperties.bool_isGamePaused)
        {
            if (!bool_Weapons_28_or_29_beastProjectilesFired)
            {
                bool_Weapons_28_or_29_beastProjectilesFired = true; //Explosive Beast or Energy Beast projectile can spawn immediately
                float_timeLastFiredShot = Time.time;
                fireBaseWeapons();
            }
            
            
        }

        if (Input.GetKeyUp(keyCode_fireWeapon)
        || (Input.GetMouseButtonUp(0)))
        {
            spriteRenderer_gunfireSide.enabled = false;
            spriteRenderer_gunfireUnder.enabled = false;
            spriteRenderer_gunfireBack.enabled = false;
            animator_gunfireSide.SetInteger("Type", 0);
            animator_gunfireUnder.SetInteger("Type", 0);
            animator_gunfireBack.SetInteger("Type", 0);
            animator_flameThrower.SetInteger("Type", 0);
            if (audioSource_flameThrower.isPlaying)
            {
                audioSource_flameThrower.Stop();
            }
        }

        if(Input.GetKeyDown(keyCode_cycleWeaponPrevious)
        && !bool_weaponSlotReassigning
        && !bool_beastModeActive)
        {
            bool_weaponSlotReassigning = true;

            if(int_weaponIndex_player1 == 0)
            {
                int_weaponIndex_player1 = 4;
            }
            else
            {
                --int_weaponIndex_player1;
            }
            script_SelectWeapon_Player1.animateWeaponIndicator(int_weaponIndex_player1);
            audioSource_weaponIndicator.PlayOneShot(audioClip_cyclePrevious);
        }

        if(Input.GetKeyDown(keyCode_cycleWeaponNext)
        && !bool_weaponSlotReassigning
        && !bool_beastModeActive)
        {
            bool_weaponSlotReassigning = true;

            if(int_weaponIndex_player1 == 4)
            {
                int_weaponIndex_player1 = 0;
            }
            else
            {
                ++int_weaponIndex_player1;
            }
            audioSource_weaponIndicator.PlayOneShot(audioClip_cycleNext);
            script_SelectWeapon_Player1.animateWeaponIndicator(int_weaponIndex_player1);
        }

        if(Input.GetKeyUp(keyCode_cycleWeaponNext)
        || Input.GetKeyUp(keyCode_cycleWeaponPrevious))
        {
            bool_weaponSlotReassigning = false;
        }

        if(Input.GetKeyDown(keyCode_rankUp)
        && int_ranksAvailable > 0
        && !bool_beastModeActive
        && !bool_rankUpgradeInProgress
        && int_weaponValue_player1 < 20
        && Time.time > float_rankUpgradeTime)
        {
            bool_rankUpgradeInProgress = true;
            rankUp();   
        }

        if (Input.GetKeyDown(keyCode_beastMode)
        && !bool_beastModeActive
        && !bool_rankUpgradeInProgress
        && int_beastModesAvailable > 0
        && Health_Player1.int_life >=0)
        {
            FaceAnimation_Player1.bool_dialogueAudioInterrupted = true;
            FaceAnimation_Player1.int_currentAnimationState = FaceAnimation_Player1.expressionIndex;
            bool_beastModeActive = true;
            StartCoroutine(beastModeOn());
        }
#endif

        if (Time.time - float_beastModeActivationTime >= 7.0f)
        {
            beastModeCoolDown();
        }
    }

    /// <summary>
    /// Used for allowing weapons to fire after a context-specific delay.
    /// </summary>
    /// <returns></returns>
    private float calculateTimePassedAfterShot()
    {
        return (Time.time - float_timeLastFiredShot);
    }

    /// <summary>
    /// Determines which weapon(s) to fire based on int_weaponValue_player1, then spawns the appropriate 
    /// projectiles.
    /// </summary>
    private void fireBaseWeapons()
    {
        animateGunfire();
        if (int_weaponValue_player1 != 4)
        {
            if (audioSource_flameThrower.isPlaying)
            {
                audioSource_flameThrower.Stop();
            }
            if(int_weaponIndex_player1 == 1
            && int_firingSpeedModifier == 2)
            {
                switch(int_weaponValue_player1)
                {
                    case 1:
                    {
                        audioSource_gunfireSpeaker.PlayOneShot(
                            audioClips_gunfireSounds[30]);
                        break;
                    }
                    case 6:
                    {
                        audioSource_gunfireSpeaker.PlayOneShot(
                            audioClips_gunfireSounds[31]);
                        break;
                    }
                    case 11:
                    {
                        audioSource_gunfireSpeaker.PlayOneShot(
                            audioClips_gunfireSounds[32]);
                        break;
                    }
                    case 16:
                    {
                        audioSource_gunfireSpeaker.PlayOneShot(
                            audioClips_gunfireSounds[33]);
                        break;
                    }
                    case 21:
                    {
                        audioSource_gunfireSpeaker.PlayOneShot(
                            audioClips_gunfireSounds[34]);
                        break;
                    }
                    case 26:
                    {
                        audioSource_gunfireSpeaker.PlayOneShot(
                            audioClips_gunfireSounds[35]);
                        break;
                    }
                }
            }
            else
            {
                audioSource_gunfireSpeaker.PlayOneShot(
            audioClips_gunfireSounds[int_weaponValue_player1]);

            }
        }
        else if (!audioSource_flameThrower.isPlaying)
        {
            audioSource_flameThrower.PlayOneShot(audioSource_flameThrower.clip);
        }
        switch (int_weaponValue_player1)
        {
            case 12: //Melee_3
            {
                GameObject projectile_shield = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                projectile_shield.transform.position = transform_gunfireSide.position + vector3_melee_3_spawnDifference;
                projectile_shield.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile_shield.SetActive(true);

                gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects(19);
                gameObject_player1Projectile_1.transform.position = new Vector3(
                    transform_gunfireSide.position.x + 1.33f,
                    transform_gunfireSide.position.y,
                    transform_gunfireSide.position.z);
                gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0);
                gameObject_player1Projectile_1.SetActive(true);

                break;
            }
            case 17: //Melee_4
            {
                GameObject projectile_shield = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                projectile_shield.transform.position = transform_gunfireSide.position + vector3_melee_3_spawnDifference;
                projectile_shield.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile_shield.SetActive(true);

                GameObject projectile_side = ObjectPool.objectPool_reference.getPooled_PlayerObjects(15);
                projectile_side.transform.position = new Vector3(
                    transform_gunfireSide.position.x + 1.33f,
                    transform_gunfireSide.position.y,
                    transform_gunfireSide.position.z);
                projectile_side.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile_side.SetActive(true);

                GameObject projectile_tailLower = ObjectPool.objectPool_reference.getPooled_PlayerObjects(16);
                projectile_tailLower.transform.position = new Vector3(
                    transform_gunfireBack.position.x + -1.064f,
                    transform_gunfireBack.position.y + -0.503f,
                    transform_gunfireBack.position.z);
                projectile_tailLower.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile_tailLower.SetActive(true);

                break;
            }
            case 22: //Melee_5
            {
                GameObject projectile_shield = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                projectile_shield.transform.position = transform_gunfireSide.position + vector3_melee_3_spawnDifference;
                projectile_shield.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile_shield.SetActive(true);

                GameObject projectile_side = ObjectPool.objectPool_reference.getPooled_PlayerObjects(12);
                projectile_side.transform.position = new Vector3(
                    transform_gunfireSide.position.x + 1.33f,
                    transform_gunfireSide.position.y,
                    transform_gunfireSide.position.z);
                projectile_side.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile_side.SetActive(true);

                GameObject projectile_tailLower = ObjectPool.objectPool_reference.getPooled_PlayerObjects(13);
                projectile_tailLower.transform.position = new Vector3(
                    transform_gunfireBack.position.x + -1.064f,
                    transform_gunfireBack.position.y + -0.503f,
                    transform_gunfireBack.position.z);
                projectile_tailLower.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile_tailLower.SetActive(true);

                GameObject projectile_tailUpper = ObjectPool.objectPool_reference.getPooled_PlayerObjects(14);
                projectile_tailUpper.transform.position = new Vector3(
                    transform_gunfireBack.position.x + -1.064f,
                    transform_gunfireBack.position.y + 0.42f,
                    transform_gunfireBack.position.z);
                projectile_tailUpper.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile_tailUpper.SetActive(true);

                break;
            }
            case 27: //Melee_Beast
            {
                GameObject projectile_shield = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                projectile_shield.transform.position = transform_gunfireSide.position + vector3_melee_5_spawnDifference;
                projectile_shield.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile_shield.SetActive(true);

                GameObject projectile_side = ObjectPool.objectPool_reference.getPooled_PlayerObjects(10);
                projectile_side.transform.position = new Vector3(
                    transform_gunfireSide.position.x + 1.33f,
                    transform_gunfireSide.position.y,
                    transform_gunfireSide.position.z);
                projectile_side.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile_side.SetActive(true);

                GameObject projectile_tailLower = ObjectPool.objectPool_reference.getPooled_PlayerObjects(11);
                projectile_tailLower.transform.position = new Vector3(
                    transform_gunfireBack.position.x + -1.064f,
                    transform_gunfireBack.position.y + -0.503f,
                    transform_gunfireBack.position.z);
                projectile_tailLower.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile_tailLower.SetActive(true);

                GameObject projectile_tailUpper = ObjectPool.objectPool_reference.getPooled_PlayerObjects(12);
                projectile_tailUpper.transform.position = new Vector3(
                    transform_gunfireBack.position.x + -1.064f,
                    transform_gunfireBack.position.y + 0.42f,
                    transform_gunfireBack.position.z);
                projectile_tailUpper.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile_tailUpper.SetActive(true);

                break;
            }

            
            case 3:
            case 8:
            case 13:
            {
                gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                gameObject_player1Projectile_1.transform.position = transform_gunfireUnder.position;
                gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0);
                gameObject_player1Projectile_1.SetActive(true);

                break;
            }
            case 18:
            case 23:
            {
                gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                gameObject_player1Projectile_1.transform.position = transform_gunfireUnder.position;
                gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0);
                gameObject_player1Projectile_1.SetActive(true);

                if (Input.GetAxis("Vertical") > 0
                ||  Input.GetKey(ProjectileControls_Player1.keyCode_moveUp))
                {
                    gameObject_player1Projectile_2 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                    gameObject_player1Projectile_2.transform.position = transform_gunfireUnder.position;
                    gameObject_player1Projectile_2.transform.rotation = Quaternion.Euler(0, 0, 45);
                    gameObject_player1Projectile_2.SetActive(true);
                }
                else if (Input.GetAxis("Vertical") < 0
                || Input.GetKey(ProjectileControls_Player1.keyCode_moveDown))
                {
                    gameObject_player1Projectile_2 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                    gameObject_player1Projectile_2.transform.position = transform_gunfireUnder.position;
                    gameObject_player1Projectile_2.transform.rotation = Quaternion.Euler(0, 0, -45);
                    gameObject_player1Projectile_2.SetActive(true);
                }

                break;
            }

            /*Energy_1 (Flamethrower) doesn't have a gameObject, only an
            animation. */
            case 4:
            {
                animator_flameThrower.SetInteger("Type", 1);
                break;
            }

            case 19:
            {
                //Alternates anglur path of projectiles (a '+' shape, then an 'x' shape, repeat pattern)
                if (bool_energy4_rightAngle)
                {
                    gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                    gameObject_player1Projectile_1.transform.position = transform_gunfireSide.position;
                    gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0);
                    gameObject_player1Projectile_1.SetActive(true);

                    gameObject_player1Projectile_2 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                    gameObject_player1Projectile_2.transform.position = transform_gunfireSide.position;
                    gameObject_player1Projectile_2.transform.rotation = Quaternion.Euler(0, 0, 90);
                    gameObject_player1Projectile_2.SetActive(true);

                    gameObject_player1Projectile_3 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                    gameObject_player1Projectile_3.transform.position = transform_gunfireSide.position;
                    gameObject_player1Projectile_3.transform.rotation = Quaternion.Euler(0, 0, 180);
                    gameObject_player1Projectile_3.SetActive(true);

                    gameObject_player1Projectile_4 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                    gameObject_player1Projectile_4.transform.position = transform_gunfireSide.position;
                    gameObject_player1Projectile_4.transform.rotation = Quaternion.Euler(0, 0, 270);
                    gameObject_player1Projectile_4.SetActive(true);

                    bool_energy4_rightAngle = false;

                    break;
                }
                else
                {
                    gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                    gameObject_player1Projectile_1.transform.position = transform_gunfireSide.position;
                    gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 45);
                    gameObject_player1Projectile_1.SetActive(true);

                    gameObject_player1Projectile_2 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                    gameObject_player1Projectile_2.transform.position = transform_gunfireSide.position;
                    gameObject_player1Projectile_2.transform.rotation = Quaternion.Euler(0, 0, 135);
                    gameObject_player1Projectile_2.SetActive(true);

                    gameObject_player1Projectile_3 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                    gameObject_player1Projectile_3.transform.position = transform_gunfireSide.position;
                    gameObject_player1Projectile_3.transform.rotation = Quaternion.Euler(0, 0, 225);
                    gameObject_player1Projectile_3.SetActive(true);

                    gameObject_player1Projectile_4 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                    gameObject_player1Projectile_4.transform.position = transform_gunfireSide.position;
                    gameObject_player1Projectile_4.transform.rotation = Quaternion.Euler(0, 0, 315);
                    gameObject_player1Projectile_4.SetActive(true);

                    bool_energy4_rightAngle = true;

                    break;
                }
            }
            case 29:
            {
                gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                gameObject_player1Projectile_1.transform.position = new Vector3(-63.8f, 1.8f, -1.01f);
                gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0);
                gameObject_player1Projectile_1.SetActive(true);
                break;
            }

            case 20:
            {
                gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                gameObject_player1Projectile_1.transform.position = transform_gunfireSide.position + new Vector3(0.5f, 0, 0);
                gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0);
                gameObject_player1Projectile_1.SetActive(true);
                break;
            }
            case 25:
            {
                gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                gameObject_player1Projectile_1.transform.position = transform_gunfireSide.position;
                gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0);
                gameObject_player1Projectile_1.SetActive(true);

                GameObject object_gunfireEffect = ObjectPool.objectPool_reference.getPooled_PlayerObjects(5);
                object_gunfireEffect.transform.position = new Vector3(
                    gameObject.transform.position.x + 4,
                    gameObject.transform.position.y,
                    gameObject.transform.position.z);
                object_gunfireEffect.transform.rotation = Quaternion.Euler(0, 0, 0);
                object_gunfireEffect.SetActive(true);

                break;
            }

            case 1:
            case 6:
            case 11:
            case 16:
            case 21:
            {
                if (int_firingSpeedModifier == 1)
                {
                    this.gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                    if (gameObject_player1Projectile_1 != null)
                    {
                        gameObject_player1Projectile_1.transform.position = transform_gunfireSide.position;
                        gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0);
                        gameObject_player1Projectile_1.SetActive(true);
                    }
                }
                
                else if (int_firingSpeedModifier == 2)
                {
                    switch(int_weaponValue_player1)
                    {
                        case 1:
                        {
                            this.gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects(55, "TailWeapon");
                            break;
                        }
                        case 6:
                        {
                            this.gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects(56, "TailWeapon");
                            break;
                        }
                        case 11:
                        {
                            this.gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects(57, "TailWeapon");
                            break;
                        }
                        case 16:
                        {
                            this.gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects(58, "TailWeapon");
                            break;
                        }
                        case 21:
                        {
                            this.gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects(59, "TailWeapon");
                            break;
                        }
                    }
                    
                    if (gameObject_player1Projectile_1 != null)
                    {
                        gameObject_player1Projectile_1.transform.position = transform_gunfireSide.position;
                        gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0);
                        gameObject_player1Projectile_1.SetActive(true);
                    }
                }

                break;
            }
            case 26:
            {
                if (int_firingSpeedModifier == 1)
                {
                    if (!bool_tail5_subAngles)
                    {
                        this.gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects(34, default);
                        if (gameObject_player1Projectile_1 != null)
                        {
                            gameObject_player1Projectile_1.transform.position = transform_gunfireSide.position;
                            gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0);
                            gameObject_player1Projectile_1.SetActive(true);
                        }

                        bool_tail5_subAngles = true;
                    }
                    else
                    {
                        gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects(35, default);
                        if (gameObject_player1Projectile_1 != null)
                        {
                            gameObject_player1Projectile_1.transform.position = transform_gunfireSide.position;
                            gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0f);
                            gameObject_player1Projectile_1.SetActive(true);
                        }


                        bool_tail5_subAngles = false;
                    }
                }
                
                else if(int_firingSpeedModifier == 2)
                {
                    if (!bool_tail5_subAngles)
                    {
                        this.gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects(36, default);
                        if (gameObject_player1Projectile_1 != null)
                        {
                            gameObject_player1Projectile_1.transform.position = transform_gunfireSide.position;
                            gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0);
                            gameObject_player1Projectile_1.SetActive(true);
                        }

                        bool_tail5_subAngles = true;
                    }
                    else
                    {
                        gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects(37, default);
                        if (gameObject_player1Projectile_1 != null)
                        {
                            gameObject_player1Projectile_1.transform.position = transform_gunfireSide.position;
                            gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0f);
                            gameObject_player1Projectile_1.SetActive(true);
                        }


                        bool_tail5_subAngles = false;
                    }
                }

                break;
            }

            case 28:
            {
                gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                GameObject projectileCollider_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects(12);
                GameObject projectileCollider_2 = ObjectPool.objectPool_reference.getPooled_PlayerObjects(13);
                GameObject projectileCollider_3 = ObjectPool.objectPool_reference.getPooled_PlayerObjects(14);

                gameObject_player1Projectile_1.transform.position = this.gameObject.transform.position;
                projectileCollider_1.transform.position = new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y - 23.0f,
                    this.gameObject.transform.position.z);
                projectileCollider_2.transform.position = new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y - 33.0f,
                    this.gameObject.transform.position.z);
                projectileCollider_3.transform.position = new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y - 43.0f,
                    this.gameObject.transform.position.z);

                gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectileCollider_1.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectileCollider_2.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectileCollider_3.transform.rotation = Quaternion.Euler(0, 0, 0);
                gameObject_player1Projectile_1.SetActive(true);
                projectileCollider_1.SetActive(true);
                projectileCollider_2.SetActive(true);
                projectileCollider_3.SetActive(true);
                break;
            }

            default:
            {
                gameObject_player1Projectile_1 = ObjectPool.objectPool_reference.getPooled_PlayerObjects();
                gameObject_player1Projectile_1.transform.position = transform_gunfireSide.position;
                gameObject_player1Projectile_1.transform.rotation = Quaternion.Euler(0, 0, 0);
                gameObject_player1Projectile_1.SetActive(true);
                break;
            }
        }
    }
   
    private void animateGunfire()
    {
        switch(int_weaponValue_player1)
        {
            //Tail Gunfire
            case 1:
            case 6:
            case 11:
            {
                spriteRenderer_gunfireBack.enabled = true;
                spriteRenderer_gunfireSide.enabled = true;
                spriteRenderer_gunfireUnder.enabled = false;
                animator_gunfireSide.SetInteger("Type", 1);
                animator_gunfireBack.SetInteger("Type", 1);
                animator_gunfireUnder.SetInteger("Type", 0);
                animator_flameThrower.SetInteger("Type", 0);
                break;
            }
            //Blue Gunfire
            case 16:
            case 21:
            {
                spriteRenderer_gunfireBack.enabled = true;
                spriteRenderer_gunfireSide.enabled = true;
                spriteRenderer_gunfireUnder.enabled = false;
                animator_gunfireSide.SetInteger("Type", 7);
                animator_gunfireBack.SetInteger("Type", 7);
                animator_gunfireUnder.SetInteger("Type", 0);
                animator_flameThrower.SetInteger("Type", 0);
                break;
            }
            //Larger Red Gunfire
            case 10:
            {
                spriteRenderer_gunfireSide.enabled = true;
                spriteRenderer_gunfireBack.enabled = false;
                spriteRenderer_gunfireUnder.enabled = false;
                animator_gunfireSide.SetInteger("Type", 2);
                animator_gunfireBack.SetInteger("Type", 0);
                animator_gunfireUnder.SetInteger("Type", 0);
                animator_flameThrower.SetInteger("Type", 0);
                break;
            }
            //Smaller Red Gunfire
            case 3:
            case 8:
            case 13:
            case 18:
            case 23:
            {
                spriteRenderer_gunfireUnder.enabled = true;
                spriteRenderer_gunfireBack.enabled = false;
                spriteRenderer_gunfireUnder.enabled = false;
                animator_gunfireSide.SetInteger("Type", 0);
                animator_gunfireBack.SetInteger("Type", 0);
                animator_gunfireUnder.SetInteger("Type", 6);
                animator_flameThrower.SetInteger("Type", 0);
                break;
            }
            //Yellow gunfire
             case 2:
             case 7:
             case 9:
             case 12:
             case 14:
                {
                spriteRenderer_gunfireSide.enabled = true;
                spriteRenderer_gunfireBack.enabled = false;
                spriteRenderer_gunfireUnder.enabled = false;
                animator_gunfireSide.SetInteger("Type", 3);
                animator_gunfireBack.SetInteger("Type", 0);
                animator_gunfireUnder.SetInteger("Type", 0);
                animator_flameThrower.SetInteger("Type", 0);
                break;
            }
             case 17:
             case 22:
            {
                spriteRenderer_gunfireSide.enabled = true;
                spriteRenderer_gunfireBack.enabled = true;
                spriteRenderer_gunfireUnder.enabled = false;
                animator_gunfireSide.SetInteger("Type", 3);
                animator_gunfireBack.SetInteger("Type", 3);
                animator_gunfireUnder.SetInteger("Type", 0);
                animator_flameThrower.SetInteger("Type", 0);
                break;
            }
            //Pink gunfire
            case 15:
            {
                spriteRenderer_gunfireSide.enabled = true;
                spriteRenderer_gunfireBack.enabled = false;
                spriteRenderer_gunfireUnder.enabled = false;
                animator_gunfireSide.SetInteger("Type", 4);
                animator_gunfireBack.SetInteger("Type", 0);
                animator_gunfireUnder.SetInteger("Type", 0);
                animator_flameThrower.SetInteger("Type", 0);
                break;
            }
            //Ballistic5 gunfire
            case 20:
            {
                spriteRenderer_gunfireSide.enabled = true;
                spriteRenderer_gunfireBack.enabled = false;
                spriteRenderer_gunfireUnder.enabled = false;
                animator_gunfireSide.SetInteger("Type", 5);
                animator_gunfireBack.SetInteger("Type", 0);
                animator_gunfireUnder.SetInteger("Type", 0);
                animator_flameThrower.SetInteger("Type", 0);
                break;
            }
            //Beast gunfire 
            case 25:
            {
                spriteRenderer_gunfireSide.enabled = true;
                animator_gunfireSide.SetInteger("Type", 8);
                switch(PlayerMovement.float_shipSpeed)
                    {
                        case 6:
                        {
                            gameObject.transform.position = new Vector3(
                                gameObject.transform.position.x - 1.8f,
                                gameObject.transform.position.y,
                                gameObject.transform.position.z);
                            break;
                        }
                        case 11:
                        {
                            gameObject.transform.position = new Vector3(
                                gameObject.transform.position.x - 3.0f,
                                gameObject.transform.position.y,
                                gameObject.transform.position.z);
                            break;
                        }
                        case 16:
                        {
                            gameObject.transform.position = new Vector3(
                                gameObject.transform.position.x - 4.4f,
                                gameObject.transform.position.y,
                                gameObject.transform.position.z);
                            break;
                        }
                        case 21:
                        {
                            gameObject.transform.position = new Vector3(
                                gameObject.transform.position.x - 5.8f,
                                gameObject.transform.position.y,
                                gameObject.transform.position.z);
                            break;
                        }
                        default:
                        {
                            gameObject.transform.position = new Vector3(
                                gameObject.transform.position.x - 4.0f,
                                gameObject.transform.position.y,
                                gameObject.transform.position.z);
                            break;
                        }
                    }
                    
                break;
            }
            //Flamethrower
            case 4:
            {
                spriteRenderer_gunfireUnder.enabled = false;
                spriteRenderer_gunfireSide.enabled = false;
                spriteRenderer_gunfireBack.enabled = false;
                animator_gunfireUnder.SetInteger("Type", 0);
                animator_gunfireSide.SetInteger("Type", 0);
                animator_gunfireBack.SetInteger("Type", 0);
                animator_flameThrower.SetInteger("Type", 8);
                break;
            }
            //Flame for gunfire
            default:
            {
                spriteRenderer_gunfireSide.enabled = true;
                animator_gunfireSide.SetInteger("Type", 1);
                animator_gunfireBack.SetInteger("Type", 0);
                animator_gunfireUnder.SetInteger("Type", 0);
                animator_flameThrower.SetInteger("Type", 0);
                break;
            }
        }
    }

    /// <summary>
    /// Upgrade player1's currently selected weapon
    /// </summary>
    private void rankUp()
    {
        float_rankUpgradeTime = Time.time + 0.25f;
        array_int_weaponStrength[int_weaponIndex_player1] += 1;
        int_ranksAvailable -= 1;
        audioSource_weaponIndicator.PlayOneShot(audioClip_rankUp);
        int_weaponValue_player1 = int_weaponIndex_player1 + (5 * array_int_weaponStrength[int_weaponIndex_player1]);
        HUD_Player1.bool_rankUpRequest_player1 = true;

    }

    IEnumerator beastModeOn()
    {
        FaceAnimation_Player1.bool_beastMode_FaceCooldown = true;
        FaceAnimation_Player1.float_timePlayer1HaltedAnimation = Time.time;
        if(Health_Player1.int_life > 50)
        {
            audioSource_SylvesterFace.PlayOneShot(audioClip_beastMode);
        }
        else
        {
            audioSource_SylvesterFace.PlayOneShot(audioClip_beastModeWeak);
        }

        float_timeLastFiredShot = 0; //Allows immediate shooting
        Health_Player1.bool_enemyGameObjectsDealDamage = false; //Invincibility
        float_beastModeActivationTime = Time.time; //Limited time
        animator_SylvesterFace.SetInteger("Expression", (int)E_FaceExpressions.BeastMode); //Animate Sylvester's face
        StatusScript_Player1.animator_faceBackground.SetInteger("Status", (int)E_StatusAnimationStates.BeastMode); //Animate Sylvester's background

        yield return new WaitForSeconds(1.2f);
         int_beastModesAvailable -= 1;
        int_temporaryWeaponValueHolder = int_weaponValue_player1; //Save weapon data
        int_weaponValue_player1 = int_weaponIndex_player1 + 25; //Activate super weapon
        if (int_weaponValue_player1 == 28 || int_weaponValue_player1 == 29)
        {
            bool_Weapons_28_or_29_beastProjectilesFired = false;
        }
        //Health bonus
        switch (GameProperties.DataManagement.GameData.string_currentDifficulty)
        {
            case "Easy":
            {
                if (Health_Player1.int_life == 150)
                {
                    break;
                }
                else
                {
                    Health_Player1.int_life += 50;
                    Health_Player1.bool_criticalStatus = false;
                }
                break;
            }
            case "Normal":
            {
                if (Health_Player1.int_life == 100)
                {
                    break;
                }
                else
                {
                    Health_Player1.int_life += 50;
                    Health_Player1.bool_criticalStatus = false;
                }
                break;
            }
            case "Arcade":
            {
                if (Health_Player1.int_life == 50)
                {
                    break;
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
                    break;
                }
                else
                {
                    Health_Player1.int_life += 50;
                    Health_Player1.bool_criticalStatus = false;
                }
                break;
            }
        }
        HUD_Player1.bool_beastModeRequest_player1 = true; //Animate the appropriate Weapon Icon
    }

    private void beastModeCoolDown()
    {
        Health_Player1.bool_enemyGameObjectsDealDamage = true; //Vulnerability
        float_beastModeActivationTime = 500000; //Time reset
        bool_beastModeActive = false; //Enable upgrades and weapon switches
        int_weaponValue_player1 = int_temporaryWeaponValueHolder; //Restore weapons
        animator_SylvesterFace.SetInteger("Expression", ((int)E_FaceExpressions.Idle)); //Animate Sylvester's face
        HUD_Player1.bool_restoreWeaponRequest = true; //Animate the appropriate Weapon Icon
        StatusScript_Player1.animator_faceBackground.SetInteger("Status", (int)E_StatusAnimationStates.Idle); //Animate Sylvester's background
        FaceAnimation_Player1.bool_beastMode_FaceCooldown = false;
        bool_beastModeActive = false;
    }

}
