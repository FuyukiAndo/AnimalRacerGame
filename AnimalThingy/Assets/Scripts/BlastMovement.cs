using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastMovement : MonoBehaviour 
{
	private float blastSpeed = 20f;
	private int blastDirection;
	private int tempDirection;
	private Vector2 movement;
	
	void Start()
	{
		PlayerAlbatross playerAlbatross;
		playerAlbatross = GetComponentInParent<PlayerAlbatross>();
		
		if(transform.parent !=null)
		{
			tempDirection = playerAlbatross.GetDirection();
		}
		
		blastDirection = tempDirection;
		transform.parent = null;
		//blastDirection = tempDirection;		
	}

	void Update() 
	{
		//Debug.Log(blastDirection);
		
		if(blastDirection == -1)
		{
			movement.x = -1 * blastSpeed;			
		}
		else if(blastDirection == 1)
		{
				movement.x = 1 * blastSpeed;		
		}
		
		transform.Translate(movement * Time.deltaTime);
	}
}
