using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingProjectileController : ProjectileGravityController 
{	
	//public float throwArchModifier = 35f;
	private int projectileDirection;
	private float projectileSpeed;

	public override void Start()
	{
		base.Start();
		
		PlayerMonkey playerMonkey;
		playerMonkey = GetComponentInParent<PlayerMonkey>();
		projectileDirection = -playerMonkey.abilityDirection;
		projectileSpeed = playerMonkey.abilityModifier;
		transform.parent = null;
		
		//movement.y = velocity;
	}
	
	public override void Update()
	{
		base.Update();
		movement.x = -1 * -projectileDirection * projectileSpeed;
	}
}