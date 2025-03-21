using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public float timeBeforeRemovingSpawnedGameObject, timeBetweenSpawns;
    public float timeInitiallySpawnedGameObject;
    
    public int amountOfSpawns;


    /// <summary>
    /// This is the enum that will be used to determine which scene the player is in,
    /// as long as the scene name matches.
    /// This is a very dangerous way to determine what the object-type is, use this
    /// technique with caution.
    /// </summary>
    private object enumToQuery;
    private object indexOfGameObjectToSpawn = null; 

    public GameObject enemyGameObjectToSpawn;

    private void Awake()
    {
        enumToQuery = Enum.Parse(typeof(Type), SceneManager.GetActiveScene().name);
        Enum.TryParse((Type)enumToQuery, gameObject.name, out indexOfGameObjectToSpawn); //Sets the gameObject index

        if (indexOfGameObjectToSpawn == null)
        {
            Debug.LogError("The object index is null. Please check the object name and the enum.");
        }
        else
        {
            StartCoroutine(GameTime());
        }
    }

    IEnumerator GameTime()
    {
        yield return new WaitForSeconds(timeBetweenSpawns);

        for (int i = 0; i < amountOfSpawns; i++)
        {
            GameObject enemyClone = ObjectPool.objectPool_reference.getPooled_LevelSpecificObjects((int)indexOfGameObjectToSpawn, (System.Enum)enumToQuery);
        }
    }
}