using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ingame;
using Ingame.Player;
using UnityEngine;

public class Test : MonoBehaviour
{
    Dictionary<object, int> map = new Dictionary<object, int>();
    List<object> list = new List<object>();

    private int i;
    private void Awake()
    {
        for (i = 0; i < 10; i++)
        {
            var newObj = new GameObject
            {
                name = "Test" + i
            };
            list.Add(newObj);
            map.Add(newObj, i);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var newObj = new GameObject
            {
                name = "Test" + i
            };
            list.Add(newObj);
            map.Add(newObj, i);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            foreach (GameObject o in list)
            {
                Destroy(o);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("List");
            foreach (var obj in list)
            {
                Debug.Log(obj);
            }
            Debug.Log("Map");
            foreach (var keyValuePair in map)
            {
                Debug.Log(keyValuePair);
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (var keyValuePair in map)
            {
                Debug.Log(map[null!]);
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            map.Remove(null!);
        }
    }
}
