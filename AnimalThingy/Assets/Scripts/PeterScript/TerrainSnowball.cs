using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TerrainSnowball : MonoBehaviour {

    public float speed = 5.0f;
    public LayerMask terrainLayer, characterLayer;
    public float pushForce = 5.0f;
    public float stunDuration;

    private Rigidbody2D rb2d;
    private Collider2D c2d;
    private Collider2D[] collision;
    private float dir;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        c2d = GetComponent<Collider2D>();
        if(speed > 0)
        {
            dir = gameObject.transform.lossyScale.x;
        }
        else
        {
            dir = -gameObject.transform.lossyScale.x;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Debug.DrawRay(transform.position,Vector2.right * (dir), Color.red);
        HitWall();
        HitPlayer();
        Vector2 movement = new Vector2(speed, 0);
        rb2d.AddForce(movement);
	}
    public void HitPlayer()
    {
        collision = Physics2D.OverlapBoxAll(transform.position, c2d.bounds.size, 0.0f);
        foreach(var collider in collision)
        {
            bool isOnLayer = characterLayer == (characterLayer | (1 << collider.gameObject.layer));
            if (isOnLayer)
            {
                collider.GetComponent<PlayerInput>().stunDurationLeft = stunDuration;
                if(transform.position.x > collider.transform.position.x)
                {
                    collider.GetComponent<PlayerController>().movement.y += pushForce;
                    collider.GetComponent<PlayerController>().movement.x += rb2d.velocity.x * pushForce;
                    Debug.Log("elloh");
                }
                else
                {
                    Debug.Log("hello");
                    collider.GetComponent<PlayerController>().movement.y += pushForce;
                    collider.GetComponent<PlayerController>().movement.x -= rb2d.velocity.x * pushForce;
                }
            }
        }
    }
    public void HitWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, dir, terrainLayer);
        if(hit.collider != null)
        {
            dir = -dir;
            speed = -speed;
        }
    }
}
