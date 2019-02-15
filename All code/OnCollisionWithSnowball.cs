using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OnCollisionWithSnowball : MonoBehaviour {
    [SerializeField]
    private float force;
    [SerializeField]
    private float stunDuration;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (!collision.gameObject.CompareTag("Ball"))
            return;
        Vector2 dir = transform.position - collision.transform.position;
        dir.Normalize();
        StartCoroutine(GetComponent<TemporaryCodeDump>().GetStunned(stunDuration));

        //GetComponent<PlayerController>().AddForce(dir*force, ForceMode2D.Impulse);

        GetComponent<PlayerController>().MovePlayer(dir * force);
    }
}
