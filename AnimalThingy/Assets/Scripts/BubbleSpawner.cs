using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : Spawner {

    // Update is called once per frame
    public Vector2 travelToFromStart;
    public float popTime;

    private Vector2 travelPosition;
    private bool onlyChild = true;
    void Update () {
        SpawnObject();
        spawnAfter(onlyChild);
	}

    public Vector2 GetTravelPosition()
    {
     return travelPosition = new Vector2(transform.position.x + travelToFromStart.x, transform.position.y + travelToFromStart.y);
    }
    public float GetPopTime()
    {
        return popTime;
    }
}
