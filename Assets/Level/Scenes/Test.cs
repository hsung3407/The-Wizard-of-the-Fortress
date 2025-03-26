    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

public class Test : MonoBehaviour
{
    private bool condition;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            condition = true;
            Debug.Log($"Input {Time.time}");
        }
    }

    IEnumerator TestC()
    {
        yield return new WaitUntil(() => condition);
        Debug.Log($"Coroutine {Time.time}");
    }
}
