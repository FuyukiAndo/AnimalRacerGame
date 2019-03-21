using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
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
	[SerializeField] private LayerMask playerLayer;
	[SerializeField] private CircleCollider2D circle;
	[SerializeField] private AudioOneshotPlayer oneshotPlayer;

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
		circle = GetComponent<CircleCollider2D>();
		oneshotPlayer = GetComponent<AudioOneshotPlayer>();
	}

	void Update()
	{
		if (Physics2D.OverlapCircle((Vector2)transform.position + circle.offset, circle.radius, playerLayer))
		{
			Collider2D collider = Physics2D.OverlapCircle((Vector2)transform.position + circle.offset, circle.radius, playerLayer);
			if (playerFlags.Length <= 0)return;
			for (int i = 0; i < playerFlags.Length; i++)
			{
				if (playerFlags[i].playerFlag == null)
				{
					continue;
				}
				if (collider.name == playerFlags[i].playerName)
				{
					playerFlags[i].playerFlag.SetActive(true);
					if (!playerFlags[i].playedAudioForPlayer)
					{
						float paramValue = 0f;
						switch (collider.name)
						{
							case "Player1":
								paramValue = Random.Range(0.0f, 0.05f);
								break;
							case "Player2":
								paramValue = Random.Range(0.1f, 0.15f);
								break;
							case "Player3":
								paramValue = Random.Range(0.2f, 0.25f);
								break;
							case "Player4":
								paramValue = Random.Range(0.3f, 0.35f);
								break;
						}
						oneshotPlayer.PlayAudioOneShot();
						oneshotPlayer.SetParameterValue(paramValue);
						playerFlags[i].playedAudioForPlayer = true;
					}
					if (!updatedCheckToGoFor)
					{
						SetNextCheckPosToGoFor();
						updatedCheckToGoFor = true;
					}
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
		Gizmos.DrawWireSphere((Vector2)transform.position + circle.offset, circle.radius);
	}

	public void SetNextCheckPosToGoFor()
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
	[HideInInspector] public bool playedAudioForPlayer = false;
}