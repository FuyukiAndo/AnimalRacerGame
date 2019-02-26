using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsflakSpawner : Spawner {

    public float floatSpeed;
    public int durability = 2;

    void Update () {
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
}
