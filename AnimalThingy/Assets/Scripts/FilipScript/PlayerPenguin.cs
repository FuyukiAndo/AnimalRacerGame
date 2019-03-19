using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPenguin : PlayerController
{
	public GameObject projectileObject;	

	private bool spawnProjectile = false;

	private float InstantiateProjectileTimer;
	private float savedInstatiateTimer;
	
	public override void Start() 
	{
		base.Start();
		InstantiateProjectileTimer = movementSpeed/96f;
		savedInstatiateTimer = InstantiateProjectileTimer;
	}

	public override void OnAbilityKey()
	{
		base.OnAbilityKey();
		
		playerInput.isControllable = false;
		playerInput.changeAngle = false;
		
		if(!isPassiveAbility)
		{
			spawnProjectile = true;
		}
	}
	
	private void PlayerPassiveAbility()
	{
		if(spawnProjectile)
		{
			InstantiateProjectileTimer -= Time.deltaTime;
			
			if(InstantiateProjectileTimer <= 0)
			{
				Instantiate(projectileObject, new Vector2(transform.position.x,transform.position.y+2f), new Quaternion(0, 0, 0, 0), gameObject.transform);		
				InstantiateProjectileTimer = savedInstatiateTimer;
			}			
		}
	}

	private void PlayerActiveAbility()
	{
		if(!isPassiveAbility && passiveAbility)
		{
			maxUseCounter--;
			
			if(maxUseCounter < 0)
			{
				isPassiveAbility = true;
				passiveAbility = false;
				maxUseCounter = savedMaxUseCounter;
			}
		}
		
		if(!passiveAbility && isPassiveAbility)
		{
			movement.x += -1 * abilityDirection * abilityModifier;
		}
		
		if(!passiveAbility && !isPassiveAbility)
		{
			playerInput.isControllable = true;
			playerInput.changeAngle = true;		
			spawnProjectile = false;	
		}		
	}	

	public override void Update() 
	{
		base.Update();
		
		PlayerPassiveAbility();
		PlayerActiveAbility();
	}	
}
