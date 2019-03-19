using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlbatross : PlayerController 
{
	public float maxFlyCounter = 3f;	
	public float untilGlideCounter = 1.0f;
	
	[Range(0.1f,10.0f)] public float glideSpeed = 4.0f;

	public GameObject windBlastObject; 

	[Header("Parameter Counter Settings")]	
	public float maxFlyCountdownSpeed = 0.1f;
	public float glideCounterSpeed = 1.4f;	
	
	private float savedMaxFlyCounter;
	private float savedJumpAndFallDelay;
	private float savedUntilGlideCounter;
	
	public override void Start()
	{
		base.Start();
		
		savedUntilGlideCounter = untilGlideCounter;
		savedMaxFlyCounter = maxFlyCounter;
		savedJumpAndFallDelay = jumpAndFallDelay;
	}
	
	private void PlayerPassiveAbility()
	{
		if(passiveAbility)
		{
			passiveAbility = false;
			isPassiveAbility = true;
		}
		
		if(!passiveAbility && isPassiveAbility)
		{
			movement.x += -1 * abilityDirection * abilityModifier;
		}		
		
		if(!passiveAbility && !isPassiveAbility)
		{
			playerInput.isControllable = true;
			playerInput.changeAngle = true;
		}
	}

	private void PlayerActiveAbility()
	{
		if(activeAbility)
		{
			untilGlideCounter -= Mathf.Abs(glideCounterSpeed) * Time.deltaTime;
			isActiveAbility = false;
		}
		
		if(isActiveAbility)
		{
			directionY = Mathf.Sign(movement.y);
			
			if(directionY == -1)
			{
				jumpAndFallDelay = glideSpeed;	
			}
		}
		else
		{
			jumpAndFallDelay = savedJumpAndFallDelay;
		}
		
		if(untilGlideCounter < 0)
		{
			activeAbility = false;	
			isActiveAbility = true;
			untilGlideCounter = savedUntilGlideCounter;
		}
		if(isJumping)
		{
			maxFlyCounter = maxFlyCounter-maxFlyCountdownSpeed;
			
			if(maxFlyCounter < 0)
			{
				maxFlyCounter = savedMaxFlyCounter;
				maxUseCounter--;
				isJumping = false;
			}
		}
		
		if(collisionController.boxCollisionDirections.down)
		{
			maxUseCounter = savedMaxUseCounter;
		}
		
		if(maxUseCounter == 0)
		{
			maxUseCounter = 0;
	
			if(collisionController.boxCollisionDirections.down)
			{
				maxUseCounter = savedMaxUseCounter;
			}
		}		
	}
	
	public override void OnJumpKeyDown()
	{
		if(!isActiveAbility)
		{	
			activeAbility = true;
			
			if(maxUseCounter !=0)
			{
				if (maxFlyCounter == savedMaxFlyCounter)
				{
					isJumping = true;
					movement.y = maxVelocity;
				}
			}
		}
	}
	
	public override void OnJumpKeyUp()
	{
		isActiveAbility = false;
		activeAbility = false;
				
		if(movement.y > minVelocity)
		{
			movement.y = minVelocity;
		}
	}
	
	public override void OnAbilityKey()
	{
		base.OnAbilityKey();
	
		playerInput.isControllable = false;
		playerInput.changeAngle = false;

		Instantiate(windBlastObject, new Vector2(transform.position.x+(2.5f*abilityDirection),transform.position.y+2f), new Quaternion(0, 0, 0, 0), gameObject.transform);
		windBlastObject.transform.parent = null;
	}
	
	public override void Update()
	{
		base.Update();	
		UpdateGravity();		
		
		PlayerActiveAbility();
		PlayerPassiveAbility();
	}	
}