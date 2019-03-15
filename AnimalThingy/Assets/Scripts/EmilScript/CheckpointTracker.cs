using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CheckpointTracker : MonoBehaviour
{
	public List<int> CheckpointsPassed
	{
		get
		{
			return checkPointsPassed;
		}
	}
	private List<int> checkPointsPassed = new List<int>();
	private int lastCheckpointPassed = 0;
	[SerializeField] private Vector2 boxOffset;
	[SerializeField] private LayerMask checkpointLayer;
	[SerializeField] private FMODAudio checkpointSFX;
	[SerializeField] private AudioEffectController effectController;
	private Vector2 boxSize;
	public float FinishingTime
	{
		get
		{
			return finishingTime;
		}
		set
		{
			finishingTime = value;
		}
	}
	private float finishingTime;
	public int PlacementPoint
	{
		get
		{
			return placementPoint;
		}
		set
		{
			placementPoint = value;
		}
	}
	private int placementPoint;

	void Start()
	{
		if (!GetComponent<BoxCollider2D>())
		{
			gameObject.AddComponent<BoxCollider2D>();
		}
		boxSize = GetComponent<BoxCollider2D>().size;
	}

	void Update()
	{
		Collider2D collider = Physics2D.OverlapBox((Vector2)transform.position + boxOffset, boxSize, 0f, checkpointLayer);
		if (collider.GetComponent<Checkpoint>())
		{
			Checkpoint checkPoint = collider.GetComponent<Checkpoint>();
			if (GoalManager.Instance.passInSequence)
			{
				if (checkPoint.Index == lastCheckpointPassed + 1)
				{
					checkPointsPassed.Add(checkPoint.Index);
					lastCheckpointPassed = checkPoint.Index;
					return;
				}
			}
			else
			{
				if (checkPointsPassed.Count > 0)
				{
					foreach (var index in checkPointsPassed)
					{
						if (index == checkPoint.Index)
						{
							return;
						}
					}
				}
				if (effectController != null)
				{
					effectController.PlayAudioOneShot();
				}
				checkPointsPassed.Add(checkPoint.Index);
			}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube((Vector2)transform.position + boxOffset, boxSize);
	}

	public Vector2 GetCurrentPosition()
	{
		return transform.position;
	}

	public int GetCurrentCheckpointIndex()
	{
		return checkPointsPassed[checkPointsPassed.Count - 1];
	}
}