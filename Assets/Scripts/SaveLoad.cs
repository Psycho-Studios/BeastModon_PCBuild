using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
[System.Serializable]
public static class SaveLoad {
	

	//it's static so we can call it from anywhere
	public static void Save(int gameFile) {
		GameObject gameObject_globalGameRules = GameObject.FindWithTag ("ConstantData");
		FileHolder thanksData = gameObject_globalGameRules.GetComponent<FileHolder> ();
		BinaryFormatter bf = new BinaryFormatter();
		//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
		FileStream file = File.Open (Application.persistentDataPath + "/savedGames.gd", FileMode.Open); //you can call it anything you want
			thanksData.final.nextLevelSaves [gameFile] = SceneManager.GetActiveScene ().buildIndex;
			bf.Serialize (file, thanksData.final);
			file.Close ();
		
		
	}   //GOOD TO GO


	public static void Save2(int gameFile) {
		GameObject gameObject_globalGameRules = GameObject.FindWithTag ("ConstantData");
		FileHolder thanksData = gameObject_globalGameRules.GetComponent<FileHolder> ();
		BinaryFormatter bf = new BinaryFormatter ();
		//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
		FileStream file = File.Open (Application.persistentDataPath + "/savedGames.gd", FileMode.Open); //you can call it anything you want
		thanksData.final.nextLevelSaves [gameFile] = 15;
		bf.Serialize (file, thanksData.final);
		file.Close ();
	}

	public static void Load(int gameFile) {
		GameObject gameObject_globalGameRules = GameObject.FindWithTag ("ConstantData");
		FileHolder thanksData = gameObject_globalGameRules.GetComponent<FileHolder> ();
		SceneManager.LoadScene (thanksData.final.nextLevelSaves[gameFile]);

	} // GOOD TO GO

	public static void NewGame(int gameFile, int mode)
	{


		BinaryFormatter bf = new BinaryFormatter ();

		if (!File.Exists (Application.persistentDataPath + "/savedGames.gd")) {
			Debug.Log ("File didn't exist!");
			GameData gameData = new GameData ();
			FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd");
			gameData = new GameData ();
			Debug.Log("Current difficulty is " + gameData.difficulty[0]);
			gameData.nextLevelSaves [gameFile] = 17; //Update this number when the game is complete
			gameData.difficulty [gameFile] = mode;
			Debug.Log("Current difficulty is " + gameData.difficulty[0]);
			bf.Serialize (file, gameData);
			file.Close ();
			SceneManager.LoadScene (17);
		} else
		{
			GameObject gameObject_globalGameRules = GameObject.FindWithTag ("ConstantData");
			FileHolder thanksData = gameObject_globalGameRules.GetComponent<FileHolder> ();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);

			Debug.Log("Current difficulty is " + thanksData.final.difficulty[0]);
			thanksData.final.nextLevelSaves[gameFile] = 17;
			thanksData.final.difficulty [gameFile] = mode;
				Debug.Log("Current difficulty is " + thanksData.final.difficulty[0]);
			bf.Serialize (file, thanksData.final);
			file.Close ();
			SceneManager.LoadScene (17);

		}

	}

	public static GameData Retrieve()
	{
        GameData gameData = new GameData();
        if (File.Exists (Application.persistentDataPath + "/savedGames.gd")) 
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream fileStream = File.Open (Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			gameData = (GameData)binaryFormatter.Deserialize (fileStream);
			fileStream.Close ();
			return gameData;
		}
		return gameData;


	} // GOOD TO GO

	public static void Delete()
	{
		if (File.Exists (Application.persistentDataPath + "/savedGames.gd")) {
			File.Delete(Application.persistentDataPath + "/savedGames.gd");
			Debug.Log ("FilesDeleted");
		}
	}

	public static void SaveSounds()
	{
		if (!File.Exists(Application.persistentDataPath + "/savedGames.gd")){
			return;
	}
		if(File.Exists(Application.persistentDataPath + "/savedGames.gd"))
			{
		GameObject gameObject_globalGameRules = GameObject.FindWithTag ("ConstantData");
		FileHolder thanksData = gameObject_globalGameRules.GetComponent<FileHolder> ();
		BinaryFormatter bf = new BinaryFormatter();
		//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
		FileStream file = File.Open (Application.persistentDataPath + "/savedGames.gd", FileMode.Open); //you can call it anything you want
		bf.Serialize (file, thanksData.final);
		file.Close ();
		Debug.Log ("Audio Data Saved.");
		}
		
	}

	[System.Serializable]
	public  class GameData 
	{
		public float[] soundData = { -10, -7, 1 };
		public  int[] clearData = {0, 0, 0, 0, 0}, difficulty = {0, 0, 0, 0, 0}, rankStay = {0, 0, 0, 0, 0};
		public int[] points = {0, 0, 0, 0, 0};
		public int[] nextLevelSaves = {0, 0, 0, 0, 0};
		public int[] arcadeLivesCounter = { 0, 0, 0, 0, 0 };
		public float[] chargeDamage = {100.0f, 110.0f, 125.0f, 160.0f, 200.0f},
		weaponDamageUpgrades = {0, 1.0f, 2.0f, 3.0f, 4.0f, 5.0f}, floorSlamTime = {3.0f, 2.75f, 2.25f, 1.9f, 1.5f}, chargeTime = {3.0f, 2.75f, 2.50f, 2.25f, 1.8f};
		public int[] shipUpgradesFile = {0, 0, 0, 0, 0}, shipUpgradesFile1 = {0, 0, 0, 0, 0}, shipUpgradesFile2 = {0, 0, 0, 0, 0},
		shipUpgradesFile3 = {0, 0, 0, 0, 0}, shipUpgradesFile4 = {0, 0, 0, 0, 0};
	}
}