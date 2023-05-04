using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterCutscene : MonoBehaviour {
	public int nextScene;
	public float endTime;
	private GameObject gameObject_globalGameRules;
	private FileHolder script_FileHolder;
	 void Awake()
	{
		gameObject_globalGameRules = GameObject.FindWithTag ("ConstantData");
		if(gameObject_globalGameRules != null)
		script_FileHolder = gameObject_globalGameRules.GetComponent<FileHolder> ();

	}
	// Use this for initialization
	void Start () {
		StartCoroutine (ShowBiz ());
		if(gameObject_globalGameRules != null && script_FileHolder != null)
		SaveLoad.Save (script_FileHolder.int_currentFileNumber);
	}

	IEnumerator ShowBiz()
	{
		yield return new WaitForSeconds(endTime);
		SceneManager.LoadSceneAsync(nextScene);
	}
	// Update is called once per frame
	void Update ()
	{
		
	}
}
