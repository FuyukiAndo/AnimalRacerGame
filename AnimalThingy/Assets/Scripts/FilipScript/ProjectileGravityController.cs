using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGravityController : MonoBehaviour 
{
	private CollisionController collisionController;

	
	[Header("Gravity Settings")]
	public float velocity = 8.0f;
	[Range(0.01f,1.0f)]public float velocityDelay = 0.4f;
	
	protected Vector2 movement;
	
	private float gravity;
	//private float maxVelocity;
	
	private bool isGravity = true;
	
	private bool isGravity = true;
	
	public virtual void Start()
	{
		collisionController = GetComponent<CollisionController>();
	}
	
	private void UpdateGravity()
	{
		gravity = -(2*velocity)/Mathf.Pow(velocityDelay, 2);
		//maxVelocity = Mathf.Abs(gravity) * velocityDelay;
	}
	
	void OnValidate()
	{
		if(velocity < 0.1f)
		{
			velocity = 0.1f;
		}
	}
	
	public virtual void Update()
	{	
		if (collisionController.boxCollisionDirections.up || collisionController.boxCollisionDirections.down)
		{
			movement.y = 0;
			movement.x = 0;
			
			if(movement.y == 0)
			{
				isGravity = false;
			}
		}	

		if(isGravity)
		{
			UpdateGravity();
			movement.y += gravity * Time.deltaTime;	
			MoveObject(movement * Time.deltaTime);
		}
	}
	
	private void MoveObject(Vector2 movement, bool onPlatform = false)
	{
		collisionController.UpdateRaycastDirections();
		collisionController.boxCollisionDirections.resetDirections();

		if(movement.x != 0)
		{
			collisionController.boxCollisionDirections.direction = (int)Mathf.Sign(movement.x);
		}

		if(movement.y < 0)
		{
			collisionController.DescendSlope(ref movement);
		}

		if(movement.x != 0 || movement.y != 0)
		{
			collisionController.checkCollision(ref movement);
		}
		
		if(onPlatform)
		{
			collisionController.boxCollisionDirections.down = true;	
		}

		transform.Translate(movement,Space.World);
	}
	
}
