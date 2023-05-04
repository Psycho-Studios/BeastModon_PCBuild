using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowUpTouch : MonoBehaviour 
{
    public GameObject gameObject_explosion;
	public float float_whenToDestroyAsset;

	void Start () 
	{
		
	}
	void OnTriggerEnter2D(Collider2D other)
        {
        if(other.tag == "Player")
        {
            GameObject gameObject_spawnedExplosion = (GameObject) Instantiate(
				gameObject_explosion,
				gameObject.transform.position,
				gameObject.transform.rotation);
			if (float_whenToDestroyAsset > 0)
				Destroy (gameObject_spawnedExplosion, float_whenToDestroyAsset);
			else
				Destroy (gameObject_spawnedExplosion, 1.0f);
            Destroy(gameObject);
        }
    }
	// Update is called once per frame
	void Update ()
	{
		
	}
}
