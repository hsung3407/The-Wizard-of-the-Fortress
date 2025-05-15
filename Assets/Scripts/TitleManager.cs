using System;
using TMPro;
using UnityEngine;
using Utility.Sound;

public class TitleManager : MonoBehaviour
{
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");
    [SerializeField] private Material skybox;
    [SerializeField] private Transform directionalLight;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private TextMeshProUGUI tmp;
    
    [SerializeField] private AudioClip titleMusic;

    private void Start()
    {
        SoundManager.Instance.PlayMusic(titleMusic, 0.5f);
    }

    void Update()
    {
        directionalLight.rotation *= Quaternion.Euler(0, -Time.deltaTime * rotateSpeed, 0);
        skybox.SetFloat(Rotation, Mathf.Repeat(Time.time * rotateSpeed, 360));

        tmp.alpha = Mathf.PingPong(Time.time, 1);
        tmp.rectTransform.localScale = Vector3.one * (Mathf.PingPong(Time.time, 1) * 0.2f + 0.9f);
    }
}