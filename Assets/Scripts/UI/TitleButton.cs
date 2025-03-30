using System;
using Ingame;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public void IntoMain()
    {
        SceneManager.LoadScene("Main");
    }
}
