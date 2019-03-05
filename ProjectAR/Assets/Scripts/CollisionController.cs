using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : RaycastController
{
	public static CollisionController collisionController;
	
	public string collisionTag = "oneWay";
	
	public float maxAngle = 80;
	
	public BoxCollisionDirections boxCollisionDirections;
	
	public float SetRayLength = 1.0f;

	public override void Start()
	{
		base.Start();
		collisionController = this;
	}
	
	public struct BoxCollisionDirections
	{
		public bool up, down, left, right, horizontal, climbing, descendAngle;
		public float ascendAngle, angleOld;
		
		public void resetDirections()
		{
			up = false;
			down = false;
			left = false;
			right = false;
			horizontal = false;
			climbing = false;

			descendAngle = false;
			
			ascendAngle = 0;
			angleOld = ascendAngle;
		}
	}	
	
	/*public void Move(Vector2 movement)
	{
		UpdateRaycastDirections();
		boxCollisionDirections.resetDirections();
		
		if(movement.y < 0)
		{
			DescendSlope(ref movement);
		}
		
		if(movement.x != 0 || movement.y != 0)
		{
			checkCollision(ref movement);
		}

		transform.Translate(movement);
	}*/
	
	public void checkCollision(ref Vector2 movement)
	{
		float directionX = Mathf.Sign(movement.x);
		float rayLengthX = Mathf.Abs(movement.x) + collisionOffset*SetRayLength;
		
		for(int i = 0; i < horizontalRaycastAmount; i++)
		{
			Vector2 rayVectorX;
			
			if (directionX == -1)
			{
				rayVectorX = raycastDirection.bottomLeft;
			}
			else
			{	
				rayVectorX = raycastDirection.bottomRight;
			}

			rayVectorX += Vector2.up * (vectorSpacing0 * i);
			
			RaycastHit2D hitX = Physics2D.Raycast(rayVectorX, Vector2.right * directionX, rayLengthX, collisionMask);
			Debug.DrawRay(rayVectorX, Vector2.right * directionX * rayLengthX, Color.red);
			
			if (hitX)
			{		
				float angle = Vector2.Angle(hitX.normal, Vector2.up);
				
				if(i == 0 && angle <= maxAngle)
				{
					float distanceToSlope = 0;
					
					if (angle != boxCollisionDirections.angleOld)
					{
						distanceToSlope = hitX.distance-collisionOffset;
						movement.x -= distanceToSlope * directionX;
					}
					ClimbSlope(ref movement, angle);	
					movement.x += distanceToSlope * directionX;
				}
			
				if(!boxCollisionDirections.climbing || angle > maxAngle)
				{			
					if ((directionX == 1 || directionX == -1) && hitX.collider.tag == collisionTag)
					{
						boxCollisionDirections.horizontal = false;
						continue;
					}
				
					movement.x = (hitX.distance - collisionOffset) * directionX;
					rayLengthX = hitX.distance;	

					if(boxCollisionDirections.climbing && movement.y > 0)
					{
						movement.y = Mathf.Tan(boxCollisionDirections.ascendAngle * Mathf.Deg2Rad) * Mathf.Abs(movement.x);
					}
				}
			}
		}	

		float directionY = Mathf.Sign(movement.y);
		float rayLengthY = Mathf.Abs(movement.y) + collisionOffset*SetRayLength;
		
		for(int i = 0; i < verticalRaycastAmount; i++)
		{
			Vector2 rayVectorY;
			
			if (directionY == -1)
			{
				rayVectorY = raycastDirection.bottomLeft;
			}
			else
			{	
				rayVectorY = raycastDirection.topLeft;
			}
			
			rayVectorY += Vector2.right * (vectorSpacing1 * i + movement.x);
			
			RaycastHit2D hitY = Physics2D.Raycast(rayVectorY, Vector2.up * directionY, rayLengthY, collisionMask);
			Debug.DrawRay(rayVectorY, Vector2.up * directionY * rayLengthY, Color.red);
			
			if (hitY)
			{	
				/*if((directionY == 1 && (hitY.collider.tag == collisionTag)))
                {
					boxCollisionDirections.down = false;
                    continue;
                }*/
				
				if(boxCollisionDirections.climbing)
				{
					movement.x = movement.y / Mathf.Tan(boxCollisionDirections.ascendAngle * Mathf.Deg2Rad) * Mathf.Sign(movement.x);
					rayLengthY = Mathf.Abs(movement.x) + collisionOffset;
					Vector2 raycastStart;
					
					if(directionX == -1)
					{
						raycastStart = raycastDirection.bottomLeft + Vector2.up * movement.y;
					}
					else
					{
						raycastStart = raycastDirection.bottomRight + Vector2.up * movement.y;
					}
					
					RaycastHit2D newHitY = Physics2D.Raycast(raycastStart,Vector2.right * directionX, rayLengthY, collisionMask);
					
					if(newHitY)
					{
						float angle = Vector2.Angle(newHitY.normal, Vector2.up);
						if(angle != boxCollisionDirections.ascendAngle)
						{
							movement.x = (newHitY.distance - collisionOffset) * directionX;
							boxCollisionDirections.ascendAngle = angle;
						}
					}
				}
				
				if (directionY == -1)
				{
					boxCollisionDirections.down = true;
				}
				
				if (directionY == 1)
				{
					boxCollisionDirections.up = true;
				}
						
				movement.y = (hitY.distance - collisionOffset) * directionY;
				rayLengthY = hitY.distance;
			}
		}
	}	
	
	
	public void ClimbSlope(ref Vector2 movement, float angle)
	{
		float moveDistance = Mathf.Abs(movement.x);
		float climbMovementY = Mathf.Sin(angle * Mathf.Deg2Rad) * moveDistance;
		
		if(movement.y <= climbMovementY)
		{
			movement.y = climbMovementY;
			movement.x = Mathf.Cos(angle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(movement.x);
			boxCollisionDirections.down = true;
			boxCollisionDirections.climbing = true;
			boxCollisionDirections.ascendAngle = angle;
		}
	}
	
	public void DescendSlope(ref Vector2 movement)
	{
		float directionX = Mathf.Sign(movement.x);
		Vector2 rayStart;
		
		if (directionX == -1)
		{
			rayStart = raycastDirection.bottomRight;
		}
		else
		{
			rayStart = raycastDirection.bottomLeft;
		}
		
		RaycastHit2D hit = Physics2D.Raycast(rayStart, -Vector2.up, Mathf.Infinity, collisionMask);
		
		if(hit)
		{
			float angle = Vector2.Angle(hit.normal, Vector2.up);
			
			if(angle != 0 && angle <=maxAngle)
			{
				if(Mathf.Sign(hit.normal.x) == directionX)
				{
					if(hit.distance - collisionOffset <= Mathf.Tan(angle * Mathf.Deg2Rad) * Mathf.Abs(movement.x))
					{
						float moveDistance = Mathf.Abs(movement.x);
						float descendMovementY = Mathf.Sin(angle * Mathf.Deg2Rad) * moveDistance;
						movement.x = Mathf.Cos(angle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(movement.x);
						movement.y -= descendMovementY;
						
						boxCollisionDirections.ascendAngle = angle;
						boxCollisionDirections.descendAngle = true;
						boxCollisionDirections.down = true;
						
					}
				}
			}
		}
	}
}
