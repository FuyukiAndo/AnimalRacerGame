using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerCollisionController))]

public class PlayerHandler : MonoBehaviour 
{
	[SerializeField] private float height = 4.0f;
	[SerializeField] private float fallDelay  = .4f;
	
	[HideInInspector] public Vector2 movement;
	
	[HideInInspector] public bool translateHorizontal = false;	
	[HideInInspector] public bool restricted = false;
	
	[HideInInspector] public PlayerCollisionController CollisionController;
	
	[HideInInspector] public static PlayerHandler player;
	
	public float moveSpeed = 1.2f;
	
	private float gravity;
	private float velocity;
	private float horizontalInput;
	private float verticalInput;
	public float wallSlideSpeedMax = 3;
	private float velocitySmoothing;
	public float movementAcceleration = 0.3f;
	private bool translateVertical = false;
	
	void Awake()
	{
		player = this;
	}
	
	void Start() 
	{
		CollisionController = GetComponent<PlayerCollisionController>();

	}
	
	void UpdateGravity()
	{
		gravity = -(2*height)/Mathf.Pow(fallDelay, 2);
		velocity = Mathf.Abs(gravity) * fallDelay;
	}
	
	void Update()
	{	
		UpdateGravity();
		
		bool wallSliding = false;
		
		if((CollisionController.BoxCollision.left || CollisionController.BoxCollision.right) && !CollisionController.BoxCollision.below && velocity < 0)
		{
				wallSliding = true;
				
				if(movement.y < -wallSlideSpeedMax)
				{
					movement.y = 2*wallSlideSpeedMax;
				}
					
		}
		
		float verticalTranslate = gravity * Time.deltaTime;
		
		horizontalInput = Input.GetAxisRaw("Horizontal");
		verticalInput = Input.GetAxisRaw("Jump");	
			
		if (movement.x == 0 && CollisionController.BoxCollision.above ||CollisionController.BoxCollision.below)
		{
			//movement.y = 0;
			translateVertical = false;
			translateHorizontal = false;
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
		
		
		if (movement.x == 0 && CollisionController.BoxCollision.above||CollisionController.BoxCollision.below)
		{
			movement.y = 0;
		}
		
		
		if(horizontalInput == 0 && CollisionController.BoxCollision.below)
		{	
			movement.x = 0;
			//movement.y = 0;
		}
		
		
		//if (restricted == false)
		//{	
			if(horizontalInput == 1)
			{
				//if (translateHorizontal == false)
				//{
					//if (CollisionController.BoxCollision.below)
					//{
						movement.x = 1 * moveSpeed;
					//}				
				//}
			}

			if(horizontalInput == -1)
			{
				//if (translateHorizontal == false)
				//{
					//if (CollisionController.BoxCollision.below)
					//{
						movement.x = -1 * moveSpeed;			
					//}
				//}
			}
		//}
			
		if (verticalInput == 1 && CollisionController.BoxCollision.below || CollisionController.BoxCollision.above)
		{
			movement.y = velocity;	
				
			/*	translateVertical = true;
				translateHorizontal = true;	
				
				if (horizontalInput == 0 && !CollisionController.BoxCollision.above)
				{
					movement.y = velocity;
						
					if (restricted == false)
					{
						movement.x = 0.0f;
					}
				}
				
				if (!CollisionController.BoxCollision.horizontal)
				{						
					if (horizontalInput == 1)
					{
						movement.y = velocity;
					}
				
					if (horizontalInput == -1)
					{
						movement.y = velocity;
					}
				}*/	
		}
		
		//gravity in y-axis
		float movementVelocity = Input.GetAxisRaw("Horizontal") * moveSpeed;
		movement.x = Mathf.SmoothDamp(movement.x, movementVelocity, ref velocitySmoothing,movementAcceleration);
		movement.y += verticalTranslate;
		CollisionController.Move(movement * Time.deltaTime);
	}
	
}