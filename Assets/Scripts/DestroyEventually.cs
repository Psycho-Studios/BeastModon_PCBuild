using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEventually : MonoBehaviour {
	public float timeTillDestruction, timeBeforeClone, timeTillCloneDestruction, distanceTillDestructionX, distanceTillDestructionY;
	public GameObject explosion;
	public bool left, up, right, down, isDistanceAFactor, isThisAChildObject;
	private GameObject clone, clone2, clone3, clone4, clone5;
	private Transform who;

	// Use this for initialization
	void Start () {
		if (isThisAChildObject == true)
			who = transform.parent.GetComponent<Transform> ();


		if (timeTillDestruction != 0) {
			Destroy (gameObject, timeTillDestruction);
		}
		else if (explosion != null) {
			if (isDistanceAFactor == true) {
				CloneDistance ();
			} 
			else 
			{
				StartCoroutine (Backup ());
			}
		}
	}


	/*TooFarLeft means whether or not the object will make another one based on how far left it goes.
	The same thing with tooFarDown; if the object goes too far down, make one in its place.
	I would suggest adding custom positions later. Good luck, Future Jeshawn. You can do this.*/

	IEnumerator Backup()
	{
		if (timeBeforeClone != 0) {
			yield return new WaitForSeconds (timeBeforeClone);
			clone = (GameObject)Instantiate (explosion, transform.position, transform.rotation);
			Destroy (clone, timeTillCloneDestruction);
		} 
			
	}
	void Update()
	{
		if (isDistanceAFactor == true) {
			CloneDistance ();
		}
	}
	void CloneDistance()
	{
		if (isThisAChildObject == true) {

			if ((who.position.x < distanceTillDestructionX) && (left == true)) {
				clone2 = (GameObject)Instantiate (explosion, transform.position, transform.rotation);
				Destroy (gameObject);
			} else if ((who.position.x > distanceTillDestructionX) && (right == true)) {
				clone3 = (GameObject)Instantiate (explosion, transform.position, transform.rotation);
				Destroy (gameObject);
			} else if ((who.position.y < distanceTillDestructionY) && (down == true)) {
				clone4 = (GameObject)Instantiate (explosion, transform.position, transform.rotation);
				Destroy (gameObject);
			} else if ((who.position.y > distanceTillDestructionY) && (up == true)) {
				clone5 = (GameObject)Instantiate (explosion, transform.position, transform.rotation);
				Destroy (gameObject);
			}
		}
		else 
		{
			if ((gameObject.transform.position.x < distanceTillDestructionX) && (left == true)) {
				clone2 = (GameObject)Instantiate (explosion, transform.position, transform.rotation);
				Destroy (gameObject);
			} else if ((gameObject.transform.position.x > distanceTillDestructionX) && (right == true)) {
				clone3 = (GameObject)Instantiate (explosion, transform.position, transform.rotation);
				Destroy (gameObject);
			} else if ((gameObject.transform.position.y < distanceTillDestructionY) && (down == true)) {
				clone4 = (GameObject)Instantiate (explosion, transform.position, transform.rotation);
				Destroy (gameObject);
			} else if ((gameObject.transform.position.y > distanceTillDestructionY) && (up == true)) {
				clone5 = (GameObject)Instantiate (explosion, transform.position, transform.rotation);
				Destroy (gameObject);
			}
		}
	}
}
