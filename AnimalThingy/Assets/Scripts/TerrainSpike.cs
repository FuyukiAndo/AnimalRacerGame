using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpike : MonoBehaviour {

    public float timeBeforeSpikeFalls = 5;
    public float fallSpeed;

    private Rigidbody2D rb2d;
    private float startFalling;
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = fallSpeed;
        rb2d.Sleep();

        fallSpeed = Mathf.Clamp(fallSpeed, 0, Mathf.Infinity);
        timeBeforeSpikeFalls = Mathf.Clamp(timeBeforeSpikeFalls, 0, Mathf.Infinity);
	}
	
	// Update is called once per frame
	void Update () {
         startFalling += Time.deltaTime;
		if(startFalling > timeBeforeSpikeFalls)
        {
            rb2d.WakeUp();
        }
	}
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
