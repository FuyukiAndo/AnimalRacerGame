using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : Spawner {

    // Update is called once per frame
    public bool wantToPopOnDestination;
    public Vector2 travelToFromStart;
    public float popTime;

    private Vector2 travelPosition;
    [Tooltip("TravelTime should be higher than speed. The math formula for the time it takes the bubble to land on it's position is travelTime/speed")]
    public float travelTime;
    [Tooltip("Speed should be lower than travel time. The math formula for the time it takes the bubble to land on it's position is travelTime/speed")]
    private bool onlyChild = true;

    void Update() {
        SpawnObject();
        spawnAfter(onlyChild);
	}
    public Vector2 getTravelPosition
    {
        get { return travelPosition = new Vector2(transform.position.x + travelToFromStart.x, transform.position.y + travelToFromStart.y); }
    }
    public float getTravelPositionX
    {
        get { return travelToFromStart.y; }

    }
    public float getTravelPositionY
    {
        get { return travelToFromStart.x; }
    }
    public float getPopTime
    {
        get { return popTime; }
    }
    public bool getPopDesition
    {
        get { return wantToPopOnDestination; }
    }
    public float getDestinationTime
    {
        get { return travelTime; }
    }
}
