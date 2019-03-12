using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnObject;
    public bool wantSpawnerToBeRemoved;
    public float timeBetweenSpawnsMin = 1;
    public float timeBetweenSpawnsMax = 3;
    protected float timeBetweenSpawns;
    protected float spawnClock;
    protected bool ifTimed;

    private void Awake()
    {
        if (wantSpawnerToBeRemoved == true)
        {
            if (GetComponent<MeshRenderer>() != null)
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
            if (GetComponent<Collider2D>() != null)
            {
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
    protected void Start()
    {
        timeBetweenSpawnsMin = Mathf.Clamp(timeBetweenSpawnsMin, 0, timeBetweenSpawnsMin);
        timeBetweenSpawnsMax = Mathf.Clamp(timeBetweenSpawnsMax, timeBetweenSpawnsMin, timeBetweenSpawnsMax);
    }
	
    protected void SpawnObject()
    {
        if (ifTimed == false)
        {
            timeBetweenSpawns = Random.Range(timeBetweenSpawnsMin, timeBetweenSpawnsMax);
            ifTimed = true;
        }
        if (spawnClock > timeBetweenSpawns)
        {
            
            Instantiate(spawnObject, transform.position, new Quaternion(0, 0, 0, 0), gameObject.transform);
            spawnClock = 0;
            ifTimed = false;
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
