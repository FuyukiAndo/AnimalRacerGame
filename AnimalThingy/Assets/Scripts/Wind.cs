using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {

    public float pushStrenght;
    public float minWindChangeInterval;
    public float maxWindChangeInterval;
    public List<Rigidbody2D> playerRigidbody2D;
    public LayerMask characterLayer;

    private float currentChangeInterval;
    private float intervalTime;
    private Rigidbody2D myRb2d;

    // Use this for initialization
    void Start()
    {
        myRb2d = GetComponent<Rigidbody2D>();
        myRb2d.bodyType = RigidbodyType2D.Kinematic;
        myRb2d.simulated = false;

        currentChangeInterval = Random.Range(minWindChangeInterval, maxWindChangeInterval);
        foreach (Rigidbody2D rb2d in FindObjectsOfType<Rigidbody2D>())
        {
            bool isLayerSame = characterLayer == (characterLayer | (1 << rb2d.gameObject.layer));
            if (isLayerSame)
            {
                playerRigidbody2D.Add(rb2d);
            }
        }
    }

    void windDiraction()
    {
        intervalTime += Time.deltaTime;
        if(intervalTime > currentChangeInterval)
        {
            pushStrenght = -pushStrenght;
            intervalTime = 0;
            currentChangeInterval = Random.Range(minWindChangeInterval, maxWindChangeInterval);
        }
        foreach (Rigidbody2D rb2d in playerRigidbody2D)
        {
            rb2d.velocity = new Vector2(pushStrenght, rb2d.velocity.y);
        }
    }
    void Update() {
        Debug.Log(currentChangeInterval);
        windDiraction();
    }
       //Currently Random Time intervals
       //Might want Random diraction in x or both diractions instead
}
