using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
	[Header("Movimentacao.")]
	public float speed;
	public float upForce;
	public float gravityScale;
	public float multiplySpeedDown;

	[Header("Ground Check.")]
	public float footOffset;
	public float groundOffset;
	public float groundDistance;
	public LayerMask groundLayer;

	[Header("Componentes.")]
	public GhostManager ghostManager;
	Rigidbody2D rb;
	Animator anim;

	ParticleSystem particles;

	float inputX;
	float inputY;

	bool facingRight = true;
	bool onGround;

	[HideInInspector] public bool isMove;


	void Start()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		particles = GetComponentInChildren<ParticleSystem>();
	}
	void Update()
	{
		GetInput();
		ChangeVelo();
		ChangeFlip();
		PhysicsCheck();
		CheckDeath();
	}
    private void FixedUpdate()
    {
		Movement();
	}
    void GetInput()
    {
		inputX = Input.GetAxisRaw("Horizontal");
		inputY = Input.GetAxisRaw("Vertical");

		if (inputY < 0)
		{
			inputY /= 5;
		}
	}
	void Movement()
	{
        if (isMove)
        {
			rb.AddForce(Vector2.right * inputX * speed);
			rb.AddForce(Vector2.up * inputY * upForce);
		}
	}
	void Flip()
    {
        if (isMove)
        {
			facingRight = !facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}
	void ChangeFlip()
    {
		if (inputX > 0 && !facingRight)
		{
			Flip();
		}
		else if (inputX < 0 && facingRight)
		{
			Flip();
		}
	}
	void ChangeVelo()
    {
		speed -= multiplySpeedDown * Time.deltaTime;
		upForce -= multiplySpeedDown * 2 * Time.deltaTime;
		anim.speed -= 0.015f * Time.deltaTime;

		if (speed <= 0f)
        {
			speed = 0f;
        }
		if (upForce <= 0f)
		{
			upForce = 0f;
		}

		if(anim.speed <= 0f)
        {
			anim.speed = 0f;
        }

	}
	void PhysicsCheck()
	{
		onGround = false;

		RaycastHit2D leftFoot = Raycast(new Vector2(-footOffset, -groundOffset), Vector2.down, groundDistance, groundLayer);
		RaycastHit2D rightFoot = Raycast(new Vector2(footOffset, -groundOffset), Vector2.down, groundDistance, groundLayer);
		
		if (leftFoot || rightFoot)
		{
			onGround = true;
		}
        if (onGround)
        {
			anim.SetBool("Move", false);
			inputX = 0;
        }
        else
        {
			anim.SetBool("Move", true);
			onGround = false;
		}
	}
	RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layerMask)
	{
		Vector2 pos = transform.position;
		RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layerMask);
		Color color = hit ? Color.red : Color.green;
		Debug.DrawRay(pos + offset, rayDirection * length, color);
		return hit;
	}

	void CheckDeath()
    {
		if(speed <= 0.1f && upForce <= 0.1f)
        {
			ghostManager.recording = false;
			GameManager.instance.podeInst = true;
			Destroy(gameObject);
		}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
			particles.Play();
        } 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
		if (other.CompareTag("Light"))
		{
			particles.Stop();
		}
	}
}