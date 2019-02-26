using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour 
{	
	PlayerController playerController;
	
	Dictionary<KeyCode, Action> keyDictionary = new Dictionary<KeyCode, Action>();
	Dictionary<KeyCode, Action> anotherDictionary = new Dictionary<KeyCode, Action>();
	
	public KeyCode playerLeftKey = KeyCode.LeftArrow;
	public KeyCode playerRightKey = KeyCode.RightArrow;
	public KeyCode playerJumpKey = KeyCode.A;
	public KeyCode playerAbilityKey = KeyCode.S; //Doesn't do anything right now
	
	private KeyCode playerNoKey = KeyCode.None;
	//int direction = 1;

	//public KeyCode playerJumpKey = KeyCode.A;
	//public KeyCode playerAbilityKey = KeyCode.S;

	// Use this for initialization
	void Start() 
	{
		playerController = GetComponent<PlayerController>();
		
		keyDictionary.Add(playerNoKey, ()=>playerController.MoveNot());
		keyDictionary.Add(playerLeftKey,()=>playerController.MoveLeft());
		keyDictionary.Add(playerRightKey,()=>playerController.MoveRight());
		keyDictionary.Add(playerJumpKey,()=>playerController.OnJumpKeyDown());
		anotherDictionary.Add(playerJumpKey, ()=>playerController.OnJumpKeyUp());
		
	//	keyDictionary.Add(playerJumpKey, ()=>playerController.OnJumpKeyDown());
	}
	
	
	
	
	/*private void MoveLeft()
	{
		//playerController.movement.x = -direction * playerController.movementSpeed;
		//playerController.direction = -1;
	}

	private void MoveRight()
	{
		//playerController.movement.x = direction * playerController.movementSpeed;
		playerController.direction = 1;
	}
	
	private void NoKey()
	{
		playerController.direction = 0;
	}*/
	
	// Update is called once per frame
	void Update() 
	{
		if(Input.GetKey(playerLeftKey))
		{
			keyDictionary[playerLeftKey]();
		}

		if(Input.GetKey(playerRightKey))
		{
			keyDictionary[playerRightKey]();
		}	
		
		if(Input.GetKeyDown(playerJumpKey))
		{
			keyDictionary[playerJumpKey]();
		}
		
		if(Input.GetKeyUp(playerJumpKey))
		{
			anotherDictionary[playerJumpKey]();
		}
		
		//No key
		keyDictionary[playerNoKey]();
		
	}
}
