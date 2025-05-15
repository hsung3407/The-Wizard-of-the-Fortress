using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.Sound;

public class MainManager : MonoBehaviour
{
    [SerializeField] private AudioClip mainMusic;
    
    private void Start()
    {
        SoundManager.Instance.PlayMusic(mainMusic, 0.5f);
    }

    public void StartStage()
    {
        SceneManager.LoadScene("Ingame");
    }
}
