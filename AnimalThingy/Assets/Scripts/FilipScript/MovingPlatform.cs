using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
[RequireComponent(typeof(CollisionController))]
//[RequireComponent(typeof(GravityController))]
public class MovingPlatform : MonoBehaviour
{
    [HideInInspector] public CollisionController collisionController;
    //[HideInInspector] public GravityController gravityController;
    [HideInInspector] public PlatformController platformController;

    [Header("Platform Gravity Settings")]
    [Tooltip("Max jump height value between 0.1f and x")]
    public float maxVelocity = 8.0f;
    [Tooltip("Min jump height value between 0.1f and 2.0f")]
    [Range(0.1f, 2.0f)] public float fallDelay = 0.4f;

    [HideInInspector] public float gravity;

    public float timeUntilBroken;
    public int platformDurability;
    public float destroyDelay;
    //public float floatSpeed;
    public LayerMask characterLayer, platformLayer;

    public Vector2 movement;
    private float breakTime;
    //private float defaultSpeed = 1;
    private float movementSpeed;

    private int movementDirection;
    private int timeBeforeDestroyed;
    //private int timeBeforeDestroyed;
    int oldColliderCount;
    int newColliderCount;

    private BoxCollider2D boxCollider;
    private Collider2D[] collision;

    //private Vector2 hitDir;
    private MovingPlatformSpawner movingPlatformSpawner;
	
    void Start()
    {
        collisionController = GetComponent<CollisionController>();
        boxCollider = GetComponent<BoxCollider2D>();
		
        if (transform.parent != null)
        {
            //timeBeforeDestroyed = platformDurability;
            movingPlatformSpawner = GetComponentInParent<MovingPlatformSpawner>();
            movementSpeed = movingPlatformSpawner.GetSpeed();
            platformDurability = movingPlatformSpawner.GetDurability();
            movementDirection = movingPlatformSpawner.GetDirection();
        }
		
    }

    void OnValidate()
    {
        if (maxVelocity < 0)
        {
            maxVelocity = 0.1f;
        }
    }
    void FixedUpdate()
    {
        OnPlatform();
    }

    public void UpdateGravity()
    {
        gravity = -(2 * maxVelocity) / Mathf.Pow(fallDelay, 2);
    }
    void Update()
    {
        //Updates gravity every frame
        UpdateGravity();
        movement.x = movementDirection;

        //Add gravity to y-axis of Vector2 'movement'
        float verticalTranslate = gravity * Time.deltaTime;
        movement.y += verticalTranslate;

        //Platform travels a direction with constant speed
        //movement.x = 0;//movementDirection * movementSpeed * Time.deltaTime;

        //MoveObject(movement * Time.deltaTime);

        //If collides with a BoxCollider2D above or below then velocity or movement in y-axis = 0
		
        if (collisionController.boxCollisionDirections.up || collisionController.boxCollisionDirections.down)
        {
            movement.y = 0;
        }

        //DestroyPlatform();
        DestroyPlatform();
    }

    void OnPlatform()
    {
        //looking for player

        oldColliderCount = newColliderCount;

        Vector2 boxSize = boxCollider.size;
        collision = Physics2D.OverlapCircleAll(transform.position, 1.5f);

        newColliderCount = collision.Length;
	
        if (newColliderCount > oldColliderCount)
        {
            for (int i = 0; i < collision.Length; i++)
            {
                //Debug.Log(collision.Length);

                bool isOnCLayer = characterLayer == (characterLayer | (1 << collision[i].gameObject.layer));
                bool isOnPLayer = platformLayer == (platformLayer | (1 << collision[i].gameObject.layer));

                if (isOnCLayer)
                {
                   // timeBeforeDestroyed--;
                    //platformDurability--;
                }

               // if (collision[i].gameObject.tag == "Ground" && collision[i].gameObject != gameObject)
                if(collision[i].gameObject.tag == "Ground" && collision[i].gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
				{
                    //Destroy(gameObject);
                    if (i != 0)//(collision.Length > 2 && i > 1)
                    {
                        Destroy(collision[i].gameObject);
                        Destroy(collision[i - 1].gameObject);
                    }

                    //Destroy(collision[i].gameObject);
                }
               
			   if (isOnPLayer)
                {
                    StartCoroutine(DestroyOnPlatformCollision());
                }
                    if (collision[i].gameObject.tag == "Wall")
                    {
                        //print("platform");
                        movementDirection = -movementDirection;
                    }
            }
        }

        /*if(collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }*/

        //if (collision.gameObject.tag == "Wall")
        //{
        //    speed = -speed;
        //}
        //}
    }
	
    //void whichHitDir()
    //{
    //   /* if(speed > 0)
    //    {
    //        hitDir = Vector2.right;
    //    }
    //    else if(speed < 0)
    //    {
    //        hitDir = Vector2.left;
    //    }
    //    else
    //    {
    //        hitDir = new Vector2(0, 0);
    //    }*/
    //}

	/*void MovePlatform()
	{
		movePlatform = new Vector2(speed, 0);
		transform.Translate(movePlatform * Time.deltaTime);
		rb2d.velocity = movePlatform;
		Debug.Log(breakTime);
	}*/

    IEnumerator DestroyOnPlatformCollision()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
	
    void DestroyPlatform()
    {
        //Debug.Log(timeBeforeDestroyed);

        if (timeBeforeDestroyed <= 0)
        {
            if (platformDurability <= 0)
            {
                breakTime += Time.deltaTime;

                if (breakTime > timeUntilBroken)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}