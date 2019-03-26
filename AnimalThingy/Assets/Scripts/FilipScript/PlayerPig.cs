using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPig : PlayerController
{
	[Header("Parameter Counter Settings")]

	public float deltaJumpCounter = 0.25f;
	public float horizontalKnockBackPower = 9f;
	public float verticalKnockBackPower = 9f;

	private float blowUpTimer = 0.71f;
	private float savedBlowUpTimer;
	private bool canSuperJump = true;
	private float savedDeltaJumpCounter;
	private int jumpIncreaseCounter = 0;
	private BoxCollider2D boxCollider2D;
	private float tempX, tempY;
	private float newX;

	public override void Start()
	{
		base.Start();
		savedDeltaJumpCounter = deltaJumpCounter;
		boxCollider2D = GetComponent<BoxCollider2D>();

		tempX = boxCollider2D.size.x;
		tempY = boxCollider2D.size.y;
		newX = 5.75f;
		savedBlowUpTimer = blowUpTimer;

		boxCollider2D.size = new Vector2(tempX, tempY);
		directionY = Mathf.Sign(movement.y);
	}

	public override void OnJumpKeyDown()
	{
		if (maxUseCounter > 0)
		{
			if (collisionController.boxCollisionDirections.down)
			{
				base.OnJumpKeyDown();
				isActiveAbility = true;
				jumpIncreaseCounter++;

				if (directionY == 1)
				{
					maxUseCounter--;
					deltaJumpCounter = savedDeltaJumpCounter;
				}

				movement.y += abilityModifier * jumpIncreaseCounter * 2f;
			}
		}
	}

	public override void OnAbilityKey()
	{
		base.OnAbilityKey();
		playerInput.isControllable = false;
		playerInput.changeAngle = false;
		abilityMeter = 0;
	}

	private void PlayerPassiveAbility()
	{
		if (passiveAbility)
		{
			boxCollider2D.size = new Vector2(newX, tempY + .28f);
			blowUpTimer -= Time.deltaTime;

			if (blowUpTimer < 0f)
			{
				passiveAbility = false;
				isPassiveAbility = true;
				boxCollider2D.size = new Vector2(tempX, tempY);
			}
		}
		else
		{
			blowUpTimer = savedBlowUpTimer;
		}
	}

	private void PlayerActiveAbility()
	{
		if (collisionController.boxCollisionDirections.down)
		{
			if (maxUseCounter < 0)
			{
				canSuperJump = true;
			}

			if (deltaJumpCounter < 0)
			{
				deltaJumpCounter = savedDeltaJumpCounter;
			}

			if (isActiveAbility)
			{
				deltaJumpCounter -= Time.deltaTime;
				activeAbility = true;
			}
		}
		else
		{
			if (maxUseCounter < 0)
			{
				canSuperJump = false;
			}
		}

		if (deltaJumpCounter < 0)
		{
			isActiveAbility = false;
			activeAbility = false;
			deltaJumpCounter = savedDeltaJumpCounter;
			jumpIncreaseCounter = 0;
			maxUseCounter = savedMaxUseCounter;
		}
	}

	public override void OpponentProjectileCollision()
	{
		base.OpponentProjectileCollision();

		if (passiveAbility)
		{
			collision = Physics2D.OverlapCircleAll(transform.position, 1.8f);

			for (int i = 0; i < collision.Length; i++)
			{
				if (collision[i].gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
				{
					if (collision[i].gameObject.tag == "Ice_Character" || collision[i].gameObject.tag == "Jungle_Character")
					{
						collision[i].gameObject.GetComponent<PlayerController>().movement.y = verticalKnockBackPower;
						collision[i].gameObject.GetComponent<PlayerController>().movement.x = collision[i].gameObject.GetComponent<PlayerController>().abilityDirection * horizontalKnockBackPower;
						collision[i].gameObject.GetComponent<PlayerInput>().isStunned = true;
					}
				}
			}
		}
	}

	public override void Update()
	{
		base.Update();

		PlayerPassiveAbility();
		PlayerActiveAbility();
	}
}