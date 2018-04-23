using UnityEngine;

public class LaserSwitch : MonoBehaviour {

	public Sprite offSprite;

	void OnTriggerEnter2D()
	{
		foreach (GameObject laser in GameObject.FindGameObjectsWithTag("Laser"))
		{
			laser.SendMessage("TurnOff");
		}
		GetComponent<SpriteRenderer>().sprite = offSprite;
		
	}
}
