using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlbatross : PlayerController 
{
	GameObject windBlastObject;//prefab;
	[Header("Flying Settings")]
	public float flyTimer = 3;
	public int maxFlyCount = 3;
	
	private bool isFlying = false;
	private int mMaxFlyCount;
	private float mFlyTimer;
	private float countdownMod = 0.1f;
	private float tempDelay;
	public bool isGliding = false;
	//public float tt = 0;

	//private PlayerInput playerInput;
	
	public override void Start()
	{
		base.Start();
		//playerStates = PlayerStates.playerIdle;
		windBlastObject = Resources.Load<GameObject>("Prefabs/SpeedUpBlast");
		
		
		mMaxFlyCount = maxFlyCount;
		mFlyTimer = flyTimer;
		//playerInput = GetComponent<PlayerInput>();
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
			//playerInput.groundedMovement = true;
		}
		else
		{
			//playerInput.groundedMovement = false;	
		}
		
		if(maxFlyCount == 0)
		{	
			maxFlyCount = 0;
	
			if(collisionController.boxCollisionDirections.down)
			{
				maxFlyCount = mMaxFlyCount;
			}
		}
				float t = movement.y+maxVelocity;
	
		//Debug.Log(tt);
		//Debug.Log("Movement.y: " + t);
		//Debug.Log("MaxVelocity: " + maxVelocity);
		
		/*if(tt < maxVelocity && isGliding == false && isFlying == true)
		{
			//isGliding = true;
			tt=tt+1.4f;
		}	
		
		if(tt >= maxVelocity)
		{
			isGliding = true;
			tt = 0;
		}*/
		
	}
	
	public void OnFlyKeyDown()
	{	

		if(!isGliding)
		{	
			//jumpAndFallDelay = 0.884f;
			
			if(maxFlyCount != 0)
			{
				if (flyTimer == mFlyTimer)
				{
					isFlying = true;
					movement.y = maxVelocity;
				}
			}
		}
	}
	
	public void OnGlideKeyDown()
	{
		 jumpAndFallDelay = 1.8f;
	}
	
	public void OnGlideKeyUp()
	{

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
		
		
		Instantiate(windBlastObject, new Vector2(transform.position.x,transform.position.y+2f), new Quaternion(0, 0, 0, 0), gameObject.transform);
		Physics2D.IgnoreCollision(windBlastObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
		
		//Instantiate(prefab,transform.position, Quaternion.identity);
		//prefab.transform.SetParent(transform.parent, true);
		//prefab.transform.parent = gameObject.transform;
	}
	
	
	/*public int GetDirection()
	{
		return direction;
	}*/
	
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
