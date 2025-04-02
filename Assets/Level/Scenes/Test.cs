    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger Enter : {Time.frameCount}");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Trigger Exit : {Time.frameCount}");
    }
}
