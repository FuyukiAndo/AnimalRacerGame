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
	[SerializeField] private float airTurningForce;

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
	public bool IsAtWall
	{
		get
		{
			return isAtWall;
		}
	}
	private bool isGrounded, isAtWall;
	private Vector2 wallDir;

	// Use this for initialization
	protected virtual void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	protected virtual void Update ()
	{
		print (isGrounded + " : " + isAtWall);
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
				isGrounded = false;
				rb.AddForce (new Vector2 (0f, jumpForce), ForceMode2D.Impulse);
			}
		}
		if (!isGrounded)
		{
			if (isAtWall)
			{
				if (wallDir == Vector2.right)
				{
					if (Input.GetKey (Left))
					{
						isAtWall = false;
					}
				}
				else if (wallDir == Vector2.left)
				{
					if (Input.GetKey (Right))
					{
						isAtWall = false;
					}
				}
				rb.velocity = new Vector2 (rb.velocity.x, -1.5f);
			}

			if (Input.GetKey (Left) && rb.velocity.x > 0)
			{
				rb.AddForce (new Vector2 (airTurningForce * -1, 0f));
			}
			if (Input.GetKey (Right) && rb.velocity.x < 0)
			{
				rb.AddForce (new Vector2 (airTurningForce, 0f));
			}
		}
	}

	protected virtual void FixedUpdate ()
	{
		RaycastHit2D hit2d = Physics2D.Raycast ((Vector2) transform.position - new Vector2 (0f, 1f), Vector2.down);
		if (hit2d.transform != null)
		{
			if (Vector2.Dot (hit2d.transform.up, Vector2.up) < 1f)
			{
				isGrounded = true;
				rb.constraints = RigidbodyConstraints2D.FreezeRotation;
				if (!Input.GetKey (Left) && !Input.GetKey (Right) && !Input.GetKey (Jump))
				{
					rb.constraints = RigidbodyConstraints2D.FreezePosition;
				}
			}
		}

		//Check if hit is sloped
		//Stabilize rotation
		//Set velocity to zero if no input
	}

	protected virtual void OnCollisionEnter2D (Collision2D other)
	{
		if (Physics2D.Raycast ((Vector2) transform.position - new Vector2 (0f, 1f), Vector2.down, .5f))
		{
			isGrounded = true;
		}

		if (Physics2D.Raycast ((Vector2) transform.position + new Vector2 (1f, 0f), Vector2.right, .5f))
		{
			isAtWall = true;
			wallDir = Vector2.right;
		}
		else if (Physics2D.Raycast ((Vector2) transform.position - new Vector2 (1f, 0f), Vector2.left, .5f))
		{
			isAtWall = true;
			wallDir = Vector2.left;
		}
	}

	protected virtual void OnCollisionStay2D (Collision2D other)
	{

	}

	protected virtual void OnCollisionExit2D (Collision2D other)
	{
		if (!Physics2D.Raycast ((Vector2) transform.position + new Vector2 (1f, 0f), Vector2.right, .5f))
		{
			isAtWall = false;
			wallDir = new Vector2 ();
		}
		else if (!Physics2D.Raycast ((Vector2) transform.position - new Vector2 (1f, 0f), Vector2.left, .5f))
		{
			isAtWall = false;
			wallDir = new Vector2 ();
		}
	}

	protected virtual void OnTriggerEnter2D (Collider2D other)
	{

	}

	protected virtual void OnTriggerStay2D (Collider2D other)
	{

	}

	protected virtual void OnTriggerExit2D (Collider2D other)
	{

	}

	protected virtual void DoActivePower ()
	{

	}

	protected virtual void OnDrawGizmos ()
	{

	}
}