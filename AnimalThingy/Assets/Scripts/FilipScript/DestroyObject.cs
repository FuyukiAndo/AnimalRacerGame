using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour 
{
	public float untilDestroy = 3.0f;
	
	
	private void Update()
	{
		untilDestroy -= Time.deltaTime;
		
		if(untilDestroy < 0)
		{
			Destroy(this.gameObject);
		}
	}
}
