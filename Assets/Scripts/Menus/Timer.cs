using System;
using TMPro;
using UnityEngine;

public enum TimerState
{
   Basic,
   Running,
   Paused
}

public class Timer : MonoBehaviour
{
   public float currentTime;
   private float minTime;
   [SerializeField] private float maxTime;
   private TextMeshProUGUI textMesh;
   private int seconds;
   private int minutes;
   [SerializeField] private GameObject defeatPanel;
   public TimerState state = TimerState.Basic;
   
   private void Start()
   {
      Time.timeScale = 1f;
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
      if (state == TimerState.Running)
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
   
   public void StartTimer() => state = TimerState.Running;
   public void StopTimer() => state = TimerState.Paused;
}
