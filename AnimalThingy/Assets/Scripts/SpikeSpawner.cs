using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : Spawner{

    public bool wantSpikeToDisappearFirst;
    public float timeBeforeSpikeFallsMin = 0;
    public float timeBeforeSpikeFallsMax = 5;
    

    void Update () {
        SpawnObject();
        spawnAfter(wantSpikeToDisappearFirst);
	}
    public float GetFallTime()
    {
        return Random.Range(timeBeforeSpikeFallsMin, timeBeforeSpikeFallsMax);
    }
}
