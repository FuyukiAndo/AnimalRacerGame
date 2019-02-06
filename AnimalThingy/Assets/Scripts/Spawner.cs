using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner{

    public GameObject spawnObject;
    public float timeBetweenSpawns;

    protected float spawnClock;
    
    public void SpawnObject(Transform transform, GameObject gameObject)
    {
        if (spawnClock > timeBetweenSpawns)
        {

            UnityEngine.Object.Instantiate(spawnObject, transform.position, new Quaternion(0, 0, 0, 0), gameObject.transform);
            spawnClock = 0;
        }
    }
}
