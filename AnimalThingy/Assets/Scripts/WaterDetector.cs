using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D hit)
    {
        var Hit = hit.GetComponent<Rigidbody2D>();
        if (Hit != null)
        {
            transform.parent.GetComponent<Water>().Splash(transform.position.x, Hit.velocity.y * Hit.mass / 40f);
        }
    }
}
