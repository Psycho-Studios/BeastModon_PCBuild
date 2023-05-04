using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTranslate : MonoBehaviour
{
    public float degreesToRotate, projectileSpeed, float_timeBeforeDestruction;
    private bool bool_explosiveProjectileLaunched;
    public bool bool_explosiveProjectile;
    public GameObject explosionIfApplicable;

    private void OnEnable()
    {
        removeGameObject();
    }

    private void Update()
    {
        launchProjectile();
    }

    public void removeGameObject() //Applicable to an animation event if ever necessary
    {
        if (bool_explosiveProjectile)
        {
            StartCoroutine(explode()); //Instantiate an explosion, then set the object inactive
        }
        else
        {
            StartCoroutine(recycleProjectile()); //Set the object inactive after a waiting period
        }
    }

    IEnumerator explode()
    {
        yield return new WaitForSeconds(float_timeBeforeDestruction);
        GameObject explosionClone = (GameObject)Instantiate(
            explosionIfApplicable,
            this.gameObject.transform.position,
            this.gameObject.transform.rotation);
        yield return new WaitForSeconds(0.25f); //Allows the explosion to engulf the round
        this.gameObject.SetActive(false);
    }

    IEnumerator recycleProjectile()
    {
        yield return new WaitForSeconds(float_timeBeforeDestruction);
        this.gameObject.SetActive(false);
    }

    //The behaviour of the projectile once it is instantiated
    public void launchProjectile()
    {
        if (degreesToRotate == 0.0f)
        {
            this.gameObject.transform.Translate(
                this.gameObject.transform.right * (projectileSpeed * Time.deltaTime),
                Camera.main.transform);
        }
        else
        {
            this.gameObject.transform.position += (Quaternion.Euler(0, 0, degreesToRotate) * Vector3.right) * (projectileSpeed * Time.deltaTime);
        }
    }
}
