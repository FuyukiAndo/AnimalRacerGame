using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnTerrainSnowball : Spawner {
    // Update is called once per frame
    public float acceleration;
    public float stunDuration;
    public float force;
    public float maxPushBack;


	void Update () {
        SpawnObject();
	}
    public float getAcc
    {
        get {return acceleration; }
    }
    public float getStunDuration
    {
        get { return stunDuration; }
    }
    public float getForce
    {
        get { return force; }
    }
    public float getMaxPushBack
    {
        get { return maxPushBack; }
    }
}