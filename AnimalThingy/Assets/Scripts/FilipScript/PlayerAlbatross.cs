using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlbatross : PlayerController 
{
	public GameObject windBlastObject; 

	public float flyCounter = 3f;	
	public float flyCountdownSpeed = 0.1f;
	
	public float untilGlideCounter = 1.0f;
	public float glideCounterSpeed = 1.4f;
	
	[Range(0.1f,6.0f)] public float glideSpeed = 4.0f;

	private float savedFlyCounter;
	private float savedJumpAndFallDelay;
	private float savedUntilGlideCounter;
	
	public override void Start()
	{
		base.Start();
		savedUntilGlideCounter = untilGlideCounter;
		savedFlyCounter = flyCounter;
		savedJumpAndFallDelay = jumpAndFallDelay;
	}
	
	public override void Update()
	{
		base.Update();	
		UpdateGravity();		
		
		if(activeAbility)
		{
			untilGlideCounter -= Mathf.Abs(glideCounterSpeed) * Time.deltaTime;
			isActiveAbility = false;
		}
		
		if(isActiveAbility)
		{
			float directionY = Mathf.Sign(movement.y);
			
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
		
		if(isPassiveAbility && passiveAbility)
		{
			movement.x += -1 * abilityDirection * abilityModifier;
			abilityMeter = abilityMeter + abilityTimer;
		}
		
		if(!passiveAbility)
		{
			isPassiveAbility = false;
			playerInput.isControllable = true;			
		}
		
		if(isJumping)
		{
			flyCounter = flyCounter-flyCountdownSpeed;
			
			if(flyCounter < 0)
			{
				flyCounter = savedFlyCounter;
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
				if (flyCounter == savedFlyCounter)
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
		
		passiveAbility = true;
		
		if(passiveAbility)
		{
			isPassiveAbility = true;
			playerInput.isControllable = false;
		}
		
		Instantiate(windBlastObject, new Vector2(transform.position.x+(2.5f*abilityDirection),transform.position.y+2f), new Quaternion(0, 0, 0, 0), gameObject.transform);
		windBlastObject.transform.parent = null;
	}
}
