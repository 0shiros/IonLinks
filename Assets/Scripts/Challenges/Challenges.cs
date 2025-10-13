using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Challenges : MonoBehaviour
{
    private List<Image> atomSprites = new();
    [SerializeField] private Timer timer;
    [SerializeField] private float timerChallenge;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            atomSprites.Add(child.GetComponentInChildren<Image>(true));
        }
    }

    private void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            CheckTimer();
            CheckAtomsUse();
        }
        
        Destroy(this);
    }

    private void CheckTimer()
    {
        if (timer.currentTime <= timerChallenge)
        {
            atomSprites[0].color = Color.white;
        }
    }

    private void CheckAtomsUse()
    {
        
    }
}
