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
[RequireComponent(typeof(GravityController))]
[RequireComponent(typeof(PlayerInput))]

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
	
	public float wallSlideSpeedMax = 3;
	
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
		gravityController = GetComponent<GravityController>();
		direction = 0;
	}

	void OnValidate()
	{
		if(movementSpeed < 1.0f)
		{
			movementSpeed = 1.0f;
		}
		
		if(direction > 1)
		{
			direction = 1;
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
			tempSpeed=tempSpeed+mod0;
			
			if(tempSpeed > movementSpeed)
			{
				tempSpeed = movementSpeed;
			}
		}

		//Translation in Left direction		
		movement.x = -1 * tempSpeed;
		
		//Set direction to Left
		direction = -1;
	}

	//Move player Right direction with smooth acceleration	
	public void MoveRight()
	{
		//Smooth movement acceleration
		for(float i = 0; i <movementSpeed;i++)
		{
			tempSpeed=tempSpeed+mod0;
			
			if(tempSpeed > movementSpeed)
			{
				tempSpeed = movementSpeed;
			}
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
	
		//SmoothDamp for deacceleratation
		float movementVelocity = direction * tempSpeed;
		movement.x = Mathf.SmoothDamp(movement.x, movementVelocity, ref velocitySmoothing,movementAcceleration);
		
		//Set direction to None
		direction = 0;
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
			wallSliding = true;

			if(movement.y < -wallSlideSpeedMax)
			{
				movement.y = 2*wallSlideSpeedMax;
			}
		}

		if (collisionController.boxCollisionDirections.up || collisionController.boxCollisionDirections.down)
		{
			movement.y = 0;
		}
	}

	public void MoveObject(Vector2 movement)
	{
		//Update collision check 
		
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