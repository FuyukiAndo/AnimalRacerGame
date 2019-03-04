using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	public int Index
	{
		get
		{
			return index;
		}
	}
	[SerializeField] private int index;
	[SerializeField] private PlayerFlag[] playerFlags;
	[SerializeField] private AnimationHandler animationHandler;
	[SerializeField] private string animationTrigger;
	[SerializeField] private AudioEffectController effectController;
	[SerializeField] private float searchRadius = 1f;

	void Start()
	{
		if (playerFlags.Length > 0)
		{
			foreach (var flag in playerFlags)
			{
				if (flag.playerFlag != null)
				{
					flag.playerFlag.SetActive(false);
				}
			}
		}
		if (GetComponentInChildren<AnimationHandler>() && animationHandler == null)
		{
			animationHandler = GetComponentInChildren<AnimationHandler>();
		}
		if (GetComponent<AudioEffectController>() && effectController == null)
		{
			effectController = GetComponent<AudioEffectController>();
		}
	}

	void Update()
	{
		Collider2D collider = Physics2D.OverlapCircle(transform.position, searchRadius);
		if (playerFlags.Length <= 0) return;
		for (int i = 0; i < playerFlags.Length; i++)
		{
			if (playerFlags[i].playerFlag == null)
			{
				continue;
			}
			if (collider.tag == playerFlags[i].playerTag || collider.GetComponent(playerFlags[i].playerControllerScript))
			{
				animationHandler.SetAnimatorTrigger(animationTrigger);
				playerFlags[i].playerFlag.SetActive(true);
			}
		}
	}

	public void SetFlagCount(int count)
	{
		playerFlags = new PlayerFlag[count];
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, searchRadius);
	}
}

[System.Serializable]
public class PlayerFlag
{
	public GameObject playerFlag;
	public string playerTag;
	[Tooltip("PlayerController Script Name")] public string playerControllerScript;
}