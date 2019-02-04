using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSnowball : MonoBehaviour {

    public float speed = 5.0f;
    public LayerMask wallLayer;

    private Rigidbody2D rb2d;
    private float dir;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        dir = gameObject.transform.lossyScale.x;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Debug.DrawRay(transform.position,Vector2.right * (dir), Color.red);
        hitWall();
        Vector2 movement = new Vector2(speed, 0);

        rb2d.AddForce(movement);
	}

    void hitWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, dir, wallLayer);
        if(hit.collider != null)
        {
            dir = -dir;
            speed = -speed;
        }
    }
}
