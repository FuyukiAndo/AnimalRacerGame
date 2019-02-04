using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {



    public int speed;
    public int JumpStrenght;
    public float distanceBeforeFlight = 100;
    public float glidGravity = 0.1f;
    
    public LayerMask groundLayer, spikeLayer;


    private Rigidbody2D rb2d;
    private BoxCollider2D bc2d;
    private Vector2 player;
    private float distanceToGround;
    private bool isGrounded;
    public float currentDistanceToFlight;




    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        currentDistanceToFlight = 0;
        distanceToGround = bc2d.bounds.extents.y;
        
	}
	void FixedUpdate () {
        Debug.DrawRay(transform.position, Vector3.up * (distanceToGround + 1f), Color.red);
        ifGrouded();
        hitSpike();
        HorizontalMovement();
        VerticalMovement();
    }
    void ifGrouded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround + 1f, groundLayer);
        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    void hitSpike()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, distanceToGround + 1f, spikeLayer);
        if(hit.collider != null)
        {
            Debug.Log("Stunned");
        }
    }
    void HorizontalMovement()
    {
        if (Input.GetKey("a") && isGrounded == true)
        {
            player = new Vector2(-speed, rb2d.velocity.y);
            rb2d.velocity = player;
            currentDistanceToFlight += rb2d.velocity.x;
        }
        else if (Input.GetKey("d") && isGrounded == true)
        {
            player = new Vector2(speed, rb2d.velocity.y);
            rb2d.velocity = player;
            currentDistanceToFlight += rb2d.velocity.x;
        }
        else if (Input.GetKey("a") && isGrounded == false)
        {
            player = new Vector2(-speed, rb2d.velocity.y);
            rb2d.velocity = player;
        }
        else if (Input.GetKey("d") && isGrounded == false)
        {
            player = new Vector2(speed, rb2d.velocity.y);
            rb2d.velocity = player;
        }
        else
        {
            player = new Vector2(0, rb2d.velocity.y);
            rb2d.velocity = player;
            currentDistanceToFlight = 0;
        }
    }
    void VerticalMovement()
    {
        //if (currentDistanceToFlight > distanceBeforeFlight || currentDistanceToFlight < -distanceBeforeFlight)
        //{
            if (Input.GetKey("w") && isGrounded == true)
            {
                rb2d.AddForce(new Vector2(0, JumpStrenght));
                currentDistanceToFlight = 0;    
            }
        //}
        //if(Input.GetKey("w") && isGrounded == false && rb2d.velocity.y < 0)
        //{
        //    rb2d.velocity = new Vector2(rb2d.velocity.x, -glidGravity);
        //}
    }

}