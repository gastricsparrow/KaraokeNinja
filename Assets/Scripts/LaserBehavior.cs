using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour {

	public float startAngle = 120f; // 0 degrees = right
	public float endAngle = 170f; 
	public float speed = 1f; // oscillation speed
	public float raycastLength = 10f;
	float angle;
	RaycastHit2D hit;
	Vector2 hitpoint;
	Vector2 currentVector;
	bool on = true;

	float midAngle;
	float angleRange;
	void Start()
	{
		midAngle = (startAngle + endAngle) / 2f;
		angleRange = (endAngle - startAngle) / 2f;

		GetComponent<LineRenderer>().useWorldSpace = true;
		GetComponent<LineRenderer>().SetPosition(0, transform.position);
	}
	// Update is called once per frame
	void Update () {
		if (on)
			Laser();

	}

	void Laser ()
	{
		angle = midAngle + angleRange * Mathf.Cos(Time.time * speed);
		

		currentVector = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

		int layerMask = (1 << LayerMask.NameToLayer("Default")) |
						(1 << LayerMask.NameToLayer("Player")) |
						(1 << LayerMask.NameToLayer("Platform")) |
						(1 << LayerMask.NameToLayer("Ground"));
		hit = Physics2D.Raycast(transform.position, currentVector.normalized, raycastLength, layerMask);
		if (hit.collider != null)
		{
			if (hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.tag == "Enemy")
			{
				hit.collider.gameObject.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
			}
			hitpoint = hit.point;

		}
		else
		{
			hitpoint = transform.position + (Vector3)currentVector * raycastLength;
		}
		GetComponent<LineRenderer>().SetPosition(1, hitpoint);
	}

	void OnDrawGizmoes()
	{
		Gizmos.color =  Color.green;
		Gizmos.DrawLine (transform.position, transform.position + (Vector3)currentVector.normalized * raycastLength);
	}

	void TurnOff()
	{
		GetComponent<LineRenderer>().enabled = false;
		on = false;
	}
}
