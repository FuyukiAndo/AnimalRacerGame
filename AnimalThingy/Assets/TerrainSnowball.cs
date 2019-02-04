using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSnowball : MonoBehaviour {

    public float speed = 5.0f;
    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Vector2 movement = new Vector2(1, 0);

        rb2d.AddForce(movement * speed);

	}
}
