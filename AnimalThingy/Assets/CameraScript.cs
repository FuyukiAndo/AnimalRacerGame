using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour {

    public string playerTag = "Player";
    public float cameraSmoothTime = 0.5f;

    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;


    private GameObject[] players;
    private GameObject furthestPosPlayer;
    private GameObject furthestNegPlayer;
    private Camera cam;
    private Vector3 velocity;

    private void Start()
    {
        cam = GetComponent<Camera>();
        players = GameObject.FindGameObjectsWithTag(playerTag);
        furthestPosPlayer = players[0].gameObject;
        furthestNegPlayer = players[1].gameObject;
    }

    private void LateUpdate()
    {
        CheckFurthestPosPlayer();
        CheckFurthestNegPlayer();
        Zoom();
        CameraFollow(furthestPosPlayer, furthestNegPlayer);
    }


    void CheckFurthestPosPlayer()
    {
        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].transform.position.x > furthestPosPlayer.transform.position.x)
            {
                if (players[i].transform.position.x - furthestPosPlayer.transform.position.x > furthestPosPlayer.transform.position.y - players[i].transform.position.y)
                {
                    GameObject newFurthest = players[i].gameObject;
                    furthestPosPlayer = newFurthest;
                }
            }
            if(players[i].transform.position.y > furthestPosPlayer.transform.position.y)
            {
                if (players[i].transform.position.y - furthestPosPlayer.transform.position.y > furthestPosPlayer.transform.position.x - players[i].transform.position.x)
                {
                    GameObject newFurthest = players[i].gameObject;
                    furthestPosPlayer = newFurthest;
                }
            }
        }
    }
    void CheckFurthestNegPlayer()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].transform.position.x < furthestNegPlayer.transform.position.x)
            {
                if(players[i].transform.position.x - furthestNegPlayer.transform.position.x < furthestNegPlayer.transform.position.y - players[i].transform.position.y)
                {
                    GameObject newFurthest = players[i].gameObject;
                    furthestNegPlayer = newFurthest;
                }
            }
            if (players[i].transform.position.y < furthestNegPlayer.transform.position.y)
            {
                if(players[i].transform.position.y - furthestNegPlayer.transform.position.y < furthestNegPlayer.transform.position.x - players[i].transform.position.x)
                {
                    GameObject newFurthest = players[i].gameObject;
                    furthestNegPlayer = newFurthest;
                }
            }
        }
    }
    
    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance(furthestPosPlayer, furthestNegPlayer)/ zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    float GetGreatestDistance(GameObject g1, GameObject g2)
    {
        var bounds = new Bounds(players[0].transform.position, Vector3.zero);
        for(int i = 0; i < players.Length; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }
        return bounds.size.x;
    }

    void CameraFollow(GameObject g1, GameObject g2)
    {
        Vector3 newPosition = new Vector3((g1.transform.position.x + g2.transform.position.x) / 2, (g1.transform.position.y + g2.transform.position.y) / 2, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, cameraSmoothTime);
    }
}
