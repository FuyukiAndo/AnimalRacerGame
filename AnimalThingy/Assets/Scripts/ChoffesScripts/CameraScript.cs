using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour {
    public float cameraSmoothTime = 0.5f;

    public float minZoomSinglePlayer, minZoom = 100f;
    public float maxZoomSinglePlayer, maxZoom = 10f;
    public float zoomLimiter = 50f;


    public List<GameObject> players;
    private GameObject furthestPosPlayer;
    private GameObject furthestNegPlayer;
    private Camera cam;
    private Vector3 velocity;

	public float startFov;
	public Transform startPos;
	public float timeBeforeZoomToPlayers;
	public float camAngle = 5, camYOffset = 5;

	private bool followPlayers = false;

	private void Start()
    {
        players = new List<GameObject>();
        cam = GetComponent<Camera>();
		cam.transform.Rotate(camAngle, 0f, 0f);
		StartCoroutine(Overlook());
	}

    public void BindPlayersToCamera()
    {
        foreach (var player in FindObjectsOfType<PlayerInput>())
        {
            players.Add(player.gameObject);
        }
        if (players.Count > 1)
        {
			furthestPosPlayer = players[0].gameObject;
			furthestNegPlayer = players[1].gameObject;
        }
    }

    private void LateUpdate()
    {
        if(players.Count == 1)
        {
            if (followPlayers)
            {
                var newPosition =  new Vector3(players[0].transform.position.x, players[0].transform.position.y + camYOffset, transform.position.z);
                transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, cameraSmoothTime);
                transform.parent = players[0].transform;
                float newZoom = Mathf.Lerp(maxZoomSinglePlayer, minZoomSinglePlayer, GetGreatestDistance() / zoomLimiter);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
            }
        }
        else
        {
            if (followPlayers)
            {
                CheckFurthestPosPlayer();
                CheckFurthestNegPlayer();
                Zoom();
                CameraFollow(furthestPosPlayer, furthestNegPlayer);
            }

        }
    }


    void CheckFurthestPosPlayer()
    {
        for(int i = 0; i < players.Count; i++)
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
        for (int i = 0; i < players.Count; i++)
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
        for(int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }
        return bounds.size.magnitude;
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(players[0].transform.position, Vector3.zero);
        bounds.Encapsulate(players[0].transform.position);
        return bounds.size.magnitude;
    }

    void CameraFollow(GameObject g1, GameObject g2)
    {
        Vector3 newPosition = new Vector3((g1.transform.position.x + g2.transform.position.x) / 2, ((g1.transform.position.y + g2.transform.position.y) / 2) + camYOffset, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, cameraSmoothTime);
    }

    IEnumerator Overlook()
    {
        Debug.Log(players.Count);
        if (startPos != null)
        {
			transform.position = startPos.position;
        }
		cam.fieldOfView = startFov;
		yield return new WaitForSeconds(timeBeforeZoomToPlayers);
		followPlayers = true;
	}
}
