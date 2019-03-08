using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour 
{
	[Header("Jump and Gravity Settings")]
	[Tooltip("Max jump height value between 0.1f and x")]
	public float maxJumpHeight = 8.0f;

	[Tooltip("Min jump height value between 0.1f and x")]
	public float minJumpHeight = 1f;

	[Tooltip("Min jump height value between 0.1f and x")]
	[Range(0.1f,2.0f)]public float jumpAndFallDelay = 0.4f;
	
	[HideInInspector] public float gravity;
	[HideInInspector] public float maxVelocity;
	[HideInInspector] public float minVelocity;
	
	public void UpdateGravity()
	{
		gravity = -(2*maxJumpHeight)/Mathf.Pow(jumpAndFallDelay, 2);
		maxVelocity = Mathf.Abs(gravity) * jumpAndFallDelay;
		minVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);
	}
	
	void OnValidate()
	{
		if(minJumpHeight < 0.1f)
		{
			minJumpHeight = 0.1f;
		}

		if(maxJumpHeight < 0.1f)
		{
			maxJumpHeight = 0.1f;
		}
		
		if(maxJumpHeight < minJumpHeight)
		{
			maxJumpHeight = minJumpHeight + 0.1f;
		}
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(gameObject.transform.position.x-4, gameObject.transform.position.y + maxJumpHeight, 0),
						new Vector3(gameObject.transform.position.x+4,gameObject.transform.position.y  + maxJumpHeight, 0));

		Gizmos.color = Color.green;
		Gizmos.DrawLine(new Vector3(gameObject.transform.position.x-4, gameObject.transform.position.y + minJumpHeight, 0),
						new Vector3(gameObject.transform.position.x+4,gameObject.transform.position.y  + minJumpHeight, 0));

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0),
						new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + maxJumpHeight, 0));
	}

}
