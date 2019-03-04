using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlbatross : PlayerController 
{
	public GameObject prefab;
	[Header("Flying Settings")]
	public float flyTimer = 3;
	public int maxFlyCount = 3;
	
	private bool isFlying = false;
	private int mMaxFlyCount;
	private float mFlyTimer;
	private float countdownMod = 0.1f;

	PlayerInput playerInput;
	
	
	public override void Start()
	{
		base.Start();
		playerStates = PlayerStates.playerIdle;
		
		mMaxFlyCount = maxFlyCount;
		mFlyTimer = flyTimer;
		playerInput = GetComponent<PlayerInput>();
		abilityMeter = 1.0f;
	}
	
	public override void Update()
	{
		base.Update();		
		
		if(isFlying)
		{
			flyTimer = flyTimer-countdownMod;
			
			if(flyTimer < 0)
			{
				flyTimer = mFlyTimer;
				maxFlyCount--;
				isFlying = false;
			}
		}
		
		if(collisionController.boxCollisionDirections.down)
		{
			maxFlyCount = mMaxFlyCount;
			playerInput.groundedMovement = true;
		}
		else
		{
			playerInput.groundedMovement = false;	
		}
		
		if(maxFlyCount == 0)
		{	
			maxFlyCount = 0;
	
			if(collisionController.boxCollisionDirections.down)
			{
				maxFlyCount = mMaxFlyCount;
			}
		}
	}
	
	public void OnFlyKeyDown()
	{	
		if(maxFlyCount != 0)
		{
			if (flyTimer == mFlyTimer)
			{
				isFlying = true;
				movement.y = maxVelocity;
			}
		}
	}
	
	public void OnFlyKeyUp()
	{
		if(movement.y > minVelocity)
		{
			movement.y = minVelocity;
		}
	}
	
	public void OnAbilityKey()
	{
		 Instantiate(prefab, transform.position, new Quaternion(0, 0, 0, 0), gameObject.transform);
		//Instantiate(prefab,transform.position, Quaternion.identity);
		//prefab.transform.SetParent(transform.parent, true);
		//prefab.transform.parent = gameObject.transform;
	}
	
	
	public int GetDirection()
	{
		return direction;
	}
	
	/*void windDiraction()
    {
        intervalTime += Time.deltaTime;
        if(intervalTime > currentChangeInterval)
        {
            pushStrenght = -pushStrenght;
            intervalTime = 0;
            currentChangeInterval = Random.Range(minWindChangeInterval, maxWindChangeInterval);
        }
        foreach (Rigidbody2D rb2d in playerRigidbody2D)
        {
            rb2d.velocity = new Vector2(pushStrenght, rb2d.velocity.y);
        }
    }*/
}
