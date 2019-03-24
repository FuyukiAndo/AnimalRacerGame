using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticProjectile : ProjectileGravityController 
{
	public override void Start()
	{
		base.Start();
		transform.parent = null;
	}
	
	public override void Update()
	{
		base.Update();
	}
}
