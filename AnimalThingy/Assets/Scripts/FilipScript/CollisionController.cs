using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : RaycastController
{
	[Header("Collision Mask")]
	[Tooltip("The layer mask which to check for collision")]
	public LayerMask collisionMask;
	
	[Header("Amount of raycasts")]
	//[Tooltip("The amount of raycasts vectors for horizontal collision")]	
	public int horizontalRaycastsAmount = 4;
	//[Tooltip("The amount of raycasts vectors for horizontal collision")]	
	public int verticalRaycastsAmount = 4;
	
	public float SetRayLength = 1.0f;
	//public static CollisionController collisionController;
	
	//private string collisionTag = "OneWay";
	private string oneWayCollision = "OneWay";
	
	//Max angle player can move on, (don't touch this) - 80
	[HideInInspector]public float maxAngle = 80;
	
	[HideInInspector]public BoxCollisionDirections boxCollisionDirections;
	
//	public float directionY;// = Mathf.Sign(movement.y);
	//public Vector2 rayVectorY;
	
	public override void Start()
	{
		base.Start();
		boxCollisionDirections.direction = 1;
	}
	
	void Update()
	{
		horizontalRaycastAmount = horizontalRaycastsAmount;
		verticalRaycastAmount = verticalRaycastsAmount;
	}
	/*string[] stringtags = new string[]{"player", "enemy"};
	public enum tagNumbers{player,enemy};
	public tagNumbers tagNum;*/
	
	/*string[] collisionTags = new string[]{"oneWay", "twoWay"};
	
	public enum TagElement
	{
		oneWay, 
		twoWay
	};
	public TagElement tagElement;*/
	
	public struct BoxCollisionDirections
	{
		public bool up, down, left, right, horizontal, climbing, descendAngle, moving;
		public float ascendAngle, angleOld;
		public int direction;
		
		public void resetDirections()
		{
			up = false;
			down = false;
			left = false;
			right = false;
			horizontal = false;
			climbing = false;
			moving = false;

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
		float directionX = boxCollisionDirections.direction;//Mathf.Sign(movement.x);
		float rayLengthX = Mathf.Abs(movement.x) + collisionOffset*SetRayLength;
		
		if(Mathf.Abs(movement.x) < collisionOffset)
		{
			rayLengthX = 2*collisionOffset;
		}
		
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
				if(hitX.distance == 0)
				{
					continue;
				}
		
				float angle = Vector2.Angle(hitX.normal, Vector2.up);

				if (directionX == -1)
				{
					boxCollisionDirections.left = true;
				}
				
				if (directionX == 1)
				{
					boxCollisionDirections.right = true;
				}

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
					if ((directionX == 1 || directionX == -1) && hitX.collider.tag == oneWayCollision)
					{
						boxCollisionDirections.horizontal = false;
						continue;
					}

					if(boxCollisionDirections.climbing && movement.y > 0)
					{
						movement.y = Mathf.Tan(boxCollisionDirections.ascendAngle * Mathf.Deg2Rad) * Mathf.Abs(movement.x);
					}
					
					movement.x = (hitX.distance - collisionOffset) * directionX;
					rayLengthX = hitX.distance;	
				}
			}
		}	

		//Physics2D.SyncTransforms(); 
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
				if(hitY.distance == 0)
				{
					continue;
				}
		
				// Collision check for one way-platform
				if((directionY == 1 && (hitY.collider.tag == oneWayCollision)))
                {
					boxCollisionDirections.down = false;
                    continue;
                }			
			
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
			movement.x = Mathf.Cos(angle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(movement.x); //Mathf.Cos(angle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(movement.x);
			
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
			
			if(angle != 0 && angle <= maxAngle)
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
