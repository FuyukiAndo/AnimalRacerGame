using System.Collections;
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

public class PlayerInput : MonoBehaviour 
{	
	private PlayerController playerController;
	private PlayerAlbatross playerAlbatross;
	
	public PlayerCharacterType playerCharacterType;
	public AnimationHandler animationHandler;
	
	//Dictionary for KeyCode Actions
	private Dictionary<KeyCode, Action> keyCodeDictionary0 = new Dictionary<KeyCode, Action>();
	private Dictionary<KeyCode, Action> keyCodeDictionary1 = new Dictionary<KeyCode, Action>();
	//private Dictionary<KeyCode, Action> keyCodeDictionary2 = new Dictionary<KeyCode, Action>();
	
	// KeyCode for Player Input
	public KeyCode playerLeftKey = KeyCode.LeftArrow;
	public KeyCode playerRightKey = KeyCode.RightArrow;
	public KeyCode playerJumpKey = KeyCode.A;
	public KeyCode playerAbilityKey = KeyCode.S;
	private KeyCode playerNoKey = KeyCode.None;

    [Range(0.01f, 1f)] public float rotationSpeed = 0.09f;
	
	// Min and max angles for turning animation
	public float minAngleValue = 90.0f;
	public float maxAngleValue = 269.0f;
	
	public float targetAngle;
	private bool isGrounded = true;
	public bool isControllable = true;

	void Start() 
	{
		//Start angle 
		targetAngle = maxAngleValue;
		
		var albatrossComponent = gameObject.GetComponent<PlayerAlbatross>();
		var monkeyComponent = gameObject.GetComponent<PlayerAlbatross>();
		var penguinComponent = gameObject.GetComponent<PlayerAlbatross>();
		var pigComponent = gameObject.GetComponent<PlayerAlbatross>();
		
		//Albatross Setup
		if(albatrossComponent !=null)
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
			//keyCodeDictionary2.Add(playerJumpKey,()=>playerAlbatross.OnGlideKeyDown()); - ???			
		}
		else if(monkeyComponent !=null)
		{
			//do something	
		} 
		else if(penguinComponent !=null)
		{
			//do something	
		}
		else if(pigComponent !=null)
		{
			//do something
		}
		else
		{
			//keyCodeDictionary0.Add(playerJumpKey,()=>playerController.OnJumpKeyDown());	
			//keyCodeDictionary1.Add(playerJumpKey, ()=>playerController.OnJumpKeyUp());
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
		
		if(isControllable && playerCharacterType == PlayerCharacterType.PlayerAlbatross)
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
		
		//Rotates player model when changing direction
		Quaternion target = Quaternion.Euler(new Vector3(0f,targetAngle,0f));
		animationHandler.transform.rotation = Quaternion.Lerp(animationHandler.transform.rotation, target, rotationSpeed);	
	}
	
	void InputAnimationGeneric()
	{		
		if(Input.GetKeyDown(playerLeftKey))
		{
			if(isGrounded)
			{
				animationHandler.SetAnimatorBool("RunT", true);
				animationHandler.SetAnimatorBool("IdleT", false);
			}
		}
		
		if(Input.GetKeyDown(playerRightKey))
		{	
			if(isGrounded)
			{
				animationHandler.SetAnimatorBool("RunT", true);
				animationHandler.SetAnimatorBool("IdleT", false);
			}
		}

		if(Input.GetKeyUp(playerLeftKey))
		{
			if(isGrounded)
			{
				animationHandler.SetAnimatorBool("RunT", false);
				animationHandler.SetAnimatorBool("IdleT", true);
			}
		}
		
		if(Input.GetKeyUp(playerRightKey))
		{	
			if(isGrounded)
			{	
				animationHandler.SetAnimatorBool("RunT", false);
				animationHandler.SetAnimatorBool("IdleT", true);
			}
		}		
	}
	
	void InputAnimationAlbatross()
	{
		if(Input.GetKeyDown(playerLeftKey))
		{
			animationHandler.SetAnimatorBool("EndTheThird", true);			
	
			if(isGrounded)
			{
				animationHandler.SetAnimatorBool("EndGlideRun", false);
			}

			if(!isGrounded)
			{
				animationHandler.SetAnimatorBool("EndGlideRun", true);		
			}
		}
		
		if(Input.GetKeyDown(playerRightKey))
		{
			animationHandler.SetAnimatorBool("EndTheThird", true);	
			
			if(isGrounded)
			{
				animationHandler.SetAnimatorBool("EndGlideRun", false);
			}
		}

		if(Input.GetKeyUp(playerLeftKey))
		{				
			animationHandler.SetAnimatorBool("EndTheThird", false);	
		
			if(isGrounded)
			{	
				animationHandler.SetAnimatorBool("EndGlideRun", false);
			}
		}
		
		if(Input.GetKeyUp(playerRightKey))
		{	
			animationHandler.SetAnimatorBool("EndTheThird", false);	
			
			if(isGrounded)
			{	
				animationHandler.SetAnimatorBool("EndGlideRun", false);
			}
		}
		
		if(Input.GetKeyDown(playerJumpKey))
		{
			animationHandler.SetAnimatorTrigger("WingDown");
		}
		
		if(Input.GetKeyUp(playerJumpKey))
		{
			animationHandler.SetAnimatorTrigger("WingUp");
		}	

		//Collision check for animation bools
		if(playerAlbatross.collisionController.boxCollisionDirections.down)
		{	
			if(playerAlbatross.movement.x != 0)
			{	
				animationHandler.SetAnimatorBool("EndGlideRun", true);					
			}

			if(playerAlbatross.movement.x == 0)
			{
				animationHandler.SetAnimatorBool("EndGlideRun", true);		
				animationHandler.SetAnimatorBool("RunT", false);						
			}
			
			isGrounded = true;
		}
		else
		{
			animationHandler.SetAnimatorBool("EndGlideRun", false);		
			animationHandler.SetAnimatorBool("IdleT", false);	
			animationHandler.SetAnimatorBool("RunT", false);		
			
			isGrounded = false;				
		}
	}
	
	void InputAnimation()
	{
		InputAnimationGeneric();
		InputAnimationRotation();
		
		//If Player Albatross then use Albatross Animations
		if(playerCharacterType == PlayerCharacterType.PlayerAlbatross)
		{
			InputAnimationAlbatross();
		}
		else if(playerCharacterType == PlayerCharacterType.PlayerPenguin)
		{
			//InputAnimationPenguin();
		}
		else if(playerCharacterType == PlayerCharacterType.PlayerPig)
		{
			//InputAnimationPig();
		}
		else if(playerCharacterType == PlayerCharacterType.PlayerMonkey)
		{
			//InputAnimationMonkey();
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
