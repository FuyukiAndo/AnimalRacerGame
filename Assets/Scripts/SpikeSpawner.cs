using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : Spawner{

    public bool wantSpikeToDisappearFirst;

    void Update () {
        SpawnObject();
        spawnAfter(wantSpikeToDisappearFirst);
	}
}
