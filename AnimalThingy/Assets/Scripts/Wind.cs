using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {

    public float pushStrenght;
    public float minWindChangeInterval;
    public float maxWindChangeInterval;
    public List<Rigidbody2D> playerRigidbody2D;

    private float currentChangeInterval;
    private float intervalTime;

    // Use this for initialization
    void Start()
    {
        currentChangeInterval = Random.Range(minWindChangeInterval, maxWindChangeInterval);
        foreach (Rigidbody2D rb2d in FindObjectsOfType<Rigidbody2D>())
        {
            if (rb2d.gameObject.layer == LayerMask.NameToLayer("Character"))
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
