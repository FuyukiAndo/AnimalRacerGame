using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonkey : PlayerController 
{
	GameObject projectileObject;
	public float wallClimbingSpeed = 5f;
	
	public bool isClimbing = false;
	public bool activeClimbing = false;
	public Vector2 surfaceOn;
	public Vector2 surfaceOff;
	public Vector2 surfaceLeap;
	int dirX;	
	public float directionX;
	
	public override void Start()
	{
		base.Start();
		projectileObject = Resources.Load<GameObject>("Prefabs/BananaGameObject");
	}
	
	public override void MoveLeft()
	{
		if(!isClimbing)
		{
			base.MoveLeft();	
		}
		else
		{
			activeClimbing = true;
			
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
		if(!isClimbing)
		{
			base.MoveRight();		
		}
		else
		{
			activeClimbing = true;
			
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
	
	public override void MoveNot()
	{
		base.MoveNot();
	}	
	
	public override void OnJumpKeyUp()
	{
		if(isClimbing)
		{
			
		}
		else
		{
			base.OnJumpKeyUp();
		}
	}	

	//If pressed down, then goes to max value.
	public override void OnJumpKeyDown()
	{
		if(isClimbing)
		{
			movement.x = -dirX * surfaceLeap.x;
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
		if(!activeClimbing)
		{
			base.gravityTranslate();//movement.y += gravity * Time.deltaTime;//verticalTranslate;		
		}
	}

	public override void Update()
	{
		base.Update();
		directionX = Mathf.Sign(movement.x);
		
		if(collisionController.boxCollisionDirections.left)
		{
			dirX = -1;
		}
		else
		{
			dirX = 1;
		}

		if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right)
		&& !collisionController.boxCollisionDirections.down && movement.y < 0)
		{
			isClimbing  = true;
	
			//if(!activeClimbing)
			//{
				if(movement.y < -wallClimbingSpeed)
				{
					movement.y = -wallClimbingSpeed;
				}
			//}
		}
		
		if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right)
		&& collisionController.boxCollisionDirections.down)
		{
			isClimbing = false;
			activeClimbing = false;
		}

		if(!(collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right))
		{
			isClimbing = false;
			activeClimbing = false;
		}
	}
	
	public override void OnAbilityKey()
	{
		base.OnAbilityKey();
	
		if(!isClimbing)
		{
			Instantiate(projectileObject, new Vector2(transform.position.x+(2.5f*abilityDirection),transform.position.y+2f), new Quaternion(0, 0, 0, 0), gameObject.transform);		
		}
	}
	
}
