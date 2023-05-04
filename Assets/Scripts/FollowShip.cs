using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShip : MonoBehaviour {
    private Transform follow;
    private GameObject ship;
	public bool behind;
	public Vector3 whereExactly;
	void Start () {
        ship = GameObject.FindWithTag("Player");
        follow = ship.GetComponent<Transform>();
        gameObject.transform.position = follow.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (behind == false) {
			gameObject.transform.position = follow.position;
		}
		else
		{
			gameObject.transform.position = follow.position + whereExactly;
		}
	}
}
