using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformSpawner : Spawner 
{
	public float floatSpeed;
    public int durability = 2;
	public int spawnDirection;

	void OnValidate()
	{
		if(spawnDirection > 1)
		{
			spawnDirection = 1;
		}
		else if(spawnDirection < -1)
		{
			spawnDirection = -1;
		}
	}

    void Update() 
	{
        SpawnObject();
	}
	
    public float GetSpeed()
    {
        return floatSpeed;
    }
	
    public int GetDurability()
    {
        return durability;
    }
	
	public int GetDirection()
	{
		return spawnDirection;
	}
}
