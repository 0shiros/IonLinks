using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    
    public void OpenPausePanel()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ClosePausePanel()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
