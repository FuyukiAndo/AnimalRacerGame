using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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
	[Header("KeyCode Settings")]
	public KeyCode playerLeftKey = KeyCode.LeftArrow;
	public KeyCode playerRightKey = KeyCode.RightArrow;
	public KeyCode playerJumpKey = KeyCode.A;
	public KeyCode playerAbilityKey = KeyCode.S;

	[Header("Miscellaneous Settings")]
	public float stunDurationTimer = 1.2f;

	[Range(0.01f, 1f)] public float rotationSpeed = 0.09f;
	public AnimationHandler animationHandler;


	[HideInInspector] public PlayerCharacterType playerCharacterType;

	[HideInInspector] public float targetAngle;

	[HideInInspector] public bool isStunned = false;
	public bool isControllable = true;
	public bool changeAngle = true;

	private PlayerAlbatross playerAlbatross;

	private PlayerMonkey playerMonkey;

	private PlayerPig playerPig;

	private PlayerPenguin playerPenguin;

	private float minAngleValue = 90.0f;
	private float maxAngleValue = 269.0f;
	private float savedStunDurationTimer;

	private KeyCode playerNoKey = KeyCode.None;

	private Dictionary<KeyCode, Action> keyCodeDictionary0 = new Dictionary<KeyCode, Action>();
	private Dictionary<KeyCode, Action> keyCodeDictionary1 = new Dictionary<KeyCode, Action>();

	//Emil AudioOneshotPlayer
	[SerializeField] private AudioOneshotPlayer oneshotPlayer;
	private bool triggeredAnger = false;
	//Emil SpeechBubble
	[SerializeField] private SpeechBubble playerSpeech;

	void Start()
	{
		targetAngle = maxAngleValue;

		var albatrossComponent = gameObject.GetComponent<PlayerAlbatross>();
		var monkeyComponent = gameObject.GetComponent<PlayerMonkey>();
		var penguinComponent = gameObject.GetComponent<PlayerPenguin>();
		var pigComponent = gameObject.GetComponent<PlayerPig>();

		savedStunDurationTimer = stunDurationTimer;

		if (albatrossComponent != null)
		{
			playerAlbatross = GetComponent<PlayerAlbatross>();

			playerCharacterType = PlayerCharacterType.PlayerAlbatross;

			keyCodeDictionary0.Add(playerNoKey, () => playerAlbatross.MoveNot());

			keyCodeDictionary0.Add(playerLeftKey, () => playerAlbatross.MoveLeft());
			keyCodeDictionary0.Add(playerRightKey, () => playerAlbatross.MoveRight());

			keyCodeDictionary0.Add(playerJumpKey, () => playerAlbatross.OnJumpKeyDown());
			keyCodeDictionary1.Add(playerJumpKey, () => playerAlbatross.OnJumpKeyUp());

			keyCodeDictionary0.Add(playerAbilityKey, () => playerAlbatross.OnAbilityKey());
		}
		else if (monkeyComponent != null)
		{
			playerMonkey = GetComponent<PlayerMonkey>();

			playerCharacterType = PlayerCharacterType.PlayerMonkey;

			keyCodeDictionary0.Add(playerNoKey, () => playerMonkey.MoveNot());

			keyCodeDictionary0.Add(playerLeftKey, () => playerMonkey.MoveLeft());
			keyCodeDictionary0.Add(playerRightKey, () => playerMonkey.MoveRight());

			keyCodeDictionary0.Add(playerJumpKey, () => playerMonkey.OnJumpKeyDown());
			keyCodeDictionary1.Add(playerJumpKey, () => playerMonkey.OnJumpKeyUp());

			keyCodeDictionary0.Add(playerAbilityKey, () => playerMonkey.OnAbilityKey());
		}
		else if (penguinComponent != null)
		{
			playerPenguin = GetComponent<PlayerPenguin>();

			playerCharacterType = PlayerCharacterType.PlayerPenguin;

			keyCodeDictionary0.Add(playerNoKey, () => playerPenguin.MoveNot());

			keyCodeDictionary0.Add(playerLeftKey, () => playerPenguin.MoveLeft());
			keyCodeDictionary0.Add(playerRightKey, () => playerPenguin.MoveRight());

			keyCodeDictionary0.Add(playerJumpKey, () => playerPenguin.OnJumpKeyDown());
			keyCodeDictionary1.Add(playerJumpKey, () => playerPenguin.OnJumpKeyUp());

			keyCodeDictionary0.Add(playerAbilityKey, () => playerPenguin.OnAbilityKey());
		}
		else if (pigComponent != null)
		{
			playerPig = GetComponent<PlayerPig>();

			playerCharacterType = PlayerCharacterType.PlayerPig;

			keyCodeDictionary0.Add(playerNoKey, () => playerPig.MoveNot());

			keyCodeDictionary0.Add(playerLeftKey, () => playerPig.MoveLeft());
			keyCodeDictionary0.Add(playerRightKey, () => playerPig.MoveRight());

			keyCodeDictionary0.Add(playerJumpKey, () => playerPig.OnJumpKeyDown());
			keyCodeDictionary1.Add(playerJumpKey, () => playerPig.OnJumpKeyUp());

			keyCodeDictionary0.Add(playerAbilityKey, () => playerPig.OnAbilityKey());
		}

		//Emil AudioOneshotPlayer
		oneshotPlayer = GetComponent<AudioOneshotPlayer>();
		playerSpeech = FindObjectsOfType<SpeechBubble>().Where(bubble => bubble.name == name).FirstOrDefault();
	}

	private void StaticZPosition()
	{
		Vector3 zpos = transform.position;
		zpos.z = 0;

		transform.position = zpos;
	}

	private void InputAction()
	{
		if (!isStunned)
		{
			if (isControllable)
			{
				//Move Left
				if (Input.GetKey(playerLeftKey))
				{
					keyCodeDictionary0[playerLeftKey]();
				}

				//Move Right
				if (Input.GetKey(playerRightKey))
				{
					keyCodeDictionary0[playerRightKey]();
				}

				//Jump Key Down
				if (Input.GetKeyDown(playerJumpKey))
				{
					keyCodeDictionary0[playerJumpKey]();
				}

				//Jump Key Up
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
		else
		{
			//Emil AudioOneshotPlayer
			if (!triggeredAnger)
			{
				triggeredAnger = true;
				oneshotPlayer.SetParameterValue(1.5f);
				oneshotPlayer.PlayAudioOneShot();
				if (playerSpeech != null)
				{
					playerSpeech.SetSpeechActive(SpeechType.stun, playerCharacterType);
				}
			}
			isStunned = true;
			changeAngle = false;
			stunDurationTimer -= Time.deltaTime;

			if (stunDurationTimer < 0)
			{
				isStunned = false;
				stunDurationTimer = savedStunDurationTimer;
				changeAngle = true;
				//Emil AudioOneshotPlayer
				triggeredAnger = false;
			}
		}

		//If no KeyCode is pressed
		keyCodeDictionary0[playerNoKey]();
	}

	private void InputAnimationRotation()
	{
		if (changeAngle)
		{
			if (Input.GetKey(playerLeftKey))
			{
				targetAngle = maxAngleValue;
			}

			if (Input.GetKey(playerRightKey))
			{
				targetAngle = minAngleValue;
			}
		}

		//Rotates player model when changing direction
		Quaternion target = Quaternion.Euler(new Vector3(0f, targetAngle, 0f));
		animationHandler.transform.rotation = Quaternion.Lerp(animationHandler.transform.rotation, target, rotationSpeed);
	}

	private void InputAnimationGeneric()
	{
		if (Input.GetKey(playerLeftKey))
		{
			animationHandler.SetAnimatorBool("RunT", true);
			animationHandler.SetAnimatorBool("IdleT", false);
		}

		if (Input.GetKey(playerRightKey))
		{
			animationHandler.SetAnimatorBool("RunT", true);
			animationHandler.SetAnimatorBool("IdleT", false);
		}

		if (Input.GetKeyUp(playerLeftKey))
		{
			animationHandler.SetAnimatorBool("RunT", false);
			animationHandler.SetAnimatorBool("IdleT", true);
		}

		if (Input.GetKeyUp(playerRightKey))
		{
			animationHandler.SetAnimatorBool("RunT", false);
			animationHandler.SetAnimatorBool("IdleT", true);
		}

		if (Input.GetKeyDown(playerAbilityKey))
		{
			//Emil AudioOneshotPlayer
			//oneshotPlayer.SetParameterValue(2.5f);
			//oneshotPlayer.PlayAudioOneShot(true);
			animationHandler.SetAnimatorBool("SpecialT",true);
		}		

		/*if (Input.GetKeyDown(playerJumpKey))
		{
			//Emil AudioOneshotPlayer
			oneshotPlayer.SetParameterValue(0.05f);
			oneshotPlayer.PlayAudioOneShot(true);
		}*/
		
		
		if(isStunned)
		{
			animationHandler.SetAnimatorBool("StunT", true);
		}
		else
		{
			animationHandler.SetAnimatorBool("StunT", false);
		}

	}

	private void InputAnimationAlbatross()
	{
		if (Input.GetKeyDown(playerJumpKey))
		{
			animationHandler.SetAnimatorTrigger("WingDown");
		}

		if (Input.GetKeyUp(playerJumpKey))
		{
			animationHandler.SetAnimatorTrigger("WingUp");
		}

		if (isControllable)
		{
			animationHandler.SetAnimatorBool("SpecialT", false);
		}

		if (playerAlbatross.collisionController.boxCollisionDirections.down)
		{
			animationHandler.SetAnimatorBool("Glide", true);
		}
		else
		{
			animationHandler.SetAnimatorBool("Glide", false);
		}
	}

	private void InputAnimationPig()
	{
		float directionY = Mathf.Sign(playerPig.movement.y);

		if (Input.GetKey(playerJumpKey))
		{
			if (directionY == -1 && playerPig.isActiveAbility)
			{
				animationHandler.SetAnimatorBool("SpecialJumpT", true);
			}

			if (directionY == 1 && playerPig.isActiveAbility)
			{
				animationHandler.SetAnimatorBool("JumpT", true);
			}
		}

		if (Input.GetKeyUp(playerJumpKey))
		{
			if (playerPig.collisionController.boxCollisionDirections.down)
			{
				animationHandler.SetAnimatorBool("JumpT", false);
			}
		}

		if (Input.GetKeyDown(playerAbilityKey))
		{
			if (!playerPig.passiveAbility && !playerPig.isPassiveAbility)
			{
				animationHandler.SetAnimatorBool("SpecialT", true);
				playerPig.passiveAbility = true;

				isControllable = false;
				changeAngle = false;
			}
		}

		if (!playerPig.passiveAbility)
		{
			animationHandler.SetAnimatorBool("SpecialT", false);

			isControllable = true;
			changeAngle = true;
		}

		if (!playerPig.isActiveAbility)
		{
			animationHandler.SetAnimatorBool("SpecialJumpT", false);
		}

		if (playerPig.collisionController.boxCollisionDirections.down)
		{
			if (playerPig.movement.y == 0)
			{
				if (!playerPig.activeAbility)
				{
					animationHandler.SetAnimatorBool("JumpT", false);
					animationHandler.SetAnimatorBool("JumpSpecialT", false);
				}
				else
				{
					animationHandler.SetAnimatorBool("JumpT", true);
					animationHandler.SetAnimatorBool("JumpSpecialT", true);
				}

				if (playerPig.movement.x == 0)
				{
					animationHandler.SetAnimatorBool("IdleT", true);
					animationHandler.SetAnimatorBool("JumpT", false);
				}
			}
		}
	}

	private void InputAnimationPenguin()
	{
		if (Input.GetKeyDown(playerJumpKey))
		{
			animationHandler.SetAnimatorBool("JumpT", true);
		}

		if (playerPenguin.collisionController.boxCollisionDirections.down)
		{
			if (playerPenguin.movement.x == 0)
			{
				animationHandler.SetAnimatorBool("IdleT", true);
			}

			if (playerPenguin.movement.y == 0)
			{
				animationHandler.SetAnimatorBool("JumpT", false);
			}
		}

		if (playerPenguin.abilityMeter == 1f)
		{
			animationHandler.SetAnimatorBool("ClimbActive", false);
		}
	}

	private void InputAnimationMonkey()
	{
		if (Input.GetKeyUp(playerLeftKey) || Input.GetKeyUp(playerRightKey))
		{
			playerMonkey.activeAbility = false;

			if (playerMonkey.isActiveAbility)
			{
				animationHandler.SetAnimatorBool("ClimbInactive", true);
			}
		}

		if (Input.GetKeyDown(playerLeftKey) || Input.GetKeyDown(playerRightKey))
		{
			if (playerMonkey.isActiveAbility)
			{
				animationHandler.SetAnimatorBool("ClimbInactive", false);
			}
		}

		if (Input.GetKeyDown(playerJumpKey))
		{
			animationHandler.SetAnimatorBool("JumpT", true);
		}

		if (Input.GetKeyDown(playerAbilityKey))
		{
			if (!playerMonkey.passiveAbility)
			{
				if (playerMonkey.movement.x == 0)
				{
					animationHandler.SetAnimatorTrigger("SpecialT"); //Bool("SpecialT", true);		
				}
			}
		}

		/*if(playerMonkey.abilityMeter == 1f)
		{
			animationHandler.SetAnimatorBool("SpecialT", false);	
		}*/

		if ((playerMonkey.collisionController.boxCollisionDirections.left || playerMonkey.collisionController.boxCollisionDirections.right)
			&& !playerMonkey.collisionController.boxCollisionDirections.down)
		{
			animationHandler.SetAnimatorBool("ClimbActive", true);
		}
		else
		{
			animationHandler.SetAnimatorBool("ClimbActive", false);
			animationHandler.SetAnimatorBool("ClimbInactive", false);

			animationHandler.SetAnimatorBool("IdleT", true);
		}

		if (playerMonkey.collisionController.boxCollisionDirections.down)
		{
			if (playerMonkey.movement.y == 0)
			{
				animationHandler.SetAnimatorBool("JumpT", false);
			}
		}
	}

	private void InputAnimation()
	{
		InputAnimationGeneric();
		InputAnimationRotation();

		if (playerCharacterType == PlayerCharacterType.PlayerAlbatross)
		{
			InputAnimationAlbatross();
		}

		if (playerCharacterType == PlayerCharacterType.PlayerPenguin)
		{
			InputAnimationPenguin();
		}

		if (playerCharacterType == PlayerCharacterType.PlayerPig)
		{
			InputAnimationPig();
		}

		if (playerCharacterType == PlayerCharacterType.PlayerMonkey)
		{
			InputAnimationMonkey();
		}
	}

	void Update()
	{
		StaticZPosition();
		InputAction();

		if (animationHandler != null)
		{
			InputAnimation();
		}
	}

	public float GetMaxAngleValue()
	{
		return maxAngleValue;
	}
}