using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(CollisionController))]
[RequireComponent(typeof(GravityController))]

public class MovingPlatform : MonoBehaviour 
{
	[HideInInspector] public CollisionController collisionController;
    [HideInInspector] public GravityController gravityController;
	[HideInInspector] public PlatformController platformController;
	
    public float timeUntilBroken;
    //public float floatSpeed;
    public LayerMask characterLayer;
	
	public Vector2 movement;

    private float breakTime;
	private float defaultSpeed = 1;
	private float movementSpeed;

	private int movementDirection;	
    private int timeBeforeDestroyed;
    private int platformDurability;
	int oldColliderCount;
	int newColliderCount;
	
	private BoxCollider2D boxCollider;
	public Collider2D[] collision; //= Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);
	
	//private Rigidbody2D rb2d;
    
	//private Vector2 hitDir;
    private IsflakSpawner isflakSpawner; //Change the name to MovingPlatformSpawner
	
	//private float speed;

	// Use this for initialization
    void Start()
    {
		collisionController = GetComponent<CollisionController>();
		gravityController = GetComponent<GravityController>();
		boxCollider = GetComponent<BoxCollider2D>();
        //rb2d = GetComponent<Rigidbody2D>();
		if(transform.parent != null)
		{
			//timeBeforeDestroyed = platformDurability;
			isflakSpawner = GetComponentInParent<IsflakSpawner>();
			movementSpeed = isflakSpawner.GetSpeed();
			platformDurability = isflakSpawner.GetDurability();
			movementDirection = isflakSpawner.GetDirection();
		}
	   /* if (transform.parent != null)
        {
            isflakSpawner = GetComponentInParent<IsflakSpawner>();
            movementSpeed = isflakSpawner.GetSpeed();
            platformDurability = isflakSpawner.GetDurability();
			movementDirection = isflakSpawner.GetDirection();
        }
        else
        {
            movementSpeed = defaultSpeed;
			movementDirection = 1;
        }*/
    }

	void FixedUpdate()
	{
		OnPlatform();
	}
    // Update is called once per frame
    void Update()
    {
		//Updates gravity every frame
		gravityController.UpdateGravity();
		
		//Add gravity to y-axis of Vector2 'movement'
		float verticalTranslate = gravityController.gravity * Time.deltaTime;
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
    }

	void OnPlatform()
	{
		//looking for player
		
		oldColliderCount = newColliderCount;
		
		Vector2 boxSize = boxCollider.size;

		collision = Physics2D.OverlapCircleAll(transform.position, 1.5f);
		
		newColliderCount = collision.Length;

		if(newColliderCount > oldColliderCount)
		{
			for(int i = 0; i < collision.Length; i++)
			{
				//Debug.Log(collision.Length);

				bool isOnLayer = characterLayer == (characterLayer | (1 << collision[i].gameObject.layer));
		
				if(isOnLayer)
				{
					timeBeforeDestroyed--;
				}

				if(collision[i].gameObject.tag == "Ground" && collision[i].gameObject != gameObject)
				{
					//Destroy(gameObject);
					if(i != 0)//(collision.Length > 2 && i > 1)
					{
						Destroy(collision[i].gameObject);
						Destroy(collision[i-1].gameObject);
					}
					
					//Destroy(collision[i].gameObject);
				}

				if(collision[i].gameObject.tag == "Workdammit")
				{
					print("platform");
				}
				/*if(collision[i].gameObject.tag == "Wall")
				{
					//movement.x = -movement.x;
					print("yes");
				}*/
				
				
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

	/* void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Wall")
		{
			//movement.x = -movement.x;
			print("yes");
		}
	}*/
	
    /*void OnCollisionEnter2D(Collision2D collision)
    {
		bool isOnLayer = characterLayer == (characterLayer | (1 << collision.gameObject.layer));
		
        if (isOnLayer)
        {
            timeBeforeDestroyed--;
        }
		
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
		
        //if (collision.gameObject.tag == "Wall")
        //{
        //    speed = -speed;
        //}
    }*/

    void whichHitDir()
    {
       /* if(speed > 0)
        {
            hitDir = Vector2.right;
        }
        else if(speed < 0)
        {
            hitDir = Vector2.left;
        }
        else
        {
            hitDir = new Vector2(0, 0);
        }*/
    }
	
  //  void MovePlatform()
    //{
		//movePlatform = new Vector2(speed, 0);
		//transform.Translate(movePlatform * Time.deltaTime);
		//rb2d.velocity = movePlatform;
		// Debug.Log(breakTime);
   // }

	/*void MoveObject(Vector2 movement)
	{
		//Checks for collisions 
		/*collisionController.UpdateRaycastDirections();
		collisionController.boxCollisionDirections.resetDirections();

		if(movement.y < 0)
		{
			collisionController.DescendSlope(ref movement);
		}

		//Only checks for collision if moving in any direction
		if(movement.x != 0 || movement.y != 0)
		{
			collisionController.checkCollision(ref movement);
		}

		//Translate the object in the direction movement
		platformController.MoveObject(movement);
		//transform.Translate(movement);
	}*/

    void DestroyPlatform()
    {
        Debug.Log(timeBeforeDestroyed);
		
        if (timeBeforeDestroyed <= 0)
        {
            breakTime += Time.deltaTime;
			
            if (breakTime > timeUntilBroken) 
			{
				Destroy(gameObject);
            }
        }
    }
	
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 4)
        {
            rb2d.velocity = new Vector2(speed, 0);
            //rb2d.bodyType = RigidbodyType2D.Kinematic;
            
        }
    }
	
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            //rb2d.bodyType = RigidbodyType2D.Dynamic;
        }
    }*/
}