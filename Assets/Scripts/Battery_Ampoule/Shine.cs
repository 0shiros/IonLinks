using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shine : MonoBehaviour
{
   private Animator animator;
   [SerializeField] private GameObject victoryPanel;
   private int nextLevel;
   
   private void Start()
   {
      animator = GetComponent<Animator>();
      nextLevel = SceneManager.GetActiveScene().buildIndex - 1;
   }

   public void ShineAnimation()
   {
      animator.SetBool("canShine", true);
   }

   public void WinLevel()
   {
      if(!victoryPanel.activeSelf) victoryPanel.SetActive(true);
     
      //Unlock the next level in selector level
      if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCount-1)
      {
         PlayerPrefs.SetInt("chains" + nextLevel, 1);
         PlayerPrefs.Save();
      }
   }
   
}
