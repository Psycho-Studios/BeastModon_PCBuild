using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour {

	public Transform pictureA;
	public float cutOffpoint;
	public float scrollSpeed;
	private float originalPosition, startPoint;
	void Start()
	{
		originalPosition = pictureA.position.x;
		startPoint = originalPosition;
	}
	void Update()
	{
		transform.Translate (-pictureA.right * (Time.deltaTime * scrollSpeed), Camera.main.transform);
		if (originalPosition + pictureA.position.x <= cutOffpoint)
			pictureA.position= new Vector3(startPoint, pictureA.position.y, pictureA.position.z);
		
			
			
	}
}
