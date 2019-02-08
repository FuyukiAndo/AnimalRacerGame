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
    
    public LayerMask groundLayer, spikeLayer;
    public float currentDistanceToFlight;
    public bool pengvin;


    private Rigidbody2D rb2d;
    private BoxCollider2D bc2d;
    private Vector2 player;
    private float distanceToGround;
    private bool isGrounded;
    private float originalSpeed;
    private IEnumerable coroutine;

    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        currentDistanceToFlight = 0;
        distanceToGround = bc2d.bounds.extents.y;
        originalSpeed = speed;
	}
	void FixedUpdate () {
        Debug.DrawRay(transform.position, Vector3.up * (distanceToGround + 1f), Color.red);
        ifGrouded();
        HorizontalMovement();
        VerticalMovement();
        Debug.Log(1 / Time.deltaTime);
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
    void OnCollisionEnter2D(Collision2D collision)
    {
      
        if(collision.gameObject.layer == 9)
        {
            getStunned();
        }
    }
    //void SpeedChange(float boostChange)
    //{
    //    Debug.Log("hello");
    //    speed = speed + boostChange;
    //    StartCoroutine(TimeDelay());
    //}
    //private IEnumerator TimeDelay()
    //{
    //    yield return new WaitForSeconds(5);
    //    speed = originalSpeed;
    //}
    void hello(float da)
    {
        Debug.Log(da);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4 && pengvin == false)
        {
            getKilled();
        }
    }
    public void getStunned()
    {
        Debug.Log("Stunned");
    }
    public void getKilled()
    {
        Destroy(gameObject);
    }

}