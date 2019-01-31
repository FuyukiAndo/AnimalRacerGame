using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (BoxCollider2D))]
public class PlayerPenguin : PlayerMoveBase
{

	protected override void Start ()
	{
		base.Start ();
	}

	protected override void Update ()
	{
		base.Update ();
	}

	protected override void OnCollisionEnter2D (Collision2D other)
	{
		base.OnCollisionEnter2D (other);
	}

	protected override void OnCollisionExit2D(Collision2D other)
	{
		base.OnCollisionExit2D(other);
	}

}