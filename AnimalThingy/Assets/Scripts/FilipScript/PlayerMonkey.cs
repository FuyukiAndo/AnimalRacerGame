using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonkey : PlayerController 
{	
	public float wallClimbingSpeed = 5f;
	public float leapDistance = 19f;

	public GameObject projectileObject;
	
	private Vector2 surfaceLeap;
	private int dirX;
	
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
			
			if(dirX == 1)
			{
				movement.y = -wallClimbingSpeed;	
			}	
			else
			{
				movement.y = wallClimbingSpeed;				
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
			
			if(dirX == 1)
			{
				movement.y = wallClimbingSpeed;	
			}	
			else
			{
				movement.y = -wallClimbingSpeed;				
			}
		}
	}

	public override void OnJumpKeyDown()
	{
		if(isActiveAbility)
		{
			movement.x = -dirX * surfaceLeap.x;
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
		/*if(isActiveAbility)
		{
			
		}
		else
		{
			base.OnJumpKeyUp();
		}*/
	}	
	
	public override void OnAbilityKey()
	{
		base.OnAbilityKey();
	
		//playerInput.isControllable = false;
		//playerInput.changeAngle = false;
	
		if(!isActiveAbility)
		{
			Instantiate(projectileObject, new Vector2(transform.position.x+(2.5f*-abilityDirection),transform.position.y+2f), new Quaternion(0, 0, 0, 0), gameObject.transform);		
		}
	}	
	
	private void PlayerPassiveAbility()
	{
		if(collisionController.boxCollisionDirections.left)
		{
			dirX = -1;
		}
		else
		{
			dirX = 1;
		}
	}

	private void PlayerActiveAbility()
	{
		surfaceLeap = new Vector2(leapDistance, 17f);
		
		if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right) && !collisionController.boxCollisionDirections.down && movement.y < 0)
		{
			isActiveAbility  = true;
			playerInput.changeAngle = false;
			//if(!activeClimbing)
			//{
				/*if(movement.y < -wallClimbingSpeed)
				{
					movement.y = -wallClimbingSpeed;
				}*/
			//}
		}
		
		if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right) && collisionController.boxCollisionDirections.down)
		{
			isActiveAbility = false;
			activeAbility = false;
			playerInput.changeAngle = true;
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
		
		PlayerPassiveAbility();
		PlayerActiveAbility();
	}	
}
