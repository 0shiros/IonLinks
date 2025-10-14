using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
   private int currentLevelIndex;

   private void Start()
   {
      currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
   }

   public void RestartLevel()
   {
      SceneManager.LoadSceneAsync(currentLevelIndex);
   }

   public void LoadMainMenu()
   {
      SceneManager.LoadSceneAsync("MainMenu");
   }

   public void LoadNextLevel()
   {
      SceneManager.LoadSceneAsync(currentLevelIndex + 1);
   }
}
