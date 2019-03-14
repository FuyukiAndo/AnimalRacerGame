using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonkey : PlayerController 
{
	public GameObject projectileObject;
	public float climbingSpeed = 5f;
	
	public Vector2 surfaceOn;
	public Vector2 surfaceOff;
	public Vector2 surfaceLeap;
	
	public override void Start()
	{
		base.Start();
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
			
			if(directionX == 1)
			{
				movement.y = -climbingSpeed;	
			}	
			else
			{
				movement.y = climbingSpeed;				
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
			
			if(directionX == 1)
			{
				movement.y = climbingSpeed;	
			}	
			else
			{
				movement.y = -climbingSpeed;				
			}
		}
	}
	
	public override void MoveNot()
	{
		base.MoveNot();
	}	
	
	public override void OnJumpKeyUp()
	{
		if(!isActiveAbility)
		{
			base.OnJumpKeyUp();
		}
	}	

	public override void OnJumpKeyDown()
	{
		if(isActiveAbility)
		{
			movement.x = -directionX * surfaceLeap.x;
				//movement.y = surfaceLeap.y;
			
			/*if(dirX == abilityDirection)
			{
				movement.x = -dirX * surfaceOn.x;
				movement.y = surfaceOn.y;
			}
			else if(movement.x == 0)
			{
				movement.x = -dirX * surfaceOff.x;
				movement.y = surfaceOff.y;
			}
			else
			{
				movement.x = -dirX * surfaceLeap.x;
				movement.y = surfaceLeap.y;
			}*/
		}
		else
		{
			base.OnJumpKeyDown();
		}
	}
	
	public override void gravityTranslate()
	{
		if(!activeAbility)
		{
			base.gravityTranslate();		
		}
	}

	public override void Update()
	{
		base.Update();
		directionX = Mathf.Sign(movement.x);
		
		if(collisionController.boxCollisionDirections.left)
		{
			directionX = -1;
		}
		else
		{
			directionX = 1;
		}

		if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right)
		&& !collisionController.boxCollisionDirections.down && movement.y < 0)
		{
			isActiveAbility = true;
			activeAbility = true;
			
			playerInput.changeAngle = false;

			/*if(movement.y < -climbingSpeed)
			{
				movement.y = -climbingSpeed/2.0f;
			}*/
		}
		
		if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right)
		&& collisionController.boxCollisionDirections.down && movement.y == 0)
		{
			isActiveAbility = false;
			activeAbility = false;

			playerInput.changeAngle = true;
		}

		if(!(collisionController.boxCollisionDirections.left) || (collisionController.boxCollisionDirections.right))
		{
			isActiveAbility = false;
			activeAbility = false;
			
			playerInput.changeAngle = true;
		}
	}
	
	public override void OnAbilityKey()
	{
		base.OnAbilityKey();
	
		if(!isActiveAbility)
		{
			Instantiate(projectileObject, new Vector2(transform.position.x+(2.5f*abilityDirection),transform.position.y+2f), new Quaternion(0, 0, 0, 0), gameObject.transform);		
		}
	}
	
}
