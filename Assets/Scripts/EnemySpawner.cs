 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

   public GameObject enemy;
 
    public float spawnWait;
    public int amountOfSpawns;
    public int howManyEnemies;
    public float lifeTime;
    public float firstSpawnWait;
    public Vector3 place;
    public Vector3 rotator;
	private GameObject gameObject_globalGameRules;

    private void Start()
    {
		
        enemy.transform.position = place;
        StartCoroutine(GameTime());
    }


    IEnumerator GameTime()
    {
        yield return new WaitForSeconds(firstSpawnWait);

        for (int i = 0; i < amountOfSpawns; i++)
        {
            for (int r = 0; r < howManyEnemies; r++)
            {
                GameObject clone = (GameObject)Instantiate(
                    enemy, 
                    place, 
                    Quaternion.Euler(rotator));
                if (lifeTime > 0)
                {
                    Destroy(clone, lifeTime);
                }
                yield return new WaitForSeconds(spawnWait);
            }
        }
    }

}