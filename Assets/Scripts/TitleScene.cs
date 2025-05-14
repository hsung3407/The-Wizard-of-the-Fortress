using System;
using UnityEngine;

public class Title : MonoBehaviour
{
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");
    [SerializeField] private Material skybox;
    [SerializeField] private Transform directionalLight;
    [SerializeField] private float rotateSpeed;
    
    void Update()
    {
        directionalLight.rotation *= Quaternion.Euler(0, -Time.deltaTime * rotateSpeed, 0);
        skybox.SetFloat(Rotation, Mathf.Repeat(Time.time * rotateSpeed, 360));;
    }
}
