using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static int currentLevel = 0;
	public static int maxLevel = 8;
	
	public static void NextLevel()
	{
		if (currentLevel >= maxLevel)
		{
			SceneManager.LoadScene("EndCredits");
		}
		else if (currentLevel == 0)
		{
			SceneManager.LoadScene("Title");
		}
		SceneManager.LoadScene("Level" + (currentLevel + 1).ToString());
		GameManager.currentLevel ++;
	}

	public static void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	
}
