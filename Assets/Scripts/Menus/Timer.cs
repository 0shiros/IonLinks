using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
   public float currentTime;
   private float minTime;
   [SerializeField] private float maxTime;
   private TextMeshProUGUI textMesh;
   private int seconds;
   private int minutes;
   [SerializeField] private GameObject defeatPanel;
   public bool launchTimer = false;
   private void Start()
   {
      currentTime = minTime;
      textMesh = GetComponent<TextMeshProUGUI>();
      defeatPanel.SetActive(false);
   }

   private void Update()
   {
      GameTimer();
   }

   public void GameTimer()
   {
      if (launchTimer)
      {
         currentTime += Time.deltaTime;
         minutes = Mathf.FloorToInt(currentTime / 60);
         seconds = Mathf.FloorToInt(currentTime % 60);
         
         if (currentTime >= maxTime)
         {
            defeatPanel.SetActive(true);
         }

         textMesh.text = string.Format("{0:00}:{1:00}", minutes, seconds);
      }
   }
}
