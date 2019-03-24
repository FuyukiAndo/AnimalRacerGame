using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingProjectile: ProjectileGravityController 
{
	private int projectileDirection;
	private float projectileArch;
	private float projectileSpeed;
	private Collider2D[] collision;	

	public override void Start()
	{
		base.Start();
		
		PlayerMonkey playerMonkey;
		playerMonkey = GetComponentInParent<PlayerMonkey>();
		projectileDirection = -playerMonkey.abilityDirection;
		projectileSpeed = playerMonkey.abilityModifier;
		projectileArch = playerMonkey.abilityModifier/2f;
		
		transform.parent = null;
		
		movement.y += 1 * projectileArch;
	}
	
	public override void Update()
	{
		base.Update();
		
		collision = Physics2D.OverlapCircleAll(transform.position, 0.5f);

			for(int i = 0; i < collision.Length; i++)
			{	
				if(collision[i].gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
				{
					if(collision[i].gameObject.tag == "Projectile")
					{
						Destroy(collision[i].gameObject);
						Destroy(this.gameObject);
					}
				}	
			}

		movement.x = -1 * -projectileDirection * projectileSpeed;
	}
}