using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpikeSpawner : Spawner{

    public bool wantSpikeToDisappearFirst;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        SpawnObject(transform,spawnObject);
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
