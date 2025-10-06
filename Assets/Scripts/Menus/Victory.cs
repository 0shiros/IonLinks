using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
   public void RestartLevel()
   {
      SceneManager.LoadSceneAsync("Level0");
   }

   public void LoadMainMenu()
   {
      SceneManager.LoadSceneAsync("MainMenu");
   }

   public void LoadNextLevel()
   {
      SceneManager.LoadSceneAsync("Level1");
   }
}
