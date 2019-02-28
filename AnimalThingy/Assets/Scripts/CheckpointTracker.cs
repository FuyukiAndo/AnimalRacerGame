using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
	[SerializeField] private Vector2 boxSize = new Vector2(1.0f,1.0f), checkpointSearchSize;
	[SerializeField] private float initialCheckpointTipDelay, recurringCheckpointTipDelay;
	[SerializeField] private string UIMethodName;
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
	[SerializeField] private Color playerColour;
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
	void Update()
	{
		Collider2D collider = Physics2D.OverlapBox(transform.position, boxSize, 0f);
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