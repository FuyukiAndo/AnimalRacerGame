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
	[SerializeField] private LayerMask checkpointLayer;
	[SerializeField] private FMODAudio checkpointSFX;
	[SerializeField] private BoxCollider2D box;
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
	private PlayerInput input;

	void Start()
	{
		if (!GetComponent<BoxCollider2D>())
		{
			gameObject.AddComponent<BoxCollider2D>();
		}
		box = GetComponent<BoxCollider2D>();
		input = GetComponent<PlayerInput>();
	}

	void Update()
	{
		if (Physics2D.OverlapBox((Vector2)transform.position + box.offset, box.size, 0f, checkpointLayer))
		{
			Collider2D collider = Physics2D.OverlapBox((Vector2)transform.position + box.offset, box.size, 0f, checkpointLayer);
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
					checkPointsPassed.Add(checkPoint.Index);
					//GoalManager.Instance.NotifyOfCheckpointCount(this);
				}
			}
		}
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