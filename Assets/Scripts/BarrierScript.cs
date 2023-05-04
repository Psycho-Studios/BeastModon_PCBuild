using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour 
{
	private Collider2D collider2d_boss;
	private Transform transform_boss;
	private GameObject gameObject_boss, gameObject_barrierObject;
	public Vector3 vector3_offsetFromBoss, vector3_barrierExplosionLocation;
	public GameObject barrierExplosion;

	void Start()
	{
		gameObject_boss = GameObject.FindWithTag ("Boss");
		collider2d_boss = gameObject.GetComponent<Collider2D> ();
		transform_boss = gameObject_boss.GetComponent<Transform> ();
		gameObject.transform.position = transform_boss.position + vector3_offsetFromBoss;
		gameObject.transform.parent = transform_boss; // This makes said gameObject the child of another (my parent = this!!!)
	}


	void OnTriggerEnter2D(Collider2D collider2d_intrudingObject)
	{
		if (collider2d_intrudingObject.tag == "ChargedGameController"
		|| collider2d_intrudingObject.tag == "GameController2") 
		{
			if (barrierExplosion != null) 
			{
				gameObject_barrierObject = (GameObject)Instantiate (
					barrierExplosion,
					(transform.position + vector3_barrierExplosionLocation),
					transform.rotation);
				Destroy (gameObject_barrierObject, 0.31f);
			}
			Destroy (gameObject);

		}
	}
		
}
