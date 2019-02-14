using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsflakSpawner : Spawner {

    public float floatSpeed;

    void Update () {
        SpawnObject();
	}
    public float GetSpeed()
    {
        return floatSpeed;
    }
}
