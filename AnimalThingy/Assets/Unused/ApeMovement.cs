using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class ApeMovement : MonoBehaviour 
{	
	[SerializeField] private float movementSpeed = 3.0f;
	[SerializeField] private float accelerationSpeed = 1.5f;
	private float horizontalInput;
	private float verticalInput;
	
	private int left = 1;
	private int right = -1;
	private int up = 1;
	private int down = -1;
	
	Vector3 movement;
	
	void playerInput()
	{
		horizontalInput = Input.GetAxisRaw("Horizontal");
		verticalInput = Input.GetAxisRaw("Vertical");	
	}
	
	void move(float dirX, float dirY, float dirZ)
	{
		transform.Translate(new Vector3(dirX * movementSpeed * Time.deltaTime, dirY * movementSpeed * Time.deltaTime, dirZ * movementSpeed * Time.deltaTime));
	}
	
	void playerMovement()
	{
		float hor = 0;
		float vert = 0;	
		float zaxis = 0;
		
		if(horizontalInput == left)
		{
			hor = left;
		}
		
		if(horizontalInput == right)
		{
			hor = right;		
		}
		
		if(verticalInput == up)
		{
			vert = up;
		}
		
		if(verticalInput == down)
		{
			vert = down;	
		}
		
		move(hor, vert, zaxis);
	}

	void Start() 
	{
		
	}

	void Update() 
	{

	}
	
	void FixedUpdate()
	{
		playerMovement();
				playerInput();
	}
	
}
