using System;
using System.Collections;
using System.Collections.Generic;
using Ingame;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(5f);
            for (int j = 0; j < TsetTwo.Count; j++)
            {
                var go = Instantiate(prefab, transform.position, Quaternion.identity);
            
                Destroy(go, 3f);
            }
        }
    }
}
