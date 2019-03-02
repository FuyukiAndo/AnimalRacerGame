using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


enum PlayerCharacterType
{
	PlayerNobody,
	PlayerAlbatross,
	PlayerPenguin,
	PlayerPig,
	PlayerMonkey
};

//[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour 
{	
	PlayerController playerController;
	public AnimationHandler animationHandler;
	
	Dictionary<KeyCode, Action> keyDictionary = new Dictionary<KeyCode, Action>();
	Dictionary<KeyCode, Action> anotherDictionary = new Dictionary<KeyCode, Action>();
	
	public KeyCode playerLeftKey = KeyCode.LeftArrow;
	public KeyCode playerRightKey = KeyCode.RightArrow;
	public KeyCode playerJumpKey = KeyCode.A;
	public KeyCode playerAbilityKey = KeyCode.S; //Doesn't do anything right now
	private KeyCode playerNoKey = KeyCode.None;

    [Range(0.01f, 1f)] public float speed = 0.25f;
	private float targetAngle = 179f+90f;
	public bool rotationActive = false; 
	public bool groundedMovement = true;

	// Use this for initialization
	void Start() 
	{
		//Starter character setup
		PlayerCharacterType playerCharacterType;
		playerCharacterType = PlayerCharacterType.PlayerNobody;
		
		playerController = GetComponent<PlayerController>();
		
		var script0 = gameObject.GetComponent<PlayerAlbatross>();
		
		keyDictionary.Add(playerNoKey, ()=>playerController.MoveNot());
		keyDictionary.Add(playerLeftKey,()=>playerController.MoveLeft());
		keyDictionary.Add(playerRightKey,()=>playerController.MoveRight());	
		
		//Albatross Setup
		if(script0 !=null)
		{
			PlayerAlbatross playerAlbatross;
			playerAlbatross = GetComponent<PlayerAlbatross>();
			playerCharacterType = PlayerCharacterType.PlayerAlbatross;
			
			keyDictionary.Add(playerJumpKey,()=>playerAlbatross.OnFlyKeyDown());
			anotherDictionary.Add(playerJumpKey, ()=>playerAlbatross.OnFlyKeyUp());
		}
		else
		{
			keyDictionary.Add(playerJumpKey,()=>playerController.OnJumpKeyDown());	
			anotherDictionary.Add(playerJumpKey, ()=>playerController.OnJumpKeyUp());
		}
	}
	
	void StaticZPos()
	{
		Vector3 zpos = transform.position;
		zpos.z = 0;

		transform.position = zpos;		
	}
	
	void InputAction()
	{
		//move player left
		if(Input.GetKey(playerLeftKey))
		{
			keyDictionary[playerLeftKey]();
		}

		//move player right
		if(Input.GetKey(playerRightKey))
		{
			keyDictionary[playerRightKey]();
		}	
		
		//max value of jump height
		if(Input.GetKeyDown(playerJumpKey))
		{
			keyDictionary[playerJumpKey]();
		}
		
		//min value of jump height
		if(Input.GetKeyUp(playerJumpKey))
		{
			anotherDictionary[playerJumpKey]();
		}
		
		//if no key is pressed	
		keyDictionary[playerNoKey]();
	}
	
	void InputAnimation()
	{
		if(Input.GetKey(playerLeftKey))
		{
			rotationActive = true;
			targetAngle = 179f+90f;
		}
		
		if(Input.GetKey(playerRightKey))
		{
			rotationActive = true;
			targetAngle = 1f+90f;
		}	
		
		if(Input.GetKeyDown(playerLeftKey))
		{
			if(groundedMovement)
			{
				animationHandler.SetAnimatorTrigger("Run");
			}
		}
		
		if(Input.GetKeyDown(playerRightKey))
		{
			if(groundedMovement)
			{
				animationHandler.SetAnimatorTrigger("Run");
			}
		}

		if(Input.GetKeyUp(playerLeftKey))
		{
			if(groundedMovement)
			{
				animationHandler.SetAnimatorTrigger("Idle");
			}
		}
		
		if(Input.GetKeyUp(playerRightKey))
		{
			if(groundedMovement)
			{
				animationHandler.SetAnimatorTrigger("Idle");
			}
		}
		
		if(Input.GetKeyDown(playerJumpKey))
		{
			//groundedMovement = false;
			//animationHandler.SetAnimatorTrigger("Jump");
		}
		
		//Rotates player model when changing direction
		if(rotationActive)
		{
			Quaternion target = Quaternion.Euler(new Vector3(0f,targetAngle,0f));
			animationHandler.transform.rotation = Quaternion.Lerp(animationHandler.transform.rotation, target, speed);	
		}
	}
	
	void Update() 
	{		
		InputAction();
		
		if(animationHandler != null)
		{
			InputAnimation();
		}
		
		StaticZPos();
	}
}
