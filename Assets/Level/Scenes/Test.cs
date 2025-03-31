    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Awake");
    }

    void OnEnable()
    {
        Debug.Log("Enabled");
    }
    
    void Start()
    {
        Debug.Log("Start");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            var gameobject = new GameObject
            {
                name = "Test Gen"
            };

            var type = Type.GetType("Test");
            var t = gameobject.AddComponent(type);   
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log($"Invoke On Test : {Time.time} / {Time.frameCount} - {Time.deltaTime}");
            BroadcastMessage("OnTest");
            StartCoroutine(TestTime());
        }
    }

    protected void OnTest()
    {
        Debug.Log($"OnTest : {Time.time}");
    }

    IEnumerator TestTime()
    {
        yield return null;
        Debug.Log($"TestTime : : {Time.time} / {Time.frameCount} - {Time.deltaTime}");
    }
}
