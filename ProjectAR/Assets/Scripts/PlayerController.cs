using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionController))]

public class PlayerController : MonoBehaviour 
{		
	public CollisionController collisionController;
	public static PlayerController playerController;
	
	[Tooltip("Max jump height value between 0.1f and x")]
	public float maxJumpHeight = 4.0f;
	
	[Tooltip("Min jump height value between 0.1f and x")]
	public float minJumpHeight = 1.0f;
	
	[Tooltip("Min jump height value between 0.1f and x")]
	[Range(0.1f,1.0f)]public float jumpAndFallSpeed = .4f;
	
	//public bool translateHorizontal = false;	
	//public bool restricted = false;
	
	public float movementSpeed = 1.2f;
	public float movementAcceleration = 0.3f;
		
	private float gravity;
	private float maxVelocity;
	private float minVelocity;
	
	private float horizontalInput;
	//private float verticalInput;
	public float wallSlideSpeedMax = 3;
	private float velocitySmoothing;

	//private bool translateVertical = false;
	
	[HideInInspector] public Vector2 movement;
	
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
		
	}
	
	void UpdateGravity()
	{
		gravity = -(2*maxJumpHeight)/Mathf.Pow(jumpAndFallSpeed, 2);
		maxVelocity = Mathf.Abs(gravity) * jumpAndFallSpeed;
		minVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);
	}
	
	void PlayerInput()
	{
		horizontalInput = Input.GetAxisRaw("Horizontal");
	//	verticalInput = Input.GetAxisRaw("Jump");	
	}
	
	void Update()
	{	
		UpdateGravity();
		//PlayerInput();
		
		bool wallSliding = false;
		
		if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right) && !collisionController.boxCollisionDirections.down && maxVelocity < 0)
		{
			wallSliding = true;
				
			if(movement.y < -wallSlideSpeedMax)
			{
				movement.y = 2*wallSlideSpeedMax;
			}					
		}
			
		if (collisionController.boxCollisionDirections.up||collisionController.boxCollisionDirections.down)
		{
			//translateVertical = false;
			//translateHorizontal = false;
		}
		
		if (collisionController.boxCollisionDirections.up || collisionController.boxCollisionDirections.down)
		{
			movement.y = 0;
		}

		
		/*if(horizontalInput == 0 && CollisionController.BoxCollision.below)
		{	
			if (restricted == false)
			{
				movement.x = 0;
				movement.y = 0;
			}
		}
		*/
		
		/*if(CollisionController.BoxCollision.below)
		{	
			movement.y = 0;
			movement.x = 0;
		}*/
		
		if(horizontalInput == -1)
		{
			movement.x = -1 * movementSpeed;	
		}

		if(horizontalInput == 1)
		{
			movement.x = 1 * movementSpeed;	
		}
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(collisionController.boxCollisionDirections.down)
			{
				movement.y = maxVelocity;
			}
		}
		
		if(Input.GetKeyUp(KeyCode.Space))
		{
			if(!collisionController.boxCollisionDirections.down)
			{
				if(movement.y > minVelocity)
				{
					movement.y = minVelocity;
				}
			}
		}
		
		//gravity in y-axis
		float movementVelocity = Input.GetAxisRaw("Horizontal") * movementSpeed;
		movement.x = Mathf.SmoothDamp(movement.x, movementVelocity, ref velocitySmoothing,movementAcceleration);
		
		float verticalTranslate = gravity * Time.deltaTime;
		movement.y += verticalTranslate;
		
		//collisionController.Move(movement * Time.deltaTime);
		MovePlayer(movement * Time.deltaTime);
	}
	
	public void MovePlayer(Vector2 movement)
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
