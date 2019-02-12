using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]

public class RaycastHandler : MonoBehaviour 
{
	public LayerMask collisionMask;
	public int horRaycastCount = 4;
	public int vertRaycastCount = 4;
	
	protected const float collisionOffset = 0.15f;
	
	protected float vectorSpacing0;
	protected float vectorSpacing1;

	protected float horRaycastSpacing;
	protected float vertRaycastSpacing;

	protected BoxCollider2D BoxCollider;
	protected RaycastVectors raycastVectors;
	
	public virtual void Start () 
	{
		BoxCollider = GetComponent<BoxCollider2D>();
	}
	
	public struct RaycastVectors
	{
		public Vector2 topLeft, topRight, bottomLeft, bottomRight;
	}
	
	public void UpdateRaycastVectors()
	{
		//Bounds av boxCollider2D
		Bounds bounds = BoxCollider.bounds;
		bounds.Expand(collisionOffset*-2);
		
		//raycast vector startpoints
		raycastVectors.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastVectors.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastVectors.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		//raycastVectors.topRight = new Vector2 (bounds.max.x, bounds.max.y);

		//mängder raycast i både vert och hor, clampade mellan 2 och maxvärdet (anges i inspector)
		horRaycastCount = Mathf.Clamp(horRaycastCount, 2, int.MaxValue);
		vertRaycastCount = Mathf.Clamp(vertRaycastCount, 2, int.MaxValue);		
		
		vectorSpacing0 = bounds.size.y / (horRaycastCount - 1);
		vectorSpacing1 = bounds.size.x / (vertRaycastCount - 1);		
	}
}
