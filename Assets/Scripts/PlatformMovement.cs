using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {

	public float speed = 6f;
	public float expandingSpeed = 30f;
	public float isCurrentNote = 1f;

	
	// Update is called once per frame
	void Update () {

		if (isCurrentNote > 0)
		{
			transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y + expandingSpeed * Time.deltaTime);
		}

		transform.position += new Vector3(0,speed * Time.deltaTime,0);
		//GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);

		if (transform.position.y > 12f)
			Destroy(gameObject);

		isCurrentNote -= 10f * Time.deltaTime;
	}

}
