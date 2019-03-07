using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSCheckpoint : MonoBehaviour {

    public List<Transform> checkpoints;
    public Image arrow;
    public Vector2 offset;
    private int index = 0;
    private bool outofScreen, outofScreenX,outofScreenY;
    private Transform currentCheckpoint;
    private Canvas canvas;
    private float checkX;
    private float checkY;
    private Vector3 dir;
    
    public static GPSCheckpoint Instance
    {
        get
        {
            return instance;
        }
    }
    private static GPSCheckpoint instance;

    // Use this for initialization
    void Start () {
        
        if(instance == null)
        {
            instance = this;
        }
        currentCheckpoint = checkpoints[index];
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

    }
    public void UpdateCheckpointToGo()
    {
        index++;
        currentCheckpoint = checkpoints[index];
    }
	private void UpdateRotation()
    {
        dir = arrow.rectTransform.position - currentCheckpoint.position;
        dir.Normalize();

        float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, rot_z -90);
    }
    void UpdateScreenArrow()    
    {
        UpdateIfInsideOfScreenX();
        UpdateIfInsideOfScreenY();
        checkX = currentCheckpoint.position.x;
        checkY = currentCheckpoint.position.y;
        if (outofScreenX && outofScreenY)
        {
            UpdateRotation();
            return;
        }
        if (outofScreenX)
        {
            UpdateRotation();
            arrow.rectTransform.position = new Vector3(arrow.rectTransform.position.x, Camera.main.WorldToScreenPoint(new Vector2(checkX, checkY)).y);
            return;
        }
        if (outofScreenY)
        {
            UpdateRotation();
            arrow.rectTransform.position = new Vector3(Camera.main.WorldToScreenPoint(new Vector2(checkX, checkY)).x, arrow.rectTransform.position.y);
            return;
        }

        arrow.rectTransform.position = (Vector2)Camera.main.WorldToScreenPoint(new Vector2(checkX,checkY));
  
    }
    void UpdateIfInsideOfScreenX()
    {
        if(canvas.worldCamera.WorldToScreenPoint(checkpoints[index].position).x < 0  || canvas.worldCamera.WorldToScreenPoint(checkpoints[index].position).x > Screen.width - arrow.rectTransform.rect.width)
        {
            outofScreenX = true;
            return;
        }
        else
        {
            outofScreenX = false;
        }
    }

    void UpdateIfInsideOfScreenY()
    {
        if (canvas.worldCamera.WorldToScreenPoint(checkpoints[index].position).y < 0 || canvas.worldCamera.WorldToScreenPoint(checkpoints[index].position).y > Screen.height - arrow.rectTransform.rect.height)
        {
            outofScreenY = true;
            return;
        }
        else
        {
            outofScreenY = false;
        }
    }

    // Update is called once per frame
    void Update () {
        UpdateScreenArrow();
	}
}
