using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleSpawner : Spawner
{

    public float needleSpeed;

    void Update()
    {
        SpawnObject();
    }

    public float GetNeedleSpeed()
    {
        return needleSpeed;
    }
}
