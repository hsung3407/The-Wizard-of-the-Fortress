using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public void StartStage()
    {
        SceneManager.LoadScene("Ingame");
    }
}
