using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : FlyingTrajectory
{
    public float timeBeforeSpikeFalls = 5;
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = speed;
        rb2d.Sleep();

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
