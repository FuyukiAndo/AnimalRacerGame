using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastMovement : MonoBehaviour 
{
	public float blastSpeed = 3f;
	private int direction;
	private Vector2 movement;
	
	PlayerAlbatross playerAlbatross;
	
	void Start()
	{
		playerAlbatross = GetComponentInParent<PlayerAlbatross>();
		direction = playerAlbatross.GetDirection();
		
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
