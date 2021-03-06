﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum PlayerCharacterType
{
	PlayerNobody,
	PlayerAlbatross,
	PlayerPenguin,
	PlayerPig,
	PlayerMonkey
};

/*public enum PlayerStates
{
	PlayerIdle,
	PlayerRun,
	PlayerJump,
	PlayerAbility
};*/

public class PlayerInput : MonoBehaviour 
{	
	private PlayerAlbatross playerAlbatross;
	private PlayerMonkey playerMonkey;
	private PlayerNobody playerNobody;
	
	[HideInInspector] public PlayerCharacterType playerCharacterType;
	
	public AnimationHandler animationHandler;
	
	/* Dictionary for KeyCode Actions */
	private Dictionary<KeyCode, Action> keyCodeDictionary0 = new Dictionary<KeyCode, Action>();
	private Dictionary<KeyCode, Action> keyCodeDictionary1 = new Dictionary<KeyCode, Action>();
	
	/* KeyCode for Player Input */
	public KeyCode playerLeftKey = KeyCode.LeftArrow;
	public KeyCode playerRightKey = KeyCode.RightArrow;
	public KeyCode playerJumpKey = KeyCode.A;
	public KeyCode playerAbilityKey = KeyCode.S;
	private KeyCode playerNoKey = KeyCode.None;

    [Range(0.01f, 1f)] public float rotationSpeed = 0.09f;

	private float minAngleValue = 90.0f;
	private float maxAngleValue = 269.0f;
	
	[HideInInspector] public float targetAngle;
	[HideInInspector] public bool isControllable = true;
	
	public float stunDurationLeft = 1.2f;

	public float GetMaxAngleValue()
	{
		return maxAngleValue;
	}

	void Start() 
	{		
		/* Start angle */
		targetAngle = maxAngleValue;
		
		var albatrossComponent = gameObject.GetComponent<PlayerAlbatross>();
		var monkeyComponent = gameObject.GetComponent<PlayerMonkey>();
		var penguinComponent = gameObject.GetComponent<PlayerAlbatross>();
		var pigComponent = gameObject.GetComponent<PlayerAlbatross>();
		
		/* Character specific keycode setup */
		if(albatrossComponent !=null) // Albatross
		{
			playerAlbatross = GetComponent<PlayerAlbatross>();
			
			playerCharacterType = PlayerCharacterType.PlayerAlbatross;

			//Movement KeyCode
			keyCodeDictionary0.Add(playerNoKey, ()=>playerAlbatross.MoveNot());
			keyCodeDictionary0.Add(playerLeftKey,()=>playerAlbatross.MoveLeft());
			keyCodeDictionary0.Add(playerRightKey,()=>playerAlbatross.MoveRight());	
			
			//Albatross Jump KeyCode Down and Up
			keyCodeDictionary0.Add(playerJumpKey,()=>playerAlbatross.OnFlyKeyDown());
			keyCodeDictionary1.Add(playerJumpKey,()=>playerAlbatross.OnFlyKeyUp());
			
			//Ability KeyCode
			keyCodeDictionary0.Add(playerAbilityKey,()=>playerAlbatross.OnAbilityKey());	
		}
		else if(monkeyComponent !=null) // Monkey
		{
			playerMonkey = GetComponent<PlayerMonkey>();
			playerCharacterType = PlayerCharacterType.PlayerMonkey;		

			keyCodeDictionary0.Add(playerNoKey, ()=>playerMonkey.MoveNot());
			keyCodeDictionary0.Add(playerLeftKey,()=>playerMonkey.MoveLeft());
			keyCodeDictionary0.Add(playerRightKey,()=>playerMonkey.MoveRight());			

			keyCodeDictionary0.Add(playerJumpKey,()=>playerMonkey.OnJumpKeyDown());
			keyCodeDictionary1.Add(playerJumpKey,()=>playerMonkey.OnJumpKeyUp());
			
			//Ability KeyCode
			keyCodeDictionary0.Add(playerAbilityKey,()=>playerMonkey.OnAbilityKey());				
		} 
		else if(penguinComponent !=null) // Penguin
		{
			//do something	
		}
		else if(pigComponent !=null) // Pig
		{
			//do something
		}
		else // Nobody
		{
			//System.Type addType = System.Type.GetType("PlayerNobody");
			//gameObject.AddComponent(addType);
			
			playerNobody = GetComponent<PlayerNobody>();
			playerCharacterType = PlayerCharacterType.PlayerNobody;		
			
			keyCodeDictionary0.Add(playerNoKey, ()=>playerNobody.MoveNot());
			keyCodeDictionary0.Add(playerLeftKey,()=>playerNobody.MoveLeft());
			keyCodeDictionary0.Add(playerRightKey,()=>playerNobody.MoveRight());	

			keyCodeDictionary0.Add(playerJumpKey,()=>playerNobody.OnJumpKeyDown());
			keyCodeDictionary1.Add(playerJumpKey,()=>playerNobody.OnJumpKeyUp());

			keyCodeDictionary0.Add(playerAbilityKey,()=>playerNobody.OnAbilityKey());			
		}
	}
	
	private void StaticZPos()
	{
		Vector3 zpos = transform.position;
		zpos.z = 0;

		transform.position = zpos;		
	}
	
