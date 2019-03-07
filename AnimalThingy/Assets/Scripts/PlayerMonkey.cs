using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonkey : PlayerController 
{
	public override void Start()
	{
		base.Start();
	}
	
	public override void Update()
	{
		if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right)) 
		{
			print("this works");
		}
	}
	
	public void OnAbilityKey()
	{
		if(abilityMeter==100)
		{
			abilityMeter = 0;
		}
	}
	
}
