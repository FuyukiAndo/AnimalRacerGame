using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class RaycastController : MonoBehaviour 
{
	//[HideInSubClass]
	protected int horizontalRaycastAmount = 4;
	
	//[HideInSubClass]
	protected int verticalRaycastAmount = 4;
	
	protected const float collisionOffset = 0.15f;
	
	protected float vectorSpacing0;
	protected float vectorSpacing1;

	protected float horRaycastSpacing;
	protected float vertRaycastSpacing;

	protected BoxCollider2D boxCollider;
	protected RaycastDirections raycastDirection;
	
	public virtual void Start() 
	{
		boxCollider = GetComponent<BoxCollider2D>();
	}
	
	public struct RaycastDirections
	{
		public Vector2 topLeft, topRight, bottomLeft, bottomRight;
	}
	
	public void UpdateRaycastDirections()
	{
		//Bounds av boxCollider2D
		Bounds boxColliderBounds = boxCollider.bounds;
		boxColliderBounds.Expand(collisionOffset*-2);
		
		//raycast vector startpoints
		raycastDirection.bottomLeft = new Vector2 (boxColliderBounds.min.x,boxColliderBounds.min.y);
		raycastDirection.bottomRight = new Vector2 (boxColliderBounds.max.x,boxColliderBounds.min.y);
		raycastDirection.topLeft = new Vector2 (boxColliderBounds.min.x,boxColliderBounds.max.y);
		//raycastVector.topRight = new Vector2 (boxColliderBounds.max.x, boxColliderBounds.max.y);

		//mängder raycast i både vert och hor, clampade mellan 2 och maxvärdet (anges i inspector)
		horizontalRaycastAmount = Mathf.Clamp(horizontalRaycastAmount, 2, int.MaxValue);
		verticalRaycastAmount = Mathf.Clamp(verticalRaycastAmount, 2, int.MaxValue);		
		
		vectorSpacing0 = boxColliderBounds.size.y / (horizontalRaycastAmount - 1);
		vectorSpacing1 = boxColliderBounds.size.x / (verticalRaycastAmount - 1);
	}
}
