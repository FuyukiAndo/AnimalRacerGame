using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNobody : PlayerController 
{
	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();
	}

	public void OnAbilityKey()
	{
		//doesn't do anything.
	}	
}
