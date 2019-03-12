using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : Spawner{

    public bool wantSpikeToDisappearFirst;
    public float timeBeforeSpikeFallsMin;
    public float timeBeforeSpikeFallsMax;
    

    void Update () {
        spawnAfter(wantSpikeToDisappearFirst);
        SpawnObject();

	}
    public float GetFallTime()
    {
        return Random.Range(timeBeforeSpikeFallsMin, timeBeforeSpikeFallsMax);
    }
}
