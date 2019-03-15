using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum PlayerCharacterType
{
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
	private PlayerPig playerPig;
	private PlayerPenguin playerPenguin;
	
	public AnimationHandler animationHandler;
	
	[HideInInspector] public PlayerCharacterType playerCharacterType;

	private Dictionary<KeyCode, Action> keyCodeDictionary0 = new Dictionary<KeyCode, Action>();
	private Dictionary<KeyCode, Action> keyCodeDictionary1 = new Dictionary<KeyCode, Action>();

	public KeyCode playerLeftKey = KeyCode.LeftArrow;
	public KeyCode playerRightKey = KeyCode.RightArrow;
	public KeyCode playerJumpKey = KeyCode.A;
	public KeyCode playerAbilityKey = KeyCode.S;
	
	private KeyCode playerNoKey = KeyCode.None;

    [Range(0.01f, 1f)] public float rotationSpeed = 0.09f;
	
	[HideInInspector] public bool isControllable = true;
	[HideInInspector] public bool changeAngle = true;

	public float stunDurationLeft = 1.2f;

	public float GetMaxAngleValue()
	{
		return maxAngleValue;
	}

	private float minAngleValue = 90.0f;
	private float maxAngleValue = 269.0f;
	
	[HideInInspector] public float targetAngle;

	void Start() 
	{
		animationHandler.speed = 0;
		
		targetAngle = maxAngleValue;
		
		var albatrossComponent = gameObject.GetComponent<PlayerAlbatross>();
		var monkeyComponent = gameObject.GetComponent<PlayerMonkey>();
		var penguinComponent = gameObject.GetComponent<PlayerPenguin>();
		var pigComponent = gameObject.GetComponent<PlayerPig>();

		if(albatrossComponent !=null)
		{
			playerAlbatross = GetComponent<PlayerAlbatross>();

			playerCharacterType = PlayerCharacterType.PlayerAlbatross;

			keyCodeDictionary0.Add(playerNoKey, ()=>playerAlbatross.MoveNot());
			
			keyCodeDictionary0.Add(playerLeftKey,()=>playerAlbatross.MoveLeft());
			keyCodeDictionary0.Add(playerRightKey,()=>playerAlbatross.MoveRight());	
			
			keyCodeDictionary0.Add(playerJumpKey,()=>playerAlbatross.OnJumpKeyDown());
			keyCodeDictionary1.Add(playerJumpKey,()=>playerAlbatross.OnJumpKeyUp());

			keyCodeDictionary0.Add(playerAbilityKey,()=>playerAlbatross.OnAbilityKey());	
		}
		else if(monkeyComponent !=null)
		{
			playerMonkey = GetComponent<PlayerMonkey>();
			
			playerCharacterType = PlayerCharacterType.PlayerMonkey;		

			keyCodeDictionary0.Add(playerNoKey, ()=>playerMonkey.MoveNot());
			
			keyCodeDictionary0.Add(playerLeftKey,()=>playerMonkey.MoveLeft());
			keyCodeDictionary0.Add(playerRightKey,()=>playerMonkey.MoveRight());			

			keyCodeDictionary0.Add(playerJumpKey,()=>playerMonkey.OnJumpKeyDown());
			keyCodeDictionary1.Add(playerJumpKey,()=>playerMonkey.OnJumpKeyUp());

			keyCodeDictionary0.Add(playerAbilityKey,()=>playerMonkey.OnAbilityKey());				
		} 
		else if(penguinComponent !=null)
		{
			playerPenguin = GetComponent<PlayerPenguin>();
			
			playerCharacterType = PlayerCharacterType.PlayerPenguin;		

			keyCodeDictionary0.Add(playerNoKey, ()=>playerPenguin.MoveNot());
			
			keyCodeDictionary0.Add(playerLeftKey,()=>playerPenguin.MoveLeft());
			keyCodeDictionary0.Add(playerRightKey,()=>playerPenguin.MoveRight());			

			keyCodeDictionary0.Add(playerJumpKey,()=>playerPenguin.OnJumpKeyDown());
			keyCodeDictionary1.Add(playerJumpKey,()=>playerPenguin.OnJumpKeyUp());

			keyCodeDictionary0.Add(playerAbilityKey,()=>playerPenguin.OnAbilityKey());	
		}
		else if(pigComponent !=null)
		{
			playerPig = GetComponent<PlayerPig>();
			
			playerCharacterType = PlayerCharacterType.PlayerPig;		

			keyCodeDictionary0.Add(playerNoKey, ()=>playerPig.MoveNot());
			
			keyCodeDictionary0.Add(playerLeftKey,()=>playerPig.MoveLeft());
			keyCodeDictionary0.Add(playerRightKey,()=>playerPig.MoveRight());			

			keyCodeDictionary0.Add(playerJumpKey,()=>playerPig.OnJumpKeyDown());
			keyCodeDictionary1.Add(playerJumpKey,()=>playerPig.OnJumpKeyUp());

			keyCodeDictionary0.Add(playerAbilityKey,()=>playerPig.OnAbilityKey());
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
        stunDurationLeft -= Time.deltaTime;
        // Move player left
        if (stunDurationLeft < 0)
        {
            if (isControllable)
            {
                if (Input.GetKey(playerLeftKey))
                {
                    keyCodeDictionary0[playerLeftKey]();
                }

                // Move player right
                if (Input.GetKey(playerRightKey))
                {
                    keyCodeDictionary0[playerRightKey]();
                }

                // Jump KeyCode Down
                if (Input.GetKeyDown(playerJumpKey))
                {
                    keyCodeDictionary0[playerJumpKey]();
                }

                // Jump KeyCode Up
                if (Input.GetKeyUp(playerJumpKey))
                {
                    keyCodeDictionary1[playerJumpKey]();
                }

                // Ability Key
                if (Input.GetKeyDown(playerAbilityKey))
                {
                    keyCodeDictionary0[playerAbilityKey]();
                }
            }
        }
		
		//If no KeyCode is pressed
		keyCodeDictionary0[playerNoKey]();
	}

	void InputAnimationRotation()
	{	
		if(changeAngle)
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
		
		// Rotates player model when changing direction
		Quaternion target = Quaternion.Euler(new Vector3(0f,targetAngle,0f));
		animationHandler.transform.rotation = Quaternion.Lerp(animationHandler.transform.rotation, target, rotationSpeed);	
	}
	
	void InputAnimationGeneric()
	{		
		if(Input.GetKey(playerLeftKey))
		{
			animationHandler.SetAnimatorBool("RunT", true);
			animationHandler.SetAnimatorBool("IdleT", false);
		}
		
		if(Input.GetKey(playerRightKey))
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
		
		if(Input.GetKeyDown(playerAbilityKey))
		{
			animationHandler.SetAnimatorBool("SpecialT",true);
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

		if(isControllable)
		{
			animationHandler.SetAnimatorBool("SpecialT",false);
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
	
	void InputAnimationPig()
	{		
		float directionY = Mathf.Sign(playerPig.movement.y);
	
		if(Input.GetKey(playerJumpKey))
		{	
			if(directionY == -1 && playerPig.isJump)
			{
				animationHandler.SetAnimatorBool("SpecialJumpT", true);
			}
			
			if(directionY == 1 && playerPig.isJump)
			{
				animationHandler.SetAnimatorBool("JumpT", true);
			}
		}
		
		/*if(playerPig.isJump)
		{
			animationHandler.SetAnimatorBool("SpecialJumpT", true);		
		}*/
		
		if(Input.GetKeyUp(playerJumpKey))
		{
			if(playerPig.collisionController.boxCollisionDirections.down)
			{
				animationHandler.SetAnimatorBool("JumpT", false);
			}
		}	
		
		if(Input.GetKeyDown(playerAbilityKey))
		{
			if(!playerPig.blowUp)
			{
				animationHandler.SetAnimatorBool("SpecialT",true);
				playerPig.blowUp = true;
				isControllable = false;
				changeAngle = false;
			}
		}
		
		if(!playerPig.blowUp)
		{
			animationHandler.SetAnimatorBool("SpecialT",false);
			isControllable = true;
			changeAngle = true;
		}	

			if(!playerPig.isJump)
			{
				animationHandler.SetAnimatorBool("SpecialJumpT", false);	
			}
		
		if(playerPig.collisionController.boxCollisionDirections.down)
		{	
			if(playerPig.movement.x == 0)
			{
				animationHandler.SetAnimatorBool("IdleT", true);
				//animationHandler.SetAnimatorBool("RunT", false);
				animationHandler.SetAnimatorBool("JumpT", false);
			}			
		}
		
		if(playerPig.collisionController.boxCollisionDirections.down && playerPig.movement.y == 0)
		{	
	
			if(!playerPig.GetSignal())
			{
				animationHandler.SetAnimatorBool("JumpT", false);	
				animationHandler.SetAnimatorBool("JumpSpecialT", false);
			}
			else
			{
				animationHandler.SetAnimatorBool("JumpT", true);	
				animationHandler.SetAnimatorBool("JumpSpecialT", true);					
			}
			
			if(playerPig.movement.x == 0)
			{
				animationHandler.SetAnimatorBool("IdleT", true);
				
			}			
		}
	}

	void InputAnimationPenguin()
	{	
		if(Input.GetKeyDown(playerJumpKey))
		{
			animationHandler.SetAnimatorBool("JumpT", true);
		}	
		
		if(playerPenguin.collisionController.boxCollisionDirections.down)
		{
			if(playerPenguin.movement.x == 0)
			{
				animationHandler.SetAnimatorBool("IdleT", true);
			}
			
			if(playerPenguin.movement.y == 0)
			{	
				animationHandler.SetAnimatorBool("JumpT", false);				
			}	
		}
			
	}

	void InputAnimationMonkey()
	{	
		if(Input.GetKeyUp(playerLeftKey) || Input.GetKeyUp(playerRightKey))
		{
			if(playerMonkey.isActiveAbility)//isClimbing)
			{
				playerMonkey.movement.y = 0;
			}
		}
		
		/*if(Input.GetKeyUp(playerRightKey))
		{
			if(playerMonkey.isActiveAbility)//isClimbing)
			{
				playerMonkey.movement.y = 0;
			}
		}*/
	
		float directionY = Mathf.Sign(playerMonkey.movement.y);
		float directionX = Mathf.Sign(playerMonkey.movement.x);
		
		if(directionX == 1)
		{
			targetAngle = minAngleValue;
		}
		
		if(directionX == -1)
		{
			targetAngle = maxAngleValue;
		}
		
		if(directionY == -1)
		{
			animationHandler.SetAnimatorBool("FallT",true);
		}
		
		if(directionY == 1)
		{
			animationHandler.SetAnimatorBool("FallT",false);
		}	

		if((playerMonkey.collisionController.boxCollisionDirections.left || playerMonkey.collisionController.boxCollisionDirections.right)
		&& !playerMonkey.collisionController.boxCollisionDirections.down /*&& playerMonkey.movement.y < 0*/)
		{
			animationHandler.SetAnimatorBool("SpecialT", true);
			animationHandler.SetAnimatorBool("IdleT", false);
			animationHandler.SetAnimatorBool("RunT", false);
			animationHandler.SetAnimatorBool("JumpT", false);
			animationHandler.SetAnimatorBool("FallT", false);
		}
		else
		{
			animationHandler.SetAnimatorBool("SpecialT", false);
			
			if(playerMonkey.movement.x != 0)
			{
				animationHandler.SetAnimatorBool("RunT", true);	
				animationHandler.SetAnimatorBool("IdleT", false);			
			}
			else
			{
				animationHandler.SetAnimatorBool("RunT", false);	
				animationHandler.SetAnimatorBool("IdleT", true);				
			}
			
		}


		if(playerMonkey.collisionController.boxCollisionDirections.down)
		{
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
			InputAnimationPenguin();
		}

		/* Pig specific animations */		
		if(playerCharacterType == PlayerCharacterType.PlayerPig)
		{
			InputAnimationPig();
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
