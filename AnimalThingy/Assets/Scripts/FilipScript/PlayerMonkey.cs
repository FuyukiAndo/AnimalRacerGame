using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonkey : PlayerController 
{	
	public float wallClimbingSpeed = 5f;
	public float leapDistance = 19f;
	private bool canThrow = true;

	public GameObject projectileObject;
	
	private Vector2 surfaceLeap;
	
	public override void Start()
	{
		base.Start();
	}

	public override void gravityTranslate()
	{
		if(!isActiveAbility)
		{
			base.gravityTranslate();	
		}
	}	
	
	public override void MoveNot()
	{
		if(!isActiveAbility)
		{
			base.MoveNot();
		}
		else
		{
			if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right) && !activeAbility)
			{
				movement.y = 0;
				movementDirection = 0;
			}
		}
	}
	
	public override void MoveLeft()
	{
		if(!isActiveAbility)
		{
			base.MoveLeft();	
		}
		else
		{
			activeAbility = true;
			
			if(abilityDirection == 1)
			{
				movement.y = wallClimbingSpeed;	
			}	
			else
			{
				movement.y = -wallClimbingSpeed;				
			}
		}		
	}
	
	public override void MoveRight()
	{	
		if(!isActiveAbility)
		{
			base.MoveRight();		
		}
		else
		{
			activeAbility = true;
			
			if(abilityDirection == 1)
			{
				movement.y = -wallClimbingSpeed;	
			}	
			else
			{
				movement.y = wallClimbingSpeed;				
			}
		}
	}

	public override void OnJumpKeyDown()
	{
		if(isActiveAbility)
		{
			movement.x = abilityDirection * surfaceLeap.x;
		}
		else
		{
			base.OnJumpKeyDown();
		}
	}
	
	public override void OnJumpKeyUp()
	{
		if(!isActiveAbility)
		{
			base.OnJumpKeyUp();
		}
	}	
	
	public override void OnAbilityKey()
	{
		base.OnAbilityKey();
	
		if(!isActiveAbility)
		{
			if(canThrow)
			{
				maxUseCounter--;
				Instantiate(projectileObject, new Vector2(transform.position.x+(2.5f*-abilityDirection),transform.position.y+2.5f), new Quaternion(0, 0, 0, 0), gameObject.transform);
			}
		}
	}	

	public override  void OpponentProjectileCollision()
	{
		base.OpponentProjectileCollision();
	}

	private void PlayerPassiveAbility()
	{		
		if(!isPassiveAbility && passiveAbility)
		{	
			if(maxUseCounter < 1)
			{
				canThrow = false;
				isPassiveAbility = true;
				passiveAbility = false;
				abilityMeter = 0;
				maxUseCounter = savedMaxUseCounter;
			}
		}
		
		if(abilityMeter == 1)
		{
			canThrow = true;
		}
	}

	private void PlayerActiveAbility()
	{
		surfaceLeap = new Vector2(leapDistance, 17f);
		
		if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right))
		{
			if(!collisionController.boxCollisionDirections.down)
			{
				if(movement.y < 0)
				{
					isActiveAbility  = true;
					playerInput.changeAngle = false;
				}
			}
		}
		
		if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right))
		{
			if(collisionController.boxCollisionDirections.down)
			{
				isActiveAbility = false;
				activeAbility = false;
				playerInput.changeAngle = true;
			}
		}

		if(!(collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right))
		{
			isActiveAbility = false;
			activeAbility = false;
			playerInput.changeAngle = true;
		}
	}
	
	public override void Update()
	{
		base.Update();
		PlayerActiveAbility();
		PlayerPassiveAbility();
	}	
}
