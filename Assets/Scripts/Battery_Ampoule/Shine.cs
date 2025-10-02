using System;
using UnityEngine;

public class Shine : MonoBehaviour
{
   private Animator animator;
   [SerializeField] private GameObject victoryPanel;
   
   private void Start()
   {
      animator = GetComponent<Animator>();
   }

   public void ShineAnimation()
   {
      animator.SetBool("canShine", true);
   }

   public void WinLevel()
   {
      if(!victoryPanel.activeSelf) victoryPanel.SetActive(true);
   }
   
}
