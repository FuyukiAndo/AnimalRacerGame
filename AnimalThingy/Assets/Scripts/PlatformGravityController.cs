using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGravityController : MonoBehaviour 
{
	[Header("Platform Gravity Settings")]
	[Tooltip("Max jump height value between 0.1f and x")]
	public float maxVelocity = 8.0f;

	[Tooltip("Min jump height value between 0.1f and 2.0f")]
	[Range(0.1f,2.0f)]public float fallDelay = 0.4f;
	
	[HideInInspector] public float gravity;
	
	void OnValidate()
	{
		if (maxVelocity < 0)
		{
			maxVelocity = 0.1f;
		}
	}
	
	public void UpdateGravity()
	{
		gravity = -(2*maxVelocity)/Mathf.Pow(fallDelay, 2);
		//maxVelocity = Mathf.Abs(gravity) * jumpAndFallDelay;
		//minVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);
	}
}
