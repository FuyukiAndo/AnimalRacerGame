using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum PlayerType
{
	playerNobody,
	playerAlbatross,
	playerPig,
	playerMonkey,
	playerPenguin
};*/

[RequireComponent(typeof(CollisionController))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
	//public PlayerStates playerStates;
	
	[Header("Jump and Gravity Settings")]

	public float maxJumpHeight = 8.0f;
	public float minJumpHeight = 1f;
	[Range(0.1f,6.0f)]public float jumpAndFallDelay = 0.4f;
	
	[HideInInspector] public float gravity;
	[HideInInspector] public float maxVelocity;
	[HideInInspector] public float minVelocity;

	//public PlayerType playerType;

	[Header("Movement Settings")]
	public float movementSpeed = 18.0f;
	[Range(0.1f,1.0f)]public float movementAcceleration = 0.15f;

	[HideInInspector] public CollisionController collisionController;
	RaycastController raycastController;

	protected float velocitySmoothing;
	protected float wallSlideSpeedMax = 3;
	
	[HideInInspector] public Vector2 movement;
	protected int movementDirection;
	protected int abilityDirection;

	protected float tempSpeed;
	protected float mod0 = 0.1f;
	protected float mod1 = 0.2f;
	
	[HideInInspector] public int abilityMeter = 100;
	[HideInInspector] public int abilityTimer = 3;
	public float stunDuration = 10f;
	
	protected PlayerInput playerInput;
	public Collider2D[] collision;	
	
	public virtual void Start()
	{
		collisionController = GetComponent<CollisionController>();
		raycastController = GetComponent<RaycastController>();
		playerInput = GetComponent<PlayerInput>();
		
		movementDirection = 0;
		abilityDirection = 0;
	}

	public void UpdateGravity()
	{
		gravity = -(2 * maxJumpHeight) / Mathf.Pow(jumpAndFallDelay, 2);
		maxVelocity = Mathf.Abs(gravity) * jumpAndFallDelay;
		minVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}

	void OnValidate()
	{
		if(minJumpHeight < 0.1f)
		{
			minJumpHeight = 0.1f;
		}

		if(maxJumpHeight < 0.1f)
		{
			maxJumpHeight = 0.1f;
		}
		
		if(maxJumpHeight < minJumpHeight)
		{
			maxJumpHeight = minJumpHeight + 0.1f;
		}
		
		if(movementSpeed < 1.0f)
		{
			movementSpeed = 1.0f;
		}
	}
	
	//If jump button released before reaching max value, then goto min value.
	public void OnJumpKeyUp()
	{
		if(!collisionController.boxCollisionDirections.down)
		{
			if(movement.y > minVelocity)
			{
				movement.y = minVelocity;
			}
		}
	}

	//If pressed down, then goes to max value.
	public void OnJumpKeyDown()
	{
		if(collisionController.boxCollisionDirections.down)
		{
			movement.y = maxVelocity;
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
		movementDirection = -1;
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
		movementDirection = 1;
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
		float movementVelocity = movementDirection * tempSpeed;
		movement.x = Mathf.SmoothDamp(movement.x, movementVelocity, ref velocitySmoothing,movementAcceleration);
		
		//Set direction to None
		movementDirection = 0;
	}

    void RecoverFromStun()
    {
        stunDuration -= Time.deltaTime;
    } 

	public virtual void Update()
	{
		UpdateGravity();
		
		float verticalTranslate = gravity * Time.deltaTime;
		
		movement.y += verticalTranslate;
		
		if(stunDuration < 0)
        {
            RecoverFromStun();
        }
        else
        {
			MoveObject(movement * Time.deltaTime);
        }
		
		bool wallSliding = false;

		/*if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right) 
			&& !collisionController.boxCollisionDirections.down && maxVelocity < 0)
		{
			wallSliding = true;

			if(movement.y < -wallSlideSpeedMax)
			{
				movement.y = 0.1f;//2*wallSlideSpeedMax;
			}
		}*/

		if (collisionController.boxCollisionDirections.up || collisionController.boxCollisionDirections.down)
		{
			movement.y = 0;
		}
		
		
		if(playerInput.targetAngle == playerInput.maxAngleValue)
		{
			abilityDirection = 1;
		}
		else
		{
			abilityDirection = -1;
		}
	
		WindblastCollision();
	}
	
	void WindblastCollision()
	{
		collision = Physics2D.OverlapCircleAll(transform.position, 1.5f);

		for(int i = 0; i < collision.Length; i++)
		{
			if(collision[i].gameObject.tag == "Blast")
			{
				Debug.Log("this works");
				//Call on Stun function
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

		transform.Translate(movement,Space.World);
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
	
	public int GetDirection()
	{
		return abilityDirection;
	}
}
