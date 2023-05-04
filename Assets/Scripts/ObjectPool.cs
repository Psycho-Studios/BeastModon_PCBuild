using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPool : MonoBehaviour
{
    private int int_amountOfObjectsToPool;
    public static ObjectPool objectPool_reference = null;
    /// <summary>
    /// Array of GameObjects assigned in the inspector
    /// </summary>
    public List<GameObject> list_playerGameObjectsToPool,
        list_TestingScene_objectsToPool,
        list_Tutorial_objectsToPool,
        list_Level_1_objectsToPool,
        list_miscellaneousGameObjectsToPool;
    private List<List<GameObject>> list_spawnablePlayerGameObjects,
        list_spawnableLevelSpecificGameObjects,
        list_spawnableMiscellaneousGameObjects;
    
        
    private void Awake()
    {
        objectPool_reference = this;
        list_spawnablePlayerGameObjects = new List<List<GameObject>>();
        list_spawnableLevelSpecificGameObjects = new List<List<GameObject>>();
        list_spawnableMiscellaneousGameObjects = new List<List<GameObject>>();

        int_amountOfObjectsToPool = 20; //60 is a decent number of projectiles to allow, no other reason

        //Provide a clone-reference to allow instantiation
        GameObject gameObject_cloneOfPlayerPooledObject, gameObject_cloneOfLevelSpecificObject;

        //Iterate through the inspector-assigned list of gameObjects to add lists of objectPools
        
        //Player-specific objects
        for (int gameObjectIndex = 0; gameObjectIndex < list_playerGameObjectsToPool.Count; gameObjectIndex++)
        {
            list_spawnablePlayerGameObjects.Add(new List<GameObject>()); //Every new list will correspond to the current gameObjectIndex

            list_spawnablePlayerGameObjects[gameObjectIndex] = new List<GameObject>(); //Individual gameObject list initialized

            if (list_playerGameObjectsToPool[gameObjectIndex] != null)
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    gameObject_cloneOfPlayerPooledObject = Instantiate(list_playerGameObjectsToPool[gameObjectIndex]);
                    gameObject_cloneOfPlayerPooledObject.SetActive(false);
                    list_spawnablePlayerGameObjects[gameObjectIndex].Add(gameObject_cloneOfPlayerPooledObject);
                }
            }
        }

        //Miscellaneous objects
        for (int gameObjectIndex = 0; gameObjectIndex < list_miscellaneousGameObjectsToPool.Count; gameObjectIndex++)
        {
            list_spawnableMiscellaneousGameObjects.Add(new List<GameObject>()); //Every new list will correspond to the current gameObjectIndex

            list_spawnableMiscellaneousGameObjects[gameObjectIndex] = new List<GameObject>(); //Individual gameObject list initialized

            if (list_miscellaneousGameObjectsToPool[gameObjectIndex] != null)
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    gameObject_cloneOfPlayerPooledObject = Instantiate(list_miscellaneousGameObjectsToPool[gameObjectIndex]);
                    gameObject_cloneOfPlayerPooledObject.SetActive(false);
                    list_spawnableMiscellaneousGameObjects[gameObjectIndex].Add(gameObject_cloneOfPlayerPooledObject);
                }
            }
        }

        //Level-specific objects
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 17: //Tutorial
            {
                for (int gameObjectIndex = 0; gameObjectIndex < list_Tutorial_objectsToPool.Count; gameObjectIndex++)
                {
                    list_spawnableLevelSpecificGameObjects.Add(new List<GameObject>()); //Every new list will correspond to the current gameObjectIndex

                    list_spawnableLevelSpecificGameObjects[gameObjectIndex] = new List<GameObject>(); //Individual gameObject list initialized

                    if (list_Tutorial_objectsToPool[gameObjectIndex] != null)
                    {
                        for (int i = 0; i < int_amountOfObjectsToPool; i++)
                        {
                            gameObject_cloneOfLevelSpecificObject = Instantiate(list_Tutorial_objectsToPool[gameObjectIndex]);
                            gameObject_cloneOfLevelSpecificObject.SetActive(false);
                            list_spawnableLevelSpecificGameObjects[gameObjectIndex].Add(gameObject_cloneOfLevelSpecificObject);
                        }
                    }
                }
                break;
            }

            case 1: //Level 1
            {
                for (int gameObjectIndex = 0; gameObjectIndex < list_Level_1_objectsToPool.Count; gameObjectIndex++)
                {
                    list_spawnableLevelSpecificGameObjects.Add(new List<GameObject>()); //Every new list will correspond to the current gameObjectIndex

                    list_spawnableLevelSpecificGameObjects[gameObjectIndex] = new List<GameObject>(); //Individual gameObject list initialized

                    if (list_Level_1_objectsToPool[gameObjectIndex] != null)
                    {
                        for (int i = 0; i < int_amountOfObjectsToPool; i++)
                        {
                            gameObject_cloneOfLevelSpecificObject = Instantiate(list_Level_1_objectsToPool[gameObjectIndex]);
                            gameObject_cloneOfLevelSpecificObject.SetActive(false);
                            list_spawnableLevelSpecificGameObjects[gameObjectIndex].Add(gameObject_cloneOfLevelSpecificObject);
                        }
                    }
                }
                break;
            }
            default: //Testing Scene
            {
                for (int gameObjectIndex = 0; gameObjectIndex < list_TestingScene_objectsToPool.Count; gameObjectIndex++)
                {
                    list_spawnableLevelSpecificGameObjects.Add(new List<GameObject>()); //Every new list will correspond to the current gameObjectIndex

                    list_spawnableLevelSpecificGameObjects[gameObjectIndex] = new List<GameObject>(); //Individual gameObject list initialized

                    if (list_TestingScene_objectsToPool[gameObjectIndex] != null)
                    {
                        for (int i = 0; i < int_amountOfObjectsToPool; i++)
                        {
                            gameObject_cloneOfLevelSpecificObject = Instantiate(list_TestingScene_objectsToPool[gameObjectIndex]);
                            gameObject_cloneOfLevelSpecificObject.SetActive(false);
                            list_spawnableLevelSpecificGameObjects[gameObjectIndex].Add(gameObject_cloneOfLevelSpecificObject);
                        }
                    }
                }
                break;
            }
        }

    }
       
    /// <summary>
    /// Gets gameObjects that oppose the player.
    /// </summary>
    /// <param name="int_objectIndex"></param>
    /// <returns></returns>
    public GameObject getPooled_LevelSpecificObjects(int int_objectIndex)
    {
        for (int i = 0; i < int_amountOfObjectsToPool; i++)
        {
            if (!list_spawnableLevelSpecificGameObjects[int_objectIndex] //List of targeted gameObject
                [i].activeInHierarchy) //If it's inactive, it's the one that is used.
            {
                return list_spawnableLevelSpecificGameObjects[int_objectIndex][i];
            }
        }
        return null;
    }
   
    /// <summary>
    /// Retrieves objects that are result of player input (bullets, damage effects, explosions, etc.)
    /// The weaponIndexModifier is specifically for selecting weapons, it is
    /// added to the WeaponValue provided.
    /// The objectType parameter is used for obtaining gameObjects that aren't
    /// dependent on the current WeaponValue.
    /// </summary>
    /// <param name="weaponIndexModifier"></param>
    /// <param name="objectType"></param>
    /// <returns></returns>
    public GameObject getPooled_PlayerObjects(int weaponIndexModifier = 0, string objectType = null) //Optional parameters
    {
        switch(objectType)
        {
            case "VisualEffect_EnemyDamage_Metal_Sides":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnablePlayerGameObjects[43][i].activeInHierarchy)
                    {
                        return list_spawnablePlayerGameObjects[43][i];
                    }
                }
                break;
            }
            case "VisualEffect_EnemyDamage_Metal_Under":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnablePlayerGameObjects[44][i].activeInHierarchy)
                    {
                        return list_spawnablePlayerGameObjects[44][i];
                    }
                }
                break;
            }
            case "VisualEffect_EnemyDamage_Flesh_Sides":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnablePlayerGameObjects[45][i].activeInHierarchy)
                    {
                        return list_spawnablePlayerGameObjects[45][i];
                    }
                }
                break;
            }
            case "VisualEffect_EnemyDamage_Flesh_Under":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnablePlayerGameObjects[46][i].activeInHierarchy)
                    {
                        return list_spawnablePlayerGameObjects[46][i];
                    }
                }
                break;
            }
            case "VisualEffect_Ballistic_Beast":
            case "VisualEffect_ChargedShot":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnablePlayerGameObjects[30][i].activeInHierarchy)
                    {
                        return list_spawnablePlayerGameObjects[30][i];
                    }
                }
                break;
            }
            case "Explosion_1":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnablePlayerGameObjects[50][i].activeInHierarchy)
                    {
                        return list_spawnablePlayerGameObjects[50][i];
                    }
                }
                break;
            }
            case "Explosion_2":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnablePlayerGameObjects[51][i].activeInHierarchy)
                    {
                        return list_spawnablePlayerGameObjects[51][i];
                    }
                }
                break;
            }
            case "Explosion_3":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnablePlayerGameObjects[52][i].activeInHierarchy)
                    {
                        return list_spawnablePlayerGameObjects[52][i];
                    }
                }
                break;
            }
            case "FloorSlam":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnablePlayerGameObjects[53][i].activeInHierarchy)
                    {
                        return list_spawnablePlayerGameObjects[53][i];
                    }
                }
                break;
            }
            case "ChargedShot":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnablePlayerGameObjects[54][i].activeInHierarchy)
                    {
                        return list_spawnablePlayerGameObjects[54][i];
                    }
                }
                break;
            }
            case "TailWeapon":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnablePlayerGameObjects[weaponIndexModifier] //List that holds the targeted gameObject
                        [i].activeInHierarchy) //If the object is inactive, it's the one that is used.
                    {
                        return list_spawnablePlayerGameObjects[weaponIndexModifier][i];
                    }
                }
                break;
            }
            default:
            {
                
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnablePlayerGameObjects[
                        ProjectileControls_Player1.int_weaponValue_player1 + weaponIndexModifier] //List of targeted gameObject
                        [i].activeInHierarchy) //If it's inactive, it's the one that is used.
                    {
                        return list_spawnablePlayerGameObjects[ProjectileControls_Player1.int_weaponValue_player1 + weaponIndexModifier][i];
                    }
                }
                
                break;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns powerups and other objects that benefit the player
    /// </summary>
    /// <param name="int_objectIndex"></param>
    /// <returns></returns>
    public GameObject getPooled_MiscellaneousObjects(int int_objectIndex, string objectName = null)
    {
        switch (objectName)
        {
            case "Explosion_1":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnableMiscellaneousGameObjects[Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Explosion_1)][i]
                        .activeInHierarchy)
                    {
                        return list_spawnableMiscellaneousGameObjects[Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Explosion_1)][i];
                    }
                }
                break;
            }
            case "Explosion_2":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnableMiscellaneousGameObjects[Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Explosion_2)][i]
                        .activeInHierarchy)
                    {
                        return list_spawnableMiscellaneousGameObjects[Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Explosion_2)][i];
                    }
                }
                break;
            }
            case "Explosion_3":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnableMiscellaneousGameObjects[Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Explosion_3)][i]
                        .activeInHierarchy)
                    {
                        return list_spawnableMiscellaneousGameObjects[Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Explosion_3)][i];
                    }
                }
                break;
            }
            case "Explosion_WalknSpy":
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    if (!list_spawnableMiscellaneousGameObjects[Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Explosion_WalknSpy)][i]
                        .activeInHierarchy)
                    {
                        return list_spawnableMiscellaneousGameObjects[Convert.ToInt32(E_SpawnableObjects.Miscellaneous.Explosion_WalknSpy)][i];
                    }
                }
                break;
            }


            default:
            {
                for (int i = 0; i < int_amountOfObjectsToPool; i++)
                {
                    //If it's inactive, it's the one that is used.
                    if (!list_spawnableMiscellaneousGameObjects[int_objectIndex][i].activeInHierarchy)
                    {
                        return list_spawnableMiscellaneousGameObjects[int_objectIndex][i];
                    }
                }
                break;
            }
        }
        return null;
    }
}
