using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBurst : MonoBehaviour
{
    public GameObject enemyBullet;
    public Vector3 bulletSpawn;
    public float setWait;
    private bool updateStopper;
    public Vector3 bulletRotation;
    public float bulletLifeTime;
    public float firstShot;

    AudioSource speakers;
    public AudioClip sound;

    public float secondsToNextShot, secondWait;
    private float clock, temp;


    IEnumerator FirstTry()
    {
        yield return new WaitForSeconds(firstShot);
        speakers.PlayOneShot(sound);
        GameObject clone = (GameObject)Instantiate(enemyBullet, (transform.position + bulletSpawn), Quaternion.Euler(bulletRotation));
        Destroy(clone, bulletLifeTime);
    }
    void Start()
    {
        speakers = gameObject.GetComponent<AudioSource>();
        updateStopper = false;
    }


    // Update is called once per frame
    void Update()
    {
        clock = Time.time;
        if (clock - temp >= secondsToNextShot)
        {
            updateStopper = false;
        }

        {
            if (updateStopper == false)
                doIT();
        }
    }
    void doIT()
    {
        if (firstShot > 0)
        {
            Debug.Log("Han Shot First");
            StartCoroutine(FirstTry());
            updateStopper = true;
            firstShot = 0;
            temp = secondsToNextShot;
            return;
        }
        else
        {
            Debug.Log("The alien shot next.");
            speakers.PlayOneShot(sound);
            GameObject clone = (GameObject)Instantiate(enemyBullet, (transform.position + bulletSpawn), Quaternion.Euler(bulletRotation));

            if (bulletLifeTime > 0)
            {
                Destroy(clone, bulletLifeTime);
            }
            else
            {
                Destroy(clone, 2.0f);
            }

            temp = Time.time + secondWait;
            Debug.Log(temp);



        }
        updateStopper = true;

    }

}
