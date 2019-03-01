﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSnowballDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TerrainSnowball terrainSnowball = collision.gameObject.GetComponent<TerrainSnowball>();
        if (terrainSnowball)
        {
            Destroy(collision.gameObject);
        }
    }
}
