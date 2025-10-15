using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
    }

    public void LevelSelector()
    {
        SceneManager.LoadSceneAsync("LevelSelector");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
