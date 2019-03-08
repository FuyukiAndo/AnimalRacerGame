using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class MoveAllFix : MonoBehaviour {

    private GameObject[] gameObjects;
    private void Awake()
    {
        foreach (var gameobject in FindObjectsOfType<GameObject>())
        {
            var vector3 = gameobject.transform.position;

            vector3.z = 0;

            gameobject.transform.position = vector3;
        }
    }
}
