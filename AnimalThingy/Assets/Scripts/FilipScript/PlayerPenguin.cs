using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPenguin : PlayerController
{
	//public bool isSlide = false;
	//public bool activeSlide = false;
	
	//public int i;
	//int count = 4;
	//public float abilityModifier = 2f;

	public override void Start() 
	{
		base.Start();
			//i = count;
	}

	public override void Update() 
	{
		base.Update();
		
		if(isPassiveAbility && !passiveAbility)//isSlide && !activeSlide)
		{
			movement.y = minVelocity;
			movement.x += -1 * abilityDirection * abilityModifier;
			maxUseCounter--;//i--;
			
			if(maxUseCounter < 0)
			{
				isPassiveAbility = false;//isSlide = false;
				passiveAbility = true;//activeSlide = true;
				maxUseCounter = savedMaxUseCounter;
				//i = count;
			}
		}
		
		if(!isPassiveAbility && passiveAbility)
		{
			movement.y = 0;
			movement.x += -1 * abilityDirection * abilityModifier;
		}
	}
	
	public override void OnAbilityKey()
	{
		base.OnAbilityKey();
		
		playerInput.isControllable = false;
		playerInput.changeAngle = false;
		
		if(!isPassiveAbility)
		{
			isPassiveAbility = true;
		}
	}
}