	void InputAction()
	{
		// Move player left
		if(isControllable)
		{
			if(Input.GetKey(playerLeftKey))
			{
				keyCodeDictionary0[playerLeftKey]();
			}

			// Move player right
			if(Input.GetKey(playerRightKey))
			{
				keyCodeDictionary0[playerRightKey]();
			}
			
			// Ability Key
			if(Input.GetKeyDown(playerAbilityKey))
			{
				keyCodeDictionary0[playerAbilityKey]();
			}
			
			// Jump KeyCode Down
			if(Input.GetKeyDown(playerJumpKey))
			{
				keyCodeDictionary0[playerJumpKey]();
			}
			
			// Jump KeyCode Up
			if(Input.GetKeyUp(playerJumpKey))
			{
				keyCodeDictionary1[playerJumpKey]();
			}
		}
		
		//If no KeyCode is pressed
		keyCodeDictionary0[playerNoKey]();
	}

	void InputAnimationRotation()
	{
		if(isControllable)
		{
			if(Input.GetKey(playerLeftKey))
			{
				targetAngle = maxAngleValue;
			}
			
			if(Input.GetKey(playerRightKey))
			{
				targetAngle = minAngleValue;
			}
		}
		
		/* Rotates player model when changing direction */
		Quaternion target = Quaternion.Euler(new Vector3(0f,targetAngle,0f));
		animationHandler.transform.rotation = Quaternion.Lerp(animationHandler.transform.rotation, target, rotationSpeed);	
	}
	
	void InputAnimationGeneric()
	{		
		if(Input.GetKeyDown(playerLeftKey))
		{
			animationHandler.SetAnimatorBool("RunT", true);
			animationHandler.SetAnimatorBool("IdleT", false);
		}
		
		if(Input.GetKeyDown(playerRightKey))
		{	
			animationHandler.SetAnimatorBool("RunT", true);
			animationHandler.SetAnimatorBool("IdleT", false);
		}

		if(Input.GetKeyUp(playerLeftKey))
		{
			animationHandler.SetAnimatorBool("RunT", false);
			animationHandler.SetAnimatorBool("IdleT", true);
		}
		
		if(Input.GetKeyUp(playerRightKey))
		{	
			animationHandler.SetAnimatorBool("RunT", false);
			animationHandler.SetAnimatorBool("IdleT", true);
		}	
	}
	
	void InputAnimationAlbatross()
	{		
		if(Input.GetKeyDown(playerJumpKey))
		{
			animationHandler.SetAnimatorTrigger("WingDown");
		}
		
		if(Input.GetKeyUp(playerJumpKey))
		{
			animationHandler.SetAnimatorTrigger("WingUp");
		}	
		
		if(Input.GetKeyDown(playerAbilityKey))
		{
			animationHandler.SetAnimatorBool("Flap",true);
		}
		
		if(isControllable)
		{
			animationHandler.SetAnimatorBool("Flap",false);
		}

		if(playerAlbatross.collisionController.boxCollisionDirections.down)
		{	
			animationHandler.SetAnimatorBool("Glide", true);	
		}
		else
		{
			animationHandler.SetAnimatorBool("Glide", false);							
		}
	}

	void InputAnimationMonkey()
	{	
		float directionY = Mathf.Sign(playerMonkey.movement.y);
		float directionX = Mathf.Sign(playerMonkey.movement.x);
		
		if(Input.GetKeyDown(playerAbilityKey))
		{
			//Throw banana ability
		}
		
		if(isControllable)
		{
			//Stop monkey from moving
		}
		
		if(directionY == -1)
		{
			animationHandler.SetAnimatorBool("FallT",true);
		}
		
		if(directionY == 1)
		{
			animationHandler.SetAnimatorBool("FallT",false);
		}	

		if(playerMonkey.collisionController.boxCollisionDirections.down)
		{
			if(playerMonkey.movement.x == 0)
			{	
				//animationHandler.SetAnimatorBool("IdleT", true);
				//Debug.Log("this works");
			}

			if(playerMonkey.movement.y == 0)
			{	
				animationHandler.SetAnimatorBool("JumpT", false);				
			}	
			
		}
		else
		{
			if(directionY == 1)
			{
				animationHandler.SetAnimatorBool("JumpT", true);
			}				
		}			
	}
	
	void InputAnimation()
	{
		/* Shared animation functions */
		InputAnimationGeneric();
		InputAnimationRotation();
	
		/* Albatross specific animations */
		if(playerCharacterType == PlayerCharacterType.PlayerAlbatross)
		{
			InputAnimationAlbatross();
		}

		/* Penguin specific animations */
		if(playerCharacterType == PlayerCharacterType.PlayerPenguin)
		{
			//InputAnimationPenguin();
		}

		/* Pig specific animations */		
		if(playerCharacterType == PlayerCharacterType.PlayerPig)
		{
			//InputAnimationPig();
		}

		/* Monkey specific animations */	
		if(playerCharacterType == PlayerCharacterType.PlayerMonkey)
		{
			InputAnimationMonkey();
		}
	}
	
	void Update() 
	{
		StaticZPos();
		InputAction();
		
		if(animationHandler != null)
		{
			InputAnimation();
		}
	}
}
