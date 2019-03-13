using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPenguin : PlayerController
{
	public bool isSlide = false;
	public bool activeSlide = false;
	
	public int i;
	int count = 4;
	//public float abilityModifier = 2f;

	public override void Start() 
	{
		base.Start();
			i = count;
	}

	public override void Update() 
	{
		base.Update();
		
		if(isSlide && !activeSlide)
		{
			movement.y = minVelocity;
			movement.x += -1 * abilityDirection * 2.0f;
			i--;
			
			if(i < 0)
			{
				isSlide = false;
				activeSlide = true;
				i = count;
			}
		}
		
		if(!isSlide && activeSlide)
		{
			movement.y = 0;
			movement.x += -1 * abilityDirection * abilityModifier;
		}
	}
	
	public override void OnAbilityKey()
	{
		base.OnAbilityKey();
		
		playerInput.isControllable = false;
		
		if(!isSlide)
		{
			isSlide = true;
		}
	}
}
