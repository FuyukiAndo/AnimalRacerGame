using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpikeSpawner : MonoBehaviour {

    public GameObject spike;
    public float timeBetweenSpawns;
    public bool wantSpikeToDisappearFirst;

    private float spawnClock;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(spawnClock > timeBetweenSpawns)
        {
            Instantiate(spike, transform.position, new Quaternion(0,0,0,0), gameObject.transform);
            spawnClock = 0;
        }
        if(transform.childCount > 0 && wantSpikeToDisappearFirst == true)
        {
            spawnClock = 0;
        }
        else
        {
            spawnClock += Time.deltaTime;
        }
	}
}
