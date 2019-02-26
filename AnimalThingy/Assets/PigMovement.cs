using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigMovement : MonoBehaviour {

    public string leftKey = "A";
    public string rightKey = "D";
    public string jumpKey = "W";
    public string abillityKey = "G";
    public float movementSpeed = 60f;
    public float midAirTurningForce = 100f;
    public float jumpForce = 10f;
    public float bounceForce = 1f;
    public float downForce = 10f;
    public LayerMask groundLayer;
    public PhysicMaterial bouncyMaterial;
    public PhysicMaterial pigMaterial;
    public float groundDetectionRayLength = 1f;
    private Rigidbody rb;
    private Vector2 movement;
    public bool isGrounded = false;

    private void Start()
    {
        movement = new Vector2(0, 0);
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
        HorizontalMovement();
        Jump();
        Bounce();
	}

    void HorizontalMovement()
    {
        if (Input.GetKey(leftKey) && isGrounded == true)
        {

            movement = new Vector2(-movementSpeed, rb.velocity.y);
            rb.velocity = movement;
        }
        if(Input.GetKey(leftKey) && isGrounded == false && rb.velocity.x > -movementSpeed)
        {
            rb.AddForce(new Vector3(-midAirTurningForce, 0, 0));
        }

        if (Input.GetKey(rightKey) && isGrounded == true)
        {
            movement = new Vector2(movementSpeed, rb.velocity.y);
            rb.velocity = movement;
        }
        if(Input.GetKey(rightKey) && isGrounded == false && rb.velocity.x < movementSpeed)
        {
            rb.AddForce(new Vector3(midAirTurningForce, 0, 0));
        }
    }

    void Jump()
    {
        Vector3 RayDirection = new Vector3(0, -1,0);
        isGrounded = Physics.Raycast(transform.position, RayDirection , groundDetectionRayLength,
            groundLayer);
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundDetectionRayLength, transform.position.z), Color.green);
        if (isGrounded)
        {
            if (Input.GetKeyDown(jumpKey))
            {
                movement = new Vector2(rb.velocity.x, jumpForce);
                rb.velocity = movement;
            }
        }
    }
    void Bounce()
    {
        if (Input.GetKey(abillityKey))
        {
            GetComponent<BoxCollider>().material = bouncyMaterial;
        }
        else
            GetComponent<BoxCollider>().material = pigMaterial;
    }
}
