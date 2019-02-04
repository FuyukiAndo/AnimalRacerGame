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
}