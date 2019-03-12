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
	[SerializeField] private AudioEffectController effectController;
	[SerializeField] private float searchRadius = 1f;
	[SerializeField] private Vector2 collisionDetectionOffset;

	private bool updatedCheckToGoFor;

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
		if (GetComponent<AudioEffectController>() && effectController == null)
		{
			effectController = GetComponent<AudioEffectController>();
		}
	}

	void Update()
	{
		Collider2D collider = Physics2D.OverlapCircle((Vector2)transform.position + collisionDetectionOffset, searchRadius);
		if (playerFlags.Length <= 0)return;
		for (int i = 0; i < playerFlags.Length; i++)
		{
			if (playerFlags[i].playerFlag == null)
			{
				continue;
			}
			if (collider.name == playerFlags[i].playerName)
			{
				if (effectController != null)
				{
					
				}
				playerFlags[i].playerFlag.SetActive(true);
				if (!updatedCheckToGoFor)
				{
					SetNextCheckPosToGoFor();
					updatedCheckToGoFor = true;
				}
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
		Gizmos.DrawWireSphere((Vector2)transform.position + collisionDetectionOffset, searchRadius);
	}

	void SetNextCheckPosToGoFor()
	{
		if (GPSCheckpoint.Instance != null)
		{
			GPSCheckpoint.Instance.UpdateCheckpointToGo();
		}
	}
}

[System.Serializable]
public class PlayerFlag
{
	public GameObject playerFlag;
	public string playerName;
}