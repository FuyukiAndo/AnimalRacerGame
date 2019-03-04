using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastMovement : MonoBehaviour 
{
	public float blastSpeed = 3f;
	private int direction;
	private int tempDirection;
	private Vector2 movement;
	
	void Start()
	{
		PlayerAlbatross playerAlbatross;
		
		playerAlbatross = GetComponentInParent<PlayerAlbatross>();
		tempDirection = playerAlbatross.GetDirection();
		direction = tempDirection;
		tempDirection = 0;
		
		
		
		if(playerAlbatross.direction == -1)
		{
			movement.x = -1 * blastSpeed;
		}
		else if(playerAlbatross.direction == 1)
		{
			movement.x = 1 * blastSpeed;
		}
		
	}
	
	
	
	void Update() 
	{

		
		transform.Translate(movement);
	}
}
