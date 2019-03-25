using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTrampoline : MonoBehaviour 
{
    public float bounceHeight;
    public LayerMask characterLayer;

    private Collider2D c2d;
    private Collider2D[] colliders;
    private int newColliderCount;

    void Start () 
	{
        c2d = GetComponent<Collider2D>();
	}

	void Update () 
	{
        CollisionCheck();
	}

    private void CollisionCheck()
    {
        var oldColliderCount = newColliderCount;

        colliders = Physics2D.OverlapBoxAll(transform.position, c2d.bounds.size, 0.0f);

        newColliderCount = colliders.Length;

        if (newColliderCount > oldColliderCount)
        {
            foreach (var collision in colliders)
            {
                bool isOnLayer = characterLayer == (characterLayer | (1 << collision.gameObject.layer));

                if (isOnLayer)
                {
                    collision.GetComponent<PlayerController>().movement.y = bounceHeight;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, c2d.bounds.size);
    }
}
