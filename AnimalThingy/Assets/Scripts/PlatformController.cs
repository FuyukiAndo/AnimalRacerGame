using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController 
{
	[HideInInspector] public CollisionController collisionController;
	[HideInInspector] public MovingPlatform movingPlatform;
	public LayerMask entityMask;
	public Vector2 moveEntity;
	Isflak isflak;
	
	public float setRayLength = 1.0f;
	
	public override void Start()
	{
		base.Start();
		movingPlatform = GetComponent<MovingPlatform>();
		collisionController = GetComponent<CollisionController>();
		
	}
	
	void Update()
	{
		UpdateRaycastDirections();
		
		if(movingPlatform)
		{
			moveEntity = movingPlatform.movement;
		}

		Vector2 movement = moveEntity * Time.deltaTime;
		MoveObject(movement);

	}
	
	public void MoveObject(Vector2 movement)
	{
		collisionController.UpdateRaycastDirections();
		collisionController.boxCollisionDirections.resetDirections();
		
		/*if(movement.y < 0)
		{
			collisionController.DescendSlope(ref movement);
		}*/

		//Only checks for collision if moving in any direction
		if(movement.x != 0 || movement.y != 0)
		{
			collisionController.checkCollision(ref movement);
		}
		
		//move players
		MoveEntities(movement);
		
		//moves platform
		transform.Translate(movement);
	}
	
	void MoveEntities(Vector2 movement)
	{
		//stores entities that moved during this frame
		HashSet<Transform> movedEntities = new HashSet<Transform>();
		
		float directionX = Mathf.Sign(movement.x);
		float directionY = Mathf.Sign(movement.y);


		if (movement.y != 0)
		{
			float rayLength = Mathf.Abs(movement.y) + collisionOffset*setRayLength;
		
			for(int i = 0; i < verticalRaycastAmount; i++)
			{
				Vector2 rayVector;
			
				if (directionY == -1)
				{
					rayVector = raycastDirection.bottomLeft;
				}
				else
				{	
					rayVector = raycastDirection.topLeft;
				}
			
				rayVector += Vector2.right * (vectorSpacing1 * i);
		
				RaycastHit2D hitY = Physics2D.Raycast(rayVector, Vector2.up * directionY, rayLength, entityMask);
				Debug.DrawRay(rayVector, Vector2.up * directionY * rayLength, Color.blue);
				
				if (hitY)
				{	
					if(!movedEntities.Contains(hitY.transform))
					{
						//add moved entities to hashset movedEntities.
						movedEntities.Add(hitY.transform);
						
						float moveX;
						
						if(directionY == 1)
						{
							moveX = movement.x;
						}
						else
						{
							moveX = 0;
						}
					
						float moveY = movement.y - (hitY.distance - collisionOffset*setRayLength) * directionY;
						hitY.transform.Translate(new Vector2(moveX, moveY));
					}
				}
			}
		}
			
		if(movement.x !=0)
		{
			float rayLengthX = Mathf.Abs(movement.x) + collisionOffset*setRayLength;

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

				RaycastHit2D hitX = Physics2D.Raycast(rayVectorX, Vector2.right * directionX, rayLengthX, entityMask);
				Debug.DrawRay(rayVectorX, Vector2.right * directionX * rayLengthX, Color.blue);

				if (hitX)
				{	
					if(!movedEntities.Contains(hitX.transform))
					{
						//add moved entities to hashset movedEntities.
						movedEntities.Add(hitX.transform);
						float moveX = movement.x - (hitX.distance - collisionOffset*setRayLength) * directionX;			
						float moveY = 0;
						
						hitX.transform.Translate(new Vector2(moveX, moveY));
					}
				}
			}
		}
		
		//karaktärers blinkande animation med intervall vart tionde sekund (Ska även köras när andra animationer körs)
		
		//horizontal and downward
		if(directionY == -1 || movement.y == 0 && movement.x !=0)
		{
			float rayLength = collisionOffset*2*setRayLength;
		
			for(int i = 0; i < verticalRaycastAmount; i++)
			{
				Vector2 rayVector = raycastDirection.topLeft + Vector2.right * (vectorSpacing1 * i);
		
				RaycastHit2D hitY = Physics2D.Raycast(rayVector, Vector2.up, rayLength, entityMask);
				
				if (hitY)
				{	
					if(!movedEntities.Contains(hitY.transform))
					{
						//add moved entities to hashset movedEntities.
						movedEntities.Add(hitY.transform);
						float moveX = movement.x;
						float moveY = movement.y;
						
						hitY.transform.Translate(new Vector2(moveX, moveY));
					}
				}
			}
		}
	}
}
