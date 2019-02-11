using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needles : FlyingTrajectory
{
    public float distanceFromStart;
    private Vector2 startPosition;
   void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        rb2d.useFullKinematicContacts = true;
        MoveNeedle();
        startPosition = transform.position; 
    }

    void MoveNeedle()
    {
        rb2d.velocity = new Vector2(speed, 0);
    }



}
