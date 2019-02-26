using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
	playerNobody,
	playerAlbatross,
	playerPig,
	playerMonkey,
	playerPenguin
};

[RequireComponent(typeof(CollisionController))]

public class PlayerController : MonoBehaviour
{
	[Header("Player Type Settings")]
	public PlayerType playerType;

	[Header("Movement Settings")]
	public float movementSpeed = 18.0f;
	[Range(0.1f,1.0f)]public float movementAcceleration = 0.15f;

	//References for CollisionController class and PlayerController class
	[HideInInspector] public CollisionController collisionController;
	[HideInInspector] public GravityController gravityController;
	
	[HideInInspector] public static PlayerController playerController;

	//private float horizontalInput;
	private float velocitySmoothing;
	[HideInInspector] public Vector2 movement;
	[HideInInspector] public int direction;
	
	//Only public for debug
	public float tempSpeed;
	public float mod0 = 0.1f;
	public float mod1 = 0.2f;

	void Awake()
	{
		playerController = this;
	}

	void Start()
	{
		collisionController = GetComponent<CollisionController>();
	}

	void OnValidate()
	{
		if(movementSpeed < 1.0f)
		{
			//Debug.LogWarning("'minJumpHeight' cannot be lower than 0");
			movementSpeed = 1.0f;
		}

		if(direction > 1)
		{
			maxJumpHeight = minJumpHeight + 0.1f;
		}
	}

	//If jump button released before reaching max value, then goto min value.
	public void OnJumpKeyUp()
	{
		if(!collisionController.boxCollisionDirections.down)
		{
			if(movement.y > gravityController.minVelocity)
			{
				movement.y = gravityController.minVelocity;
			}
		}
	}

	//If pressed down, then goes to max value.
	public void OnJumpKeyDown()
	{
		if(collisionController.boxCollisionDirections.down)
		{
			movement.y = gravityController.maxVelocity;
		}
	}
	
	//Move player Left direction with smooth acceleration
	public void MoveLeft()
	{
		//Smooth movement acceleration
		for(float i = 0; i <movementSpeed;i++)
		{
			movement.y = 0;
		}

		//Translation in Right direction
		movement.x = 1 * tempSpeed;

		//Set direction to Right 
		direction = 1;
	}
	
	public void MoveNot()
	{
		//Smooth deacceleration
		tempSpeed=tempSpeed-0.2f;
		
		//When fully deaccelerated, movement speed is 0
		if(tempSpeed < 0)
		{
			tempSpeed = 0;
		}

	void Update()
	{
		gravityController.UpdateGravity();
		
		float verticalTranslate = gravityController.gravity * Time.deltaTime;
		
		movement.y += verticalTranslate;
		//collisionController.Move(movement * Time.deltaTime);
		MoveObject(movement * Time.deltaTime);
		
		bool wallSliding = false;

		if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right) 
			&& !collisionController.boxCollisionDirections.down && gravityController.maxVelocity < 0)
		{
			if(collisionController.boxCollisionDirections.down)
			{
				movement.y = maxVelocity;
			}
		}

		if (collisionController.boxCollisionDirections.up || collisionController.boxCollisionDirections.down)
		{
			if(!collisionController.boxCollisionDirections.down)
			{
				if(movement.y > minVelocity)
				{
					movement.y = minVelocity;
				}
			}
		}

	}

	public void MoveObject(Vector2 movement)
	{
		collisionController.UpdateRaycastDirections();
		collisionController.boxCollisionDirections.resetDirections();

		if(movement.y < 0)
		{
			collisionController.DescendSlope(ref movement);
		}
		if(movement.x != 0 || movement.y != 0)
		{
			collisionController.checkCollision(ref movement);
		}

		transform.Translate(movement);
	}
}
