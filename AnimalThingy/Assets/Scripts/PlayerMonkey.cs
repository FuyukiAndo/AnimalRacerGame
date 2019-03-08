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
		base.Update();

		bool wallSliding = false;

		if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right)
		&& !collisionController.boxCollisionDirections.down && movement.y < 0)
		{
			wallSliding = true;

			if(movement.y < -wallSlideSpeedMax)
			{
				Debug.Log("wallslide");
				movement.y = -wallSlideSpeedMax;
			}
		}
		
		/*if((collisionController.boxCollisionDirections.left || collisionController.boxCollisionDirections.right)) 
		{
			//print("The wall has been collided with");
		}	*/	
	}
	
	public void OnAbilityKey()
	{
		if(abilityMeter==100)
		{
			abilityMeter = 0;
		}
	}
	
}
