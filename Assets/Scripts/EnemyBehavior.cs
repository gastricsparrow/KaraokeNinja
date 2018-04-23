using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

	public float speed = 5f;
	float raycastLength = 4f;
	Animator animator;
	public float pursueTime = 3f;
	float pursuingTimeout = 0f;
	bool pursuing;
	float facing;

	void Start()
	{
		animator = GetComponent<Animator>();
	}
	// Update is called once per frame
	void Update () {
		FindPlayer();
		if (pursuing)
		{
			Pursue();
		}
	}

	void FindPlayer()
	{
		bool flipped = GetComponent<SpriteRenderer>().flipX;
		facing = flipped ? -1f : 1f; 

		RaycastHit2D hit = Physics2D.Raycast(transform.position, facing * Vector2.right, raycastLength, 1 << LayerMask.NameToLayer("Player"));
		if (hit.collider != null)
		{
			if (hit.collider.gameObject.tag == "Player")
			{
				animator.SetBool("pursuing", true);
				pursuingTimeout = pursueTime;
				pursuing = true;
			}
		}
		else if (pursuingTimeout > 0)
		{
			pursuingTimeout -= Time.deltaTime;
		}

		if (pursuingTimeout <= 0)
		{
			animator.SetBool("pursuing", false);
			pursuing = false;
		}

	}

	// void OnDrawGizmos()
	// {
	// 	Gizmos.DrawLine(transform.position, transform.position + new Vector3(raycastLength, 0, 0));
	// }
	void Pursue()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(facing * speed, GetComponent<Rigidbody2D>().velocity.y);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Spike")
		{
			Die();
		}
	}
	
	void Die()
	{ 
		animator.SetTrigger("die");
		GetComponent<BoxCollider2D>().enabled = false;
		Destroy(gameObject, 1f);
	}
}
