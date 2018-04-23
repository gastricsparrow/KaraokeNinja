using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

	public GameObject tutorial;
	// Use this for initialization
	public void NextLevel()
	{
		StartCoroutine(StartLevel());
	}

	void Update()
	{
		if (Input.GetButtonDown("Enter"))
		{
			StartCoroutine(StartLevel());
		}
	}

	public IEnumerator StartLevel()
	{	
		tutorial.SetActive(true);
		yield return new WaitForSeconds(4f);
		GameManager.NextLevel();
		yield return null;
	}
	
}
