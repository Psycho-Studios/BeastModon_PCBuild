using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSlam_Player1 : MonoBehaviour
{
    private bool bool_readyToSlam, bool_preppingFloorSlam;
	private float float_timeBeforeFloorSlam, float_timeSpentAscending,
		float_currentTime;
	private Animator animator_floorSlamIndicator;
    public AudioClip audioClip_slamCharge, audioClip_slamExecute;
    private AudioSource _audioSource;
    private GameObject gameObject_camera,
		gameObject_floorSlamLight,
	 	gameObject_globalGameRules;
    private Vector3 vector3_cameraCoordinates;
	
    void Awake()
    {
		float_timeBeforeFloorSlam = 2.0f;
		animator_floorSlamIndicator = this.gameObject.GetComponent<Animator>();
		_audioSource = this.gameObject.GetComponent<AudioSource>();
		gameObject_camera = GameObject.FindWithTag("MainCamera");
		vector3_cameraCoordinates = gameObject_camera.GetComponent<Transform>().position;
    }

    void Update()
    {
		float_currentTime = Time.timeSinceLevelLoad;
		if (Input.GetKey(ProjectileControls_Player1.keyCode_moveUp)
		|| (Input.GetAxisRaw("Vertical") > 0)) //Logic for a platform besides UNITY_STANDALONE
		{
			if (bool_preppingFloorSlam == false)
			{
				float_timeSpentAscending = float_currentTime; // Slam Time recorded, initiating attack
				bool_preppingFloorSlam = true;
			}

			if ((float_currentTime - float_timeSpentAscending >= float_timeBeforeFloorSlam) 
			&& (float_currentTime - float_timeSpentAscending <= float_timeBeforeFloorSlam + 2.0f) 
			&& (bool_readyToSlam == false)) 
			{
				animator_floorSlamIndicator.SetInteger ("Path", 2);
				this._audioSource.clip = audioClip_slamCharge;
				this._audioSource.Play(); // If it's been long enough, you'll hear a notification to slam!
				bool_readyToSlam = true;
			}
			if (float_currentTime - float_timeSpentAscending > float_timeBeforeFloorSlam + 2.0f) // Wait too long, and it all resets.
			{ 
				bool_readyToSlam = false;
				animator_floorSlamIndicator.SetInteger("Path", 0);
				float_timeSpentAscending = 500000f;
				bool_preppingFloorSlam = false;
			}
		}

		if (Input.GetKey(ProjectileControls_Player1.keyCode_moveDown)
		|| (Input.GetAxisRaw ("Vertical") == 0)) //Logic for a platform besides UNITY_STANDALONE
        {
			animator_floorSlamIndicator.SetInteger ("Path", 0); 
			if (bool_readyToSlam == true)
			{
			 // If you still meet the requirements, the screen shall shake.
				StartCoroutine (ScreenShake ()); //Will play a sound
				bool_readyToSlam = false;
			}
			if (bool_readyToSlam == false)
			{
				float_timeSpentAscending = 500000f; // If no requirements are met, it all resets
				bool_preppingFloorSlam = false;
			}
		}
			
    }
    IEnumerator ScreenShake()
    {
		GameObject gameObject_floorSlam = ObjectPool.objectPool_reference.getPooled_PlayerObjects(
			default,
			"FloorSlam");
		gameObject_floorSlam.transform.position = vector3_cameraCoordinates;
		this._audioSource.clip = audioClip_slamExecute;
		this._audioSource.Play();
        yield return new WaitForSeconds(0.3f);
        gameObject_camera.transform.Translate(-0.5f, 0.02f, 4.75f);//24, -0.06, -10.00
        yield return new WaitForSeconds(0.045f);
        gameObject_camera.transform.Translate(2f, 0, -0.29f);//-26, -0.06, -9.71
        yield return new WaitForSeconds(0.045f);
		gameObject_floorSlam.SetActive(true); //FloorSlam object set active here
		gameObject_camera.transform.Translate(3f, 0.04f, -0.75f);//-23, 0.02, -10.46
        yield return new WaitForSeconds(0.045f);
        gameObject_camera.transform.Translate(-2f, 0.08f, 0); //-25, 0.1f, -10.46f
        yield return new WaitForSeconds(0.045f);
        gameObject_camera.transform.Translate(-3f, -0.05f, 0f);//-22, -0.4f, -10.46f
        yield return new WaitForSeconds(0.045f);
        gameObject_camera.transform.Translate(1f, 0.04f, 0);//-21, 0, -10.46f
        yield return new WaitForSeconds(0.045f);
        gameObject_camera.transform.Translate(-0.5f, -0.02f, 0);///-23.5f, -0.2f, -10.46f
        yield return new WaitForSeconds(0.045f);
        gameObject_camera.transform.Translate(1.5f, 0, 0);///-24, -0.2f, -10.46f
        yield return new WaitForSeconds(0.03f);
        gameObject_camera.transform.Translate(2f, -0.02f, 0);///-22, 0, -10.46f
        yield return new WaitForSeconds(0.045f);
        gameObject_camera.transform.position = vector3_cameraCoordinates;
		yield return new WaitForSeconds(1.5f);
		gameObject_floorSlam.SetActive(false);
	}
   
}
    