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
	[SerializeField] private Vector2 boxSize, checkpointSearchSize;
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
		Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);
		foreach (var collider in colliders)
		{
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
	}

	public Vector2 GetCurrentPosition()
	{
		return transform.position;
	}

	public int GetCurrentCheckpointIndex()
	{
		return checkPointsPassed[checkPointsPassed.Count - 1];
	}

	IEnumerator FindNearestCheckpointNotTaken()
	{
		yield return new WaitForSeconds(initialCheckpointTipDelay);
		while (true)
		{
			BroadcastMessage(UIMethodName, GetNearestNotTakenCheckpoint(), SendMessageOptions.DontRequireReceiver);
			yield return new WaitForSeconds(recurringCheckpointTipDelay);
		}

	}

	Checkpoint GetNearestNotTakenCheckpoint()
	{
		Checkpoint point1 = null, point2 = null, closest = null;
		//Find all colliders
		Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, checkpointSearchSize, 0f);
		List<Checkpoint> checkpoints = new List<Checkpoint>();
		//Find all checkpoints
		foreach (var collider in colliders)
		{
			if (collider.GetComponent<Checkpoint>())
			{
				checkpoints.Add(collider.GetComponent<Checkpoint>());
			}
		}
		//Remove the checkpoints the player has already passed
		for (int i = 0; i < checkPointsPassed.Count; i++)
		{
			if (checkpoints[i].Index == checkPointsPassed[i])
			{
				checkpoints.RemoveAt(i);
			}
		}
		//Check the distance & set the closest checkpoint
		for (int i = 0; i < checkpoints.Count; i++)
		{
			point1 = checkpoints[i];
			point2 = checkpoints[i + 1];
			if ((point1.transform.position - transform.position).magnitude < 
			(point2.transform.position - transform.position).magnitude)
			{
				closest = point1;
			}
			else
			{
				closest = point2;
			}
		}
		return closest;
	}

	//Check every checkpoint, set colour for the ones who the player has passed, leave the ones who the player hasn't passed uncoloured
}