using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach this script to any explosion effect
public class TimedExplosion : MonoBehaviour
{
    public float timeBeforeObjectRemoval;
    
    private void Awake()
    {
        StartCoroutine(explode());
    }
    IEnumerator explode()
    {
        yield return new WaitForSeconds(timeBeforeObjectRemoval);
        Destroy(this.gameObject, 0.0f);
    }
}
