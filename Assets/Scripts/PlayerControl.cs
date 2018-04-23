using UnityEngine;

public class PlayerControl : MonoBehaviour {

	public float maxSpeed = 5f;
	public float jumpPower = 200f;

	public float moveInput;
	public bool jumpInput;
	SpriteRenderer spriteRenderer;
	Animator animator;
	Vector2 velocity;

	float jumpForce;
	float jumpFactor = 1f;
	[SerializeField]
	bool grounded;

	float raycastLength = 0.85f;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		GetInput();
		CheckGround();
		Move();


	}

	void CheckGround()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, (1 << LayerMask.NameToLayer("Platform")) | (1 << LayerMask.NameToLayer("Ground")));
		if (hit.collider != null)
		{	
			grounded = true;
			if (hit.collider.gameObject.tag == "Platform")
			{
				jumpFactor = 1.5f;
			}
			else
			{
				jumpFactor = 1f;
			}
		}
		else
		{
			grounded = false;
		}
			
	}

	void GetInput() {
		velocity = GetComponent<Rigidbody2D>().velocity;
		moveInput = Input.GetAxis ("Horizontal");
	}

	void Move() {
		Vector2 move = Vector2.zero;

        move.x = moveInput;

        if (Input.GetButtonDown ("Jump") && grounded) {
            velocity.y = jumpPower * jumpFactor;
        } else if (Input.GetButtonUp ("Jump")) 
        {
            if (velocity.y > 0) {
                velocity.y = velocity.y * 0.5f;
            }
        }

        if (move.x != 0)
			spriteRenderer.flipX = move.x < 0.01f;
        

        animator.SetBool ("grounded", grounded);
        animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);

        velocity.x = move.x * maxSpeed;
		GetComponent<Rigidbody2D>().velocity = velocity;
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -raycastLength, 0));
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Victory")
		{
			print ("WIN!");
			Invoke("NextLevel", 3);
		}
		else if (other.gameObject.tag == "Spike")
		{
			Die();
		}
		else if (other.gameObject.name.Contains("Tutorial"))
		{
			GameObject.FindObjectOfType<Canvas>().transform.Find(other.gameObject.name).gameObject.SetActive(true);
		}
	}

	void Restart()
	{
		GameManager.Restart();
	}

	void NextLevel()
	{
		GameManager.NextLevel();
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.collider.gameObject.tag == "Enemy")
		{
			Die();
		}
	}

	void Die()
	{
		print ("FAIL!");
		animator.SetTrigger ("die");
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<Rigidbody2D>().simulated = false;
		Invoke("Restart", 2);
	}
	
}
