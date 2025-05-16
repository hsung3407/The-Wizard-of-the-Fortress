using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.Sound;

public class MainManager : MonoBehaviour
{
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private float volume = 0.5f;
    
    private void Start()
    {
        SoundManager.Instance.PlayMusic(mainMusic, volume);
    }

    public void StartStage()
    {
        SceneManager.LoadScene("Ingame");
    }
}
