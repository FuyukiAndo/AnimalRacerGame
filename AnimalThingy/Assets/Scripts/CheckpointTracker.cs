using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
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

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Checkpoint>())
		{
			Checkpoint checkPoint = other.GetComponent<Checkpoint>();
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