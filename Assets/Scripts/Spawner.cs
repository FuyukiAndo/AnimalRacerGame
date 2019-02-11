using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour{

    public GameObject spawnObject;
    public float timeBetweenSpawns;

    protected float spawnClock;

    protected void Start()
    {
        timeBetweenSpawns = Mathf.Clamp(timeBetweenSpawns, 0, timeBetweenSpawns);
    }
    protected void SpawnObject()
    {
        if (spawnClock > timeBetweenSpawns)
        {

            Instantiate(spawnObject, transform.position, new Quaternion(0, 0, 0, 0), gameObject.transform);
            spawnClock = 0;
        }
        else
        {
            spawnClock += Time.deltaTime;
        }
    }
    protected void spawnAfter(bool first)
    {
        if (transform.childCount > 0 && first == true)
        {
            spawnClock = 0;
        }
    }
}
