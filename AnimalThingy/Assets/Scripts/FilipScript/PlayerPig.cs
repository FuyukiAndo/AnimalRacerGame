using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPig : PlayerController 
{
	public int maxJumpCount = 7;
	public float heightModifier = 2.0f;
	public float inbetweenJumpCounter = 0.25f;
	private float tempCounter;
	public bool isJump = false;
	private bool newJump = false;
	public bool blowUp = false;
	private bool blowUp2 = false;
	private int i = 0;
	BoxCollider2D boxCollider2D;
    float scaleX, scaleY;
	float tempX, tempY;
	float newX, newY;
	float oldOffsetY, newOffsetY;
	public float timer;
	
	public bool GetSignal()
	{
		return newJump;
	}
	
	public override void Start() 
	{
		base.Start();
		tempCounter = inbetweenJumpCounter;	
		boxCollider2D = GetComponent<BoxCollider2D>();
		
		tempX = boxCollider2D.size.x;
		tempY = boxCollider2D.size.y;
		newX = 5.75f;
		newY = 3.75f;
		oldOffsetY = 1.36f;
		timer = 0.71f;
		
		Debug.Log(tempX);
		Debug.Log(tempY);
		
		boxCollider2D.size = new Vector2(tempX, tempY);
	}

	public override void Update() 
	{		
		base.Update();

		if(blowUp)
		{
			boxCollider2D.size = new Vector2(newX,tempY+.28f);
			timer-=Time.deltaTime;
			
			if(timer < 0)
			{
				blowUp = false;
			}
		}
		else
		{
			timer = 0.71f;
			boxCollider2D.size = new Vector2(tempX, tempY);
		}

		if(collisionController.boxCollisionDirections.down)
		{	
			if(inbetweenJumpCounter < 0)
			{
				inbetweenJumpCounter = tempCounter;
			}
	
			if(isJump)
			{
				inbetweenJumpCounter -= Time.deltaTime;
				newJump = true;
			}
		}
		
		if(inbetweenJumpCounter < 0)
		{
			isJump = false;
			newJump = false;
			inbetweenJumpCounter = tempCounter;
			i = 0;
		}
		else
		{
			
		}
	}
	
	public override void OnJumpKeyDown()
	{
		base.OnJumpKeyDown();
		isJump = true;
		i++;
		
		movement.y += heightModifier * i*2;
	}
	
	public override void OnAbilityKey()
	{
		base.OnAbilityKey();	
	}
	
}
