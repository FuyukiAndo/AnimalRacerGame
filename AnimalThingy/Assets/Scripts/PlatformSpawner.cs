using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
CODE UPDATED BY FILIP ANTONIJEVIC 2019-02-24
--------------------------------------------
* Added 'GetDirection()' function and Vector2 variable 'spawnDirection'
--------------------------------------------
**/

public class PlatformSpawner : Spawner 
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
