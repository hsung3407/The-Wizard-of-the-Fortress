using System;
using System.Collections;
using UnityEngine;

public class TsetTwo : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Move : {Time.frameCount}");
            transform.position = new Vector3(0, 10000, 0);

            StartCoroutine(Wait());

        }
    }

    IEnumerator Wait()
    {
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        gameObject.SetActive(false);
    }
}
