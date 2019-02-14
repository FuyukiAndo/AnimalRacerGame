using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionController))]

public class PlayerController : MonoBehaviour 
{	
	public enum PlayerType
	{
		playerNobody,
		playerAlbatross,
		playerPig,
		playerMonkey,
		playerPenguin
	};
	
	[Header("Player Type Settings")]
	public PlayerType playerType;
	
	
	[HideInInspector] public CollisionController collisionController;
	[HideInInspector] public static PlayerController playerController;
	
	[Header("Jump and Gravity Settings")]
	[Tooltip("Max jump height value between 0.1f and x")]
	public float maxJumpHeight = 8.0f;
	
	[Tooltip("Min jump height value between 0.1f and x")]
	public float minJumpHeight = 0.1f;
	
	[Tooltip("Min jump height value between 0.1f and x")]
	[Range(0.1f,2.0f)]public float jumpAndFallDelay = 0.4f;
	
	[Header("Movement Settings")]
	public float movementSpeed = 18.0f;
	[Range(0.1f,1.0f)]public float movementAcceleration = 0.15f;
		
	private float gravity;
	private float maxVelocity;
	private float minVelocity;
	private float horizontalInput;
	[HideInInspector] public float wallSlideSpeedMax = 3;
	private float velocitySmoothing;
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
		if(minJumpHeight < 0.1f)
		{
			//Debug.LogWarning("'minJumpHeight' cannot be lower than 0");
			minJumpHeight = 0.1f;
		}
		
		if(maxJumpHeight < 0.1f)
		{
			//Debug.LogWarning("'minJumpHeight' cannot be lower than 0");
			maxJumpHeight = 0.1f;
		}

		if(movementSpeed < 1.0f)
		{
			//Debug.LogWarning("'minJumpHeight' cannot be lower than 0");
			movementSpeed = 1.0f;
		}
		
		if(maxJumpHeight < minJumpHeight)
		{
			maxJumpHeight = minJumpHeight + 0.1f;
		}
	}
	
	void UpdateGravity()
	{
		gravity = -(2*maxJumpHeight)/Mathf.Pow(jumpAndFallDelay, 2);
		maxVelocity = Mathf.Abs(gravity) * jumpAndFallDelay;
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
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(gameObject.transform.position.x-4, gameObject.transform.position.y + maxJumpHeight, 0), 
						new Vector3(gameObject.transform.position.x+4,gameObject.transform.position.y  + maxJumpHeight, 0));
						
		Gizmos.color = Color.green;
		Gizmos.DrawLine(new Vector3(gameObject.transform.position.x-4, gameObject.transform.position.y + minJumpHeight, 0), 
						new Vector3(gameObject.transform.position.x+4,gameObject.transform.position.y  + minJumpHeight, 0));
		
		Gizmos.color = Color.blue;						
		Gizmos.DrawLine(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0), 
						new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + maxJumpHeight, 0));
	}
	
	
}
