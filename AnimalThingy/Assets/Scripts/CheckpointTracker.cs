using System.Collections;
using System.Collections.Generic;
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
	[SerializeField] private Vector2 boxSize;

	void Update()
	{
		Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);
		foreach (var collider in colliders)
		{
			if (collider.GetComponent<Checkpoint>())
			{
				print("check");
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

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, boxSize);
	}
}