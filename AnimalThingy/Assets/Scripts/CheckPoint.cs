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
	private List<Color> activatedColours = new List<Color>();

	public void SetColour(Color colour)
	{
		activatedColours.Add(colour);
	}

	public List<Color> GetActivatedColours()
	{
		return activatedColours;
	}
}