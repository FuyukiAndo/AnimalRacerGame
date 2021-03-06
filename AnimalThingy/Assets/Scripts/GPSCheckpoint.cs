﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSCheckpoint : MonoBehaviour {

    public List<Transform> checkpoints;
    public Image arrow;
    public Vector2 offset;
    private int index = 0;
    private bool outofScreenX,outofScreenY;
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

        if (instance == null)
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
        dir = transform.position - currentCheckpoint.position;

        dir.Normalize();

        float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrow.rectTransform.eulerAngles = new Vector3(0f, 0f, rot_z - 90 );
    }
    void UpdateScreenArrow()    
    {
        UpdateIfInsideOfScreenX();
        UpdateIfInsideOfScreenY();
        checkX = currentCheckpoint.position.x + offset.x;
        checkY = currentCheckpoint.position.y + offset.y;
        if (outofScreenX && outofScreenY)
        {
            UpdateRotation();
            return;
        }
        if (outofScreenX)
        {
            UpdateRotation();
            arrow.rectTransform.position = new Vector3(arrow.rectTransform.position.x, Camera.main.WorldToScreenPoint(new Vector3(checkX, checkY)).y);
            return;
        }
        if (outofScreenY)
        {
            UpdateRotation();
            arrow.rectTransform.position = new Vector3(Camera.main.WorldToScreenPoint(new Vector3(checkX, checkY)).x, arrow.rectTransform.position.y);
            return;
        }
        arrow.rectTransform.eulerAngles = new Vector3(0f, 0f, 0f);
        arrow.rectTransform.position = Camera.main.WorldToScreenPoint(new Vector3(checkX,checkY));
  
    }
    void UpdateIfInsideOfScreenX()
    {
        if(Camera.main.WorldToScreenPoint(checkpoints[index].position).x < 0 + arrow.rectTransform.rect.width || Camera.main.WorldToScreenPoint(checkpoints[index].position).x > Screen.width - arrow.rectTransform.rect.width)
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
        if (Camera.main.WorldToScreenPoint(checkpoints[index].position).y < 0 || Camera.main.WorldToScreenPoint(checkpoints[index].position).y > Screen.height - arrow.rectTransform.rect.height)
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
