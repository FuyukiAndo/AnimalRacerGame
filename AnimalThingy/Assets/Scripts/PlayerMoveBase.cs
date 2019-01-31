using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (BoxCollider2D))]
public class PlayerMoveBase : MonoBehaviour
{
	protected float MoveSpeed
	{
		get
		{
			return moveSpeed;
		}
		set
		{
			moveSpeed = value;
		}
	}
	protected float JumpForce
	{
		get
		{
			return jumpForce;
		}
		set
		{
			jumpForce = value;
		}
	}

	[SerializeField] private float moveSpeed, jumpForce;
	[SerializeField] private KeyCode Jump, Left, Right;

	protected Rigidbody2D RB
	{
		get
		{
			return rb;
		}
	}
	private Rigidbody2D rb;

	public Vector2 RBVel
	{
		get
		{
			return vel;
		}
		protected set
		{
			vel = value;
		}
	}
	private Vector2 vel;

	public bool IsGrounded
	{
		get
		{
			return isGrounded;
		}
		protected set
		{
			isGrounded = value;
		}
	}
	private bool isGrounded;

	// Use this for initialization
	protected virtual void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	protected virtual void Update ()
	{
		if (isGrounded)
		{
			if (Input.GetKey (Left))
			{
				rb.velocity = new Vector2 (moveSpeed * -1, rb.velocity.y);
			}
			if (Input.GetKey (Right))
			{
				rb.velocity = new Vector2 (moveSpeed, rb.velocity.y);
			}
			if (Input.GetKeyDown (Jump))
			{
				rb.AddForce (new Vector2 (0f, jumpForce), ForceMode2D.Impulse);
				isGrounded = false;
			}
		}
		if (!isGrounded)
		{
			if (Input.GetKey (Left))
			{
				rb.velocity = new Vector2 (moveSpeed * -1, rb.velocity.y);
			}
			if (Input.GetKey (Right))
			{
				rb.velocity = new Vector2 (moveSpeed, rb.velocity.y);
			}
		}
	}

	protected virtual void FixedUpdate ()
	{

	}

	protected virtual void OnCollisionEnter2D (Collision2D other)
	{
		isGrounded = true;
	}

	protected virtual void DoPassivePower()
	{

	}

	protected virtual void DoActivePower()
	{
		
	}
}