using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour {



    public float speed;
    public int JumpStrenght;
    public float distanceBeforeFlight = 100;
    public float glidGravity = 0.1f;
    public int hp = 3;
    
    public LayerMask terrainLayer, spikeLayer, waterLayer;
    public float currentDistanceToFlight;
    public bool pengvin;


    private Rigidbody2D rb2d;
    private BoxCollider2D bc2d;
    private Vector2 player;
    private float distanceToGround;
    private bool isGrounded;
    private float originalSpeed;
    private int jumpCount;
    private bool isStunned;

    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        currentDistanceToFlight = 0;
        distanceToGround = bc2d.bounds.extents.y;
        originalSpeed = speed;
	}
	void FixedUpdate () {
        Debug.DrawRay(transform.position, Vector3.down * (distanceToGround + 0.1f), Color.red);
        ifGrouded();
        HorizontalMovement();
        VerticalMovement();
    }


    void ifGrouded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround + 0.1f, terrainLayer);
        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    void HorizontalMovement()
    {
        if (isStunned == false)
        {
            if (Input.GetKey("a") && isGrounded == true)
            {
                player = new Vector2(-speed, rb2d.velocity.y);
                currentDistanceToFlight += rb2d.velocity.x;
            }
            else if (Input.GetKey("d") && isGrounded == true)
            {
                player = new Vector2(speed, rb2d.velocity.y);
                currentDistanceToFlight += rb2d.velocity.x;
            }
            else if (Input.GetKey("a") && isGrounded == false)
            {
                player = new Vector2(-speed, rb2d.velocity.y);
            }
            else if (Input.GetKey("d") && isGrounded == false)
            {
                player = new Vector2(speed, rb2d.velocity.y);
            }
            else
            {
                player = new Vector2(0, rb2d.velocity.y);
                currentDistanceToFlight = 0;
            }
            rb2d.velocity = player;
        }
    }
    void VerticalMovement()
    {
        //if (currentDistanceToFlight > distanceBeforeFlight || currentDistanceToFlight < -distanceBeforeFlight)
        //{
        if (isStunned == false)
        {
            if (Input.GetKey("w") && isGrounded == true && jumpCount == 0)
            {
                rb2d.AddForce(new Vector2(0, JumpStrenght));
                currentDistanceToFlight = 0;
                jumpCount++;
            }
            else if (isGrounded == true)
            {
                jumpCount = 0;
            }
        }        //}
        //if(Input.GetKey("w") && isGrounded == false && rb2d.velocity.y < 0)
        //{
        //    rb2d.velocity = new Vector2(rb2d.velocity.x, -glidGravity);
        //}
    }
    public IEnumerator SpeedChange(float boostChangeAmount, float boostDuration, GameObject speedObject)
    {
        speed = speed + boostChangeAmount;
        yield return new WaitForSeconds(boostDuration);
        speed = originalSpeed;
        Destroy(speedObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isOnLayer = waterLayer == (waterLayer | (1 << collision.gameObject.layer));
        /*Mathf.Log(waterLayer.value, 2) == collision.gameObject.layer*/
        /*(waterLayer | (1 << collision.gameObject.layer)) - Kollar bitmaskens position på collisionen och jämför den med bitmasken*/

        if ( isOnLayer && pengvin == false)
        {
            GetKilled();
        }
    }
    public IEnumerator GetStunned(float stunDuration, GameObject stunObject)
    {
        isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
        Destroy(stunObject);
        
    }
    public void GetKilled()
    {
        Destroy(gameObject);
    }

}