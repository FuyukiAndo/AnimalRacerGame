using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (BoxCollider2D))]
public class PlayerMovePenguin : PlayerMoveBase
{

	[SerializeField] private PhysicsMaterial2D slipperyMat, normalMat;
	[SerializeField] private float powerMaxTime, cooldownTime, maxSlideDist;
	private float powerCurrentTime, currentCoolDownTime = 0f;

	protected override void Start ()
	{
		base.Start ();
		powerCurrentTime = powerMaxTime;
	}

	protected override void Update ()
	{
		base.Update ();
		DoActivePower ();
	}

	protected override void OnCollisionEnter2D (Collision2D other)
	{
		base.OnCollisionEnter2D (other);
	}

	protected override void OnCollisionStay2D (Collision2D other)
	{
		base.OnCollisionStay2D (other);
	}

	protected override void OnCollisionExit2D (Collision2D other)
	{
		base.OnCollisionExit2D (other);
	}

	protected override void DoActivePower ()
	{
		if (Input.GetKeyDown (KeyCode.E) && currentCoolDownTime <= 0f)
		{
			RB.sharedMaterial = slipperyMat;
			currentCoolDownTime = cooldownTime;
		}
		if (RB.sharedMaterial == slipperyMat)
		{
			powerCurrentTime -= (1f / powerMaxTime) * Time.deltaTime;
			if (powerCurrentTime <= 0f)
			{
				RB.sharedMaterial = normalMat;
				powerCurrentTime = powerMaxTime;
			}
		}
	}

	protected override void OnDrawGizmos ()
	{
		base.OnDrawGizmos ();
	}

}