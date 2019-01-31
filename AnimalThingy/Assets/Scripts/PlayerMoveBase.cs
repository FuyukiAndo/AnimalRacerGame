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
	private bool isGrounded, isAtWall;

	// Use this for initialization
	protected virtual void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	protected virtual void Update ()
	{
		print(isGrounded + " : " + isAtWall);
		if (isGrounded && !isAtWall)
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
		if (!isGrounded && !isAtWall)
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
		if (Physics2D.Raycast ((Vector2) transform.position, Vector2.down, .5f))
		{
			isGrounded = true;
		}
	}

	protected virtual void OnCollisionExit2D (Collision2D other)
	{
		isAtWall = false;
	}

	protected virtual void DoPassivePower ()
	{

	}

	protected virtual void DoActivePower ()
	{

	}
}