using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : RaycastHandler
{	
	public static PlayerCollisionController controller;
	
	public string collisionTag = "oneWay";
	
	public Collision BoxCollision;
	
	public float SetRayLength = 1.0f;

	public override void Start()
	{
		base.Start();
	}
	
	public struct Collision
	{
		public bool above, below, left, right, horizontal;	
		
		public void resetValues()
		{
			above = false;
			below = false;
			left = false;
			right = false;
			horizontal = false;
		}
	}	
	
	public void Move(Vector2 movement)
	{
		UpdateRaycastVectors();
		BoxCollision.resetValues();
		
		if(movement.x != 0 || movement.y != 0)
		{
			checkCollision(ref movement);
		}

		transform.Translate(movement);
	}
	
	void checkCollision(ref Vector2 movement)
	{
		float directionX = Mathf.Sign(movement.x);
		float rayLengthX = Mathf.Abs(movement.x) + collisionOffset*SetRayLength;
		
		for(int i = 0; i < horRaycastCount; i++)
		{
			Vector2 rayVectorX;
			
			if (directionX == -1)
			{
				rayVectorX = raycastVectors.bottomLeft;
			}
			else
			{	
				rayVectorX = raycastVectors.bottomRight;
			}

			rayVectorX += Vector2.up * (vectorSpacing0 * i);
			
			RaycastHit2D hitX = Physics2D.Raycast(rayVectorX, Vector2.right * directionX, rayLengthX, collisionMask);
			Debug.DrawRay(rayVectorX, Vector2.right * directionX * rayLengthX, Color.red);
			
			if (hitX)
			{				
				if ((directionX == 1 || directionX == -1) && hitX.collider.tag == collisionTag)
				{
					BoxCollision.horizontal = false;
					continue;
				}
				
				movement.x = (hitX.distance - collisionOffset) * directionX;
				rayLengthX = hitX.distance;
			}
		}	

		float directionY = Mathf.Sign(movement.y);
		float rayLengthY = Mathf.Abs(movement.y) + collisionOffset*SetRayLength;
		
		for(int i = 0; i < vertRaycastCount; i++)
		{
			Vector2 rayVectorY;
			
			if (directionY == -1)
			{
				rayVectorY = raycastVectors.bottomLeft;
			}
			else
			{	
				rayVectorY = raycastVectors.topLeft;
			}
			
			rayVectorY += Vector2.right * (vectorSpacing1 * i + movement.x);
			
			RaycastHit2D hitY = Physics2D.Raycast(rayVectorY, Vector2.up * directionY, rayLengthY, collisionMask);
			Debug.DrawRay(rayVectorY, Vector2.up * directionY * rayLengthY, Color.red);
			
			if (hitY)
			{	
				if((directionY == 1 && (hitY.collider.tag == collisionTag)))
                {
					BoxCollision.below = false;
                    continue;
                }
				
				if (directionY == -1)
				{
					BoxCollision.below = true;
				}
				
				if (directionY == 1)
				{
					BoxCollision.above = true;
				}
						
				movement.y = (hitY.distance - collisionOffset) * directionY;
				rayLengthY = hitY.distance;
			}
		}
	}	
}
