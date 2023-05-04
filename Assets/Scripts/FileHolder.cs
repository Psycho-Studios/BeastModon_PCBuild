using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class FileHolder : MonoBehaviour {
	public SaveLoad.GameData final;
	// I WROTE THIS SO THE SAVE SYSTEM COULD CONSTANTLY KEEP TRACK OF WHAT FILE TO SAVE TO THROUGHOUT THE GAME
	public int int_currentFileNumber;

	public bool gary, sceneSelected;


	void Awake()
	{
		final = SaveLoad.Retrieve();
		if(SceneManager.GetActiveScene().buildIndex >= 0)
		{
			DontDestroyOnLoad(this.gameObject);
		}
	}


	void Update()
	{
		// Should check for credits
		if (gary == true) {
			Debug.Log ("Gary's got the data!");
			if (sceneSelected == false) {
				switch (final.difficulty [int_currentFileNumber]) {
				case 4:
					final.clearData [int_currentFileNumber] = 1; //Easy Mode Cleared!
					StartCoroutine (Add ());
					break;
				case 3:
					final.clearData [int_currentFileNumber] = 2; //Normal Mode Cleared!
					StartCoroutine (Add ());
					break;
				case 2:
					final.clearData [int_currentFileNumber] = 3; //Hard Mode Cleared!
					StartCoroutine (Add ());
					break;
				case 1:
					final.clearData [int_currentFileNumber] = 4; //Arcade Mode Cleared!
					StartCoroutine (Add ());
					break;
				}
				gary = false;
			}
		}
	}

	IEnumerator Add()
	{
		yield return new WaitForSeconds (2);
		final.points[int_currentFileNumber] += 50000;
		SaveLoad.Save2 (int_currentFileNumber);
		Destroy (this.gameObject);
	}
		
}
