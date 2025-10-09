using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Shine : MonoBehaviour
{
   private Animator animator;
   [SerializeField] private GameObject victoryPanel;
   private int nextLevel;
   private Light2D spotLight;
   
   [SerializeField] private float startIntensity;
   [SerializeField] private float targetIntensity;
   [SerializeField] private float duration;
   
   private void Start()
   {
      spotLight = GetComponentInChildren<Light2D>();
      animator = GetComponent<Animator>();
      nextLevel = SceneManager.GetActiveScene().buildIndex - 1;
      spotLight.intensity = 0;
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

   public void AnimationCallFadeLight()
   {
      StartCoroutine(FadeLightIntensity(startIntensity, targetIntensity, duration));
   }
   
   IEnumerator FadeLightIntensity(float startIntensity, float targetIntensity, float duration)
   {
      float elapsedTime = 0f;

      while (elapsedTime < duration)
      {
         float t = elapsedTime / duration;
         spotLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
         elapsedTime += Time.deltaTime;
         yield return null; 
      }
      
      spotLight.intensity = targetIntensity;
   }
   
}
