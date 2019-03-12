using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{
	GravityController gravityController;
	CollisionController collisionController;
	Vector2 movement;
	
	void Start () 
	{
		transform.parent = null;	
		gravityController = GetComponent<GravityController>();
		collisionController = GetComponent<CollisionController>();
	}

	void Update()
	{	
		gravityController.UpdateGravity();
		
		movement.y += gravityController.gravity * Time.deltaTime;
		
		MoveObject(movement * Time.deltaTime);
	}
	
	public void MoveObject(Vector2 movement)
	{
		collisionController.UpdateRaycastDirections();
		collisionController.boxCollisionDirections.resetDirections();

		if(movement.x != 0 || movement.y != 0)
		{
			collisionController.checkCollision(ref movement);
		}

		transform.Translate(movement);
	}
	
}
