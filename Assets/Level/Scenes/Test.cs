using System;
using System.Collections;
using UnityEngine;

namespace Level.Scenes
{
    public class Test : MonoBehaviour
    {
        Coroutine _coroutine;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (_coroutine != null)
                {
                    Debug.Log("Stop");
                    StopCoroutine(_coroutine);
                }
                Debug.Log("Start");
                _coroutine = StartCoroutine(TestC(_coroutine));
            }
        }

        private IEnumerator TestC(Coroutine self)
        {
            Debug.Log("SC");
            yield return null;
            Debug.Log("EC");

            self = null;
        }
    }
}