using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionController))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{	
	[Header("Jump and Gravity Settings")]
	public float maxJumpHeight = 8.0f;
	public float minJumpHeight = 1.0f;
	[Range(.01f,1f)] public float jumpAndFallDelay = 0.4f;

	[Header("Movement Settings")]
	public float movementSpeed = 18.0f;
	[Range(.01f,1f)] public float movementAcceleration = 0.15f;
	[Range(.01f,1f)] public float accelerationModifier = 0.1f;
	[Range(.01f,1f)] public float deaccelerationModifier = 0.1f;

	[Header("Ability Meter Settings")]
	[Range(0,1f)] public float abilityMeter = 1f;
	[Range(0.01f,.1f)] public float abilityMeterModifier = 0.01f;	
	
	[Header("Character Settings")]
	public int maxUseCounter = 3;
	public float abilityModifier = 2.0f;
	public int abilityDirection;

	[HideInInspector] public CollisionController collisionController;
	
	[HideInInspector] public Vector2 movement;
	
	public bool isActiveAbility = false;
	public bool isPassiveAbility = false;
	public bool activeAbility = false;
	public bool passiveAbility = false;
	public bool isJumping = false;

	[HideInInspector] public Collider2D[] collision;	
	
	protected int savedMaxUseCounter;
	protected float abilityMeterMax = 1f;
	
	protected float maxVelocity;
	protected float minVelocity;

	protected PlayerInput playerInput;
	protected RaycastController raycastController;

	protected float accelerationSpeed;
	
	protected int movementDirection;
	
	protected float velocitySmoothing;	
	protected float gravity;
	
	protected float directionX;
	protected float directionY;
	
	protected float startSpeed;
	
	public virtual void Start()
	{
		collisionController = GetComponent<CollisionController>();
		raycastController = GetComponent<RaycastController>();
		playerInput = GetComponent<PlayerInput>();
		
		movementDirection = (int)Mathf.Sign(movement.x);
		abilityDirection = (int)Mathf.Sign(transform.rotation.x);//movement.x);
		savedMaxUseCounter = maxUseCounter;
		abilityMeterMax = abilityMeter;
		
		startSpeed = movementSpeed;
	}

	public void UpdateGravity()
	{
		gravity = -(2 * maxJumpHeight) / Mathf.Pow(jumpAndFallDelay, 2);
		maxVelocity = Mathf.Abs(gravity) * jumpAndFallDelay;
		minVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}
	
	public virtual void gravityTranslate()
	{
		movement.y += gravity * Time.deltaTime;	
	}
	
	public virtual void MoveNot()
	{
		accelerationSpeed = accelerationSpeed - deaccelerationModifier;

		if(accelerationSpeed < 0)
		{
			accelerationSpeed = 0;
		}

		float movementVelocity = movementDirection * accelerationSpeed;
		movement.x = Mathf.SmoothDamp(movement.x, movementVelocity, ref velocitySmoothing,movementAcceleration);

		movementDirection = 0;
	}	
	
	public virtual void MoveLeft()
	{
		for(float i = 0; i <movementSpeed;i++)
		{
			accelerationSpeed = accelerationSpeed + accelerationModifier;
			
			if(accelerationSpeed > movementSpeed)
			{
				accelerationSpeed = movementSpeed;
			}
		}
	
		movement.x = -1 * accelerationSpeed;

		movementDirection = -1;
		abilityDirection = 1;
	}

	public virtual void MoveRight()
	{
		for(float i = 0; i < movementSpeed;i++)
		{
			accelerationSpeed = accelerationSpeed + accelerationModifier;
			
			if(accelerationSpeed > movementSpeed)
			{
				accelerationSpeed = movementSpeed;
			}
		}
		
		movement.x = 1 * accelerationSpeed;

		movementDirection = 1;
		abilityDirection = -1;
	}

	public virtual void OnJumpKeyDown()
	{
		if(collisionController.boxCollisionDirections.down)
		{
			movement.y = maxVelocity;
		}
	}

	//If jump button released before reaching max value, then goto min value.
	public virtual void OnJumpKeyUp()
	{
		if(!collisionController.boxCollisionDirections.down)
		{
			if(movement.y > minVelocity)
			{
				movement.y = minVelocity;
			}
		}
	}
	
	public virtual void OnAbilityKey()
	{
		if(!passiveAbility && !isPassiveAbility)
		{
			passiveAbility = true;
		}
	}	

	public virtual void OpponentProjectileCollision()
	{
		LayerMask projectileMask = LayerMask.NameToLayer("Projectile");
		collision = Physics2D.OverlapCircleAll(transform.position, 1.5f);//, projectileMask);

		for(int i = 0; i < collision.Length; i++)
		{
			if(collision[i].gameObject.tag == "Projectile")//layer == LayerMask.GetMask("Projectile"))?
			{
				playerInput.isStunned = true;
				Destroy(collision[i].gameObject);
			}
			
			if(collision[i].gameObject.tag == "Obstacle" && playerInput.playerCharacterType != PlayerCharacterType.PlayerPenguin)
			{
				playerInput.isControllable = false;
				movement.x = -abilityDirection * movementSpeed*1.4f;
			}
			
			if(!collision[i].gameObject)
			{
				playerInput.isControllable = true;
			}
		}

	}

	public void MoveObject(Vector2 movement, bool onPlatform = false)
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
	
	private void AbilityHandler()
	{
		/*if(playerInput.changeAngle)
		{
			if(playerInput.targetAngle == playerInput.GetMaxAngleValue())
			{
				abilityDirection = 1;
			}
			else
			{
				abilityDirection = -1;
			}
		}*/
	
		if(abilityMeter < 1f && isPassiveAbility)
		{	
			abilityMeter = abilityMeter + abilityMeterModifier;	
		}
		
		if(abilityMeter > 1f)
		{
			abilityMeter = 1f;
			isPassiveAbility = false;
			passiveAbility = false;
		}
	}
	
	public virtual void Update()
	{
		UpdateGravity();
		gravityTranslate();
		
		MoveObject(movement * Time.deltaTime);

		if (collisionController.boxCollisionDirections.up || collisionController.boxCollisionDirections.down)
		{
			movement.y = 0;
		}	
		
		OpponentProjectileCollision();
		AbilityHandler();
	}	
	
	public int GetDirection()
	{
		return abilityDirection;
	}
	
	public IEnumerator movemenetSpeedChanger(float speed, float duration)
	{
		movementSpeed += speed;
		yield return new WaitForSeconds(duration);
		movementSpeed = startSpeed;
	}

	
	/* Unity Editor related section */
	
	private void OnValidate()
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
	
	private void OnDrawGizmosSelected()
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
	
	/* Unity Editor related section */
	
}
