using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spike : FlyingTrajectory
{
    public float timeBeforeSpikeFallsMin;
    public float timeBeforeSpikeFallsMax;

    private float timeBeforeSpikeFalls;
    private SpikeSpawner spikeSpawner;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = speed;
        rb2d.sleepMode = RigidbodySleepMode2D.StartAsleep;
    }
    void Start () {
        if (transform.parent != null)
        {
            spikeSpawner = GetComponentInParent<SpikeSpawner>();
            timeBeforeSpikeFalls = spikeSpawner.GetFallTime();
        }
        else
        {
            timeBeforeSpikeFalls = Random.Range(timeBeforeSpikeFallsMin, timeBeforeSpikeFallsMax);
        }
        speed = Mathf.Clamp(speed, 0, Mathf.Infinity);
        timeBeforeSpikeFalls = Mathf.Clamp(timeBeforeSpikeFalls, 0, Mathf.Infinity);
	}
	
    void WakeUp()
    {
        startFalling += Time.deltaTime;
        if (startFalling > timeBeforeSpikeFalls)
        {
            rb2d.WakeUp();
        }
    }
	void FixedUpdate () {
        WakeUp();
	}
}

