using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPenguin : PlayerController
{
	public GameObject projectileObject;	

	public float slideTimer = 2f;
	[HideInInspector] public float savedSlideTimer;

	private bool spawnProjectile = false;

	private float InstantiateProjectileTimer;
	private float savedInstatiateTimer;
	
	public override void Start() 
	{
		base.Start();
		InstantiateProjectileTimer = movementSpeed/96f;
		savedInstatiateTimer = InstantiateProjectileTimer;
		savedSlideTimer = slideTimer;
	}

	public override  void OpponentProjectileCollision()
	{
		base.OpponentProjectileCollision();
	}

	public override void OnAbilityKey()
	{
		base.OnAbilityKey();

		if(slideTimer == savedSlideTimer && abilityMeter == 1f)
		{
			playerInput.isControllable = false;
			playerInput.changeAngle = false;
			abilityMeter = 0;
			
			if(!isPassiveAbility)
			{
				spawnProjectile = true;
			}
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
		if(passiveAbility)
		{
			if(slideTimer > 0)
			{
				slideTimer -= Time.deltaTime;
				movement.x += -1 * abilityDirection * abilityModifier;
			}
			
			if(slideTimer < 0)
			{
				slideTimer = savedSlideTimer;
				isPassiveAbility = true;
				passiveAbility = false;
				playerInput.isControllable = true;
				playerInput.changeAngle = true;
				spawnProjectile = false;	
			}
		}		
		
		if(abilityMeter == 1f)
		{
			slideTimer = savedSlideTimer;
		}	
	}	

	public override void Update() 
	{
		base.Update();
		
		PlayerPassiveAbility();
		PlayerActiveAbility();
	}	
}
