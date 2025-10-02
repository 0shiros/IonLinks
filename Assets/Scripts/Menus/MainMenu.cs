using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LevelSelector()
    {
        SceneManager.LoadSceneAsync("LevelSelector");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
