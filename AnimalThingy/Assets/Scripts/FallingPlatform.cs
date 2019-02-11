using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour {
    [Header("Enviorment Interaction")]
    public LayerMask terrainLayer;
    [Header("Player Interaction")]
    public float stunDuration;

    private Rigidbody2D rb2d;
    private BoxCollider2D bc2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();   
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            bc2d.isTrigger = true;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        bool isOnTerrainLayer = terrainLayer == (terrainLayer | (1 << collision.gameObject.layer));
        if (isOnTerrainLayer)
        {
            Destroy(gameObject);
        }
        else if (player != null)
        {
            StartCoroutine(player.GetStunned(stunDuration, gameObject));
        }
    }
}
