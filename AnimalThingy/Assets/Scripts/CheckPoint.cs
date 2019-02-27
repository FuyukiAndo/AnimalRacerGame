using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
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
	public PlayerFlag[] playerFlags;

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
	}

	void Update()
	{
		Collider2D collider = Physics2D.OverlapCircle(transform.position, 1f);
		if (playerFlags.Length <= 0) return;
		for (int i = 0; i < playerFlags.Length; i++)
		{
			if (playerFlags[i].playerFlag == null)
			{
				continue;
			}
			if (collider.tag == playerFlags[i].playerTag || collider.GetComponent(playerFlags[i].playerControllerScript))
			{
				playerFlags[i].playerFlag.SetActive(true);
			}
		}
	}

	public void SetFlagCount(int count)
	{
		playerFlags = new PlayerFlag[count];
	}
}

[System.Serializable]
public class PlayerFlag
{
	public GameObject playerFlag;
	public string playerTag;
	[Tooltip("PlayerController Script Name")] public string playerControllerScript;
}