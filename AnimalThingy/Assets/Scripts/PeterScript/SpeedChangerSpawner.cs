using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChangerSpawner : MonoBehaviour {

    public GameObject goodMushroom;
    [Range(0, 1)]
    public float chanceForGoodMushroom;
    public float goodMushroomSpeed;

    public GameObject badMushroom;
    [Range(0, 1)]
    public float chanceForBadMushroom;
    public float badMushroomSpeed;

    public bool wantSpawnerToBeRemoved;
    private int chance;
    private float spawnChanceAmountOnGood;
    private void Awake()
    {
        if (wantSpawnerToBeRemoved == true)
        {
            if (GetComponent<MeshRenderer>() != null)
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
            if (GetComponent<Collider2D>() != null)
            {
                GetComponent<Collider2D>().enabled = false;
            }
        }
        goodMushroomSpeed = Mathf.Clamp(goodMushroomSpeed, 0, Mathf.Infinity);
        badMushroomSpeed = Mathf.Clamp(badMushroomSpeed, -Mathf.Infinity, 0);
        chanceForBadMushroom = 1 - chanceForGoodMushroom;
        
    }
    private void Start()
    {
        chance = Random.Range(0, 100);
        spawnChanceAmountOnGood = chanceForGoodMushroom * 100;
        Debug.Log(spawnChanceAmountOnGood);
        if (chance < spawnChanceAmountOnGood)
        {
            Instantiate(goodMushroom, transform.position, new Quaternion(0, 0, 0, 0), gameObject.transform);
        }
        else
        {
            Instantiate(badMushroom, transform.position, new Quaternion(0, 0, 0, 0), gameObject.transform);
        }
    }
    public float getGoodSpeed
    {
        get { return goodMushroomSpeed; }
    }
    public float getBadSpeed
    {
        get { return badMushroomSpeed; }
    }
}
