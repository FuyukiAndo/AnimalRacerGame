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

	[SerializeField] private float moveSpeed, jumpForce, slopeAngleThreshhold;
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
	private float slopeAngle;
	private RaycastHit2D hit2d;

	// Use this for initialization
	protected virtual void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		rb.freezeRotation = true;
	}

	// Update is called once per frame
	protected virtual void Update ()
	{
		if (isGrounded)
		{
			if (CheckForSlope ())
			{
				rb.velocity = Vector2.zero;
				Vector2 moveRight = GetRotationForSlope().normalized * moveSpeed;
				Vector2 moveLeft = (GetRotationForSlope().normalized * -1) * moveSpeed;
				if (Input.GetKey (Left))
				{
					rb.velocity = moveLeft;
				}
				if (Input.GetKey (Right))
				{
					rb.velocity = moveRight;
				}
			}
			else
			{
				if (Input.GetKey (Left))
				{
					rb.velocity = Vector2.Scale (transform.right, new Vector2 (moveSpeed * -1, rb.velocity.y));
				}
				if (Input.GetKey (Right))
				{
					rb.velocity = Vector2.Scale (transform.right, new Vector2 (moveSpeed, rb.velocity.y));
				}
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

			if (Input.GetKey (Left) && rb.velocity.x >= 0f)
			{
				rb.AddForce (new Vector2 (airTurningForce * -1, 0f));
			}
			if (Input.GetKey (Right) && rb.velocity.x <= 0f)
			{
				rb.AddForce (new Vector2 (airTurningForce, 0f));
			}
		}
	}

	protected virtual void FixedUpdate ()
	{

	}

	protected virtual void OnCollisionEnter2D (Collision2D other)
	{
		if (Physics2D.Raycast ((Vector2) transform.position - new Vector2 (0f, .6f), Vector2.down, .5f))
		{
			isGrounded = true;
		}

		if (Physics2D.Raycast ((Vector2) transform.position + new Vector2 (1f, 0f), Vector2.right, .1f))
		{
			isAtWall = true;
			wallDir = Vector2.right;
		}
		else if (Physics2D.Raycast ((Vector2) transform.position - new Vector2 (1f, 0f), Vector2.left, .1f))
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
		if (!Physics2D.Raycast ((Vector2) transform.position + new Vector2 (1f, 0f), Vector2.right, .1f))
		{
			isAtWall = false;
			wallDir = new Vector2 ();
		}
		else if (!Physics2D.Raycast ((Vector2) transform.position - new Vector2 (1f, 0f), Vector2.left, .1f))
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

	private Vector2 GetRotationForSlope ()
	{
		return (Quaternion.FromToRotation (Vector2.up, hit2d.normal) * transform.rotation) * Vector2.right;
	}

	private bool CheckForSlope ()
	{
		hit2d = Physics2D.Raycast ((Vector2) transform.position - new Vector2 (0f, .6f), Vector2.down, 1f);
		if (hit2d.transform == null) return false;
		slopeAngle = Mathf.Abs (hit2d.transform.rotation.z); //Mathf.Abs (Mathf.Atan2 (hit2d.normal.x, hit2d.normal.y) * Mathf.Rad2Deg);

		if (Vector2.Dot (hit2d.transform.up, Vector2.up) < 1f && slopeAngle < slopeAngleThreshhold)
		{
			return true;
		}
		return false;
	}
}