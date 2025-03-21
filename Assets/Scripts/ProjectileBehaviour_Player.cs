using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Attach this script to every projectile the player(s) fire
public class ProjectileBehaviour_Player : ProjectileData
{  
    [HideInInspector] 
    public bool bool_isChildObject, bool_waitingForParent;
    
    public bool bool_projectileExplodesAfterTimer;

    private bool bool_explosiveProjectileLaunched, bool_parentLocationRecorded;

    public float degreesToRotate, projectileSpeed,
        timeAfterExplosionToDestroy, float_timeBeforeDestruction;

    public GameObject explosionIfApplicable;

    [HideInInspector]
    public Transform transform_parentGameObject;

    /*This script runs after ProjectileData.cs. If it belongs to a child object, it will record where to spawn.
     After that's done, SeperateTransformFromParent.cs will execute, which renders a parent's transform null,
     hence the logic below. Once this object is enabled, the vector3_objectLocation field can be utilized.*/
    private void Awake()
    {
        bool_playerProjectile = true;
    
        if(bool_isParentObject)
        {
            this.initialize_childObjectList();
        }
        if(bool_isChildObject)
        {
            this.transform_parentObject = this.transform.parent;
        }
    }

    private void OnEnable()
    {
        if (bool_isChildObject
        && !bool_waitingForParent) //If the parent gameObject's reference to this child object is populated, there is nothing to wait for.
        {
            this.transform.position = transform_parentObject.position + vector3_child_transformModifier;            
            this.transform.parent = null; //Projectile can now independently move from the parent
        }
        else if (bool_isParentObject)
        {
            foreach (Transform transform_childGameObject in this.list_childProjectiles)
            {
                transform_childGameObject.gameObject.SetActive(true);
            }
            if(this.gameObject.name.Contains("Puppeteer"))
            {
                StartCoroutine(removePuppeteer());
            }
        }

        removeGameObject();
    }

    IEnumerator removePuppeteer()
    {
        yield return new WaitForSeconds(float_timeBeforeDestruction);
        this.gameObject.SetActive(false);
    }

    private void Update() 
    {
        if(bool_isChildObject
        && !bool_waitingForParent
        && !bool_parentLocationRecorded) //This happens once, the first time Update is called
        {
            bool_parentLocationRecorded = true;        
        }

        launchProjectile();    
    }

    public override void launchProjectile()
    {
        if(degreesToRotate == 0.0f)
        {
            this.gameObject.transform.Translate(
                this.gameObject.transform.right * (projectileSpeed * Time.deltaTime),
                Camera.main.transform);
        }
        else
        {
            this.gameObject.transform.position += (Quaternion.Euler (0, 0, degreesToRotate) * Vector3.right) * (projectileSpeed * Time.deltaTime);
        }
    }

    public void removeGameObject() //If your object must disappear after time goes by then it must be marked as explosive
    {
        if (bool_projectileExplodesAfterTimer)
        {
            StartCoroutine(explode()); //Instantiate an explosion, then set the object inactive
        }
    }

    /// <summary>
    /// Used for timed explosions, NOT missiles.
    /// </summary>
    /// <returns></returns>
    IEnumerator explode()
    {
        yield return new WaitForSeconds(float_timeBeforeDestruction);

        if (explosionIfApplicable != null)
        {
            GameObject explosionClone = ObjectPool.objectPool_reference.getPooled_PlayerObjects(0, explosionIfApplicable.name);
            explosionClone.transform.position = this.gameObject.transform.position;
            explosionClone.transform.rotation = this.gameObject.transform.rotation;
            yield return new WaitForSeconds(0.25f); //Allows the explosion to engulf the round
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(false);
        }   
    }
}
