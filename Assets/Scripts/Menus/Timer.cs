using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TimerState
{
   Basic,
   Running,
   Paused
}

public class Timer : MonoBehaviour
{
   public float currentTime;
   private int minTime;
   [SerializeField] private int maxTime;
   private Image timerImage;
   private TextMeshProUGUI textMesh;
   private int seconds;
   private int minutes;
   [SerializeField] private GameObject defeatPanel;
   public TimerState state = TimerState.Basic;
   
   private void Start()
   {
      Time.timeScale = 1f;
      currentTime = maxTime;
      minTime = 0;
      textMesh = GetComponentInChildren<TextMeshProUGUI>();
      defeatPanel.SetActive(false);
      textMesh.text = string.Format("{0:00}:{1:00}", 
         minutes = Mathf.FloorToInt(currentTime / 60), Mathf.FloorToInt(currentTime % 60));
   }

   private void Update()
   {
      GameTimer();
   }

   public void GameTimer()
   {
      if (state == TimerState.Running && currentTime > minTime)
      {
         currentTime -= Time.deltaTime;
         seconds = Mathf.FloorToInt(currentTime % 60);
         minutes = Mathf.FloorToInt(currentTime / 60);
         
         if (currentTime <= minTime)
         {
            defeatPanel.SetActive(true);
         }

         textMesh.text = string.Format("{0:00}:{1:00}", minutes, seconds);
      }
   }
   
   public void StartTimer() => state = TimerState.Running;
   public void StopTimer() => state = TimerState.Paused;
}
