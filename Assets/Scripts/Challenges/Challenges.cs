using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Challenges : MonoBehaviour
{
    private List<Image> atomSprites = new();
    [SerializeField] private Timer timer;
    [SerializeField] private float timerChallenge;
    [SerializeField] private AtomSpawner atomSpawner;
    [SerializeField] private int atomChallenge;
    private int currentLevel;
    private bool hasSuccessedTimerChallenge = false;
    private bool hasSuccessedAtomChallenge = false;
    
    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "Background")
            {
                atomSprites.Add(child.GetComponentInChildren<Image>(true));
            }
        }
        
        currentLevel = SceneManager.GetActiveScene().buildIndex - 2;
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
        if (PlayerPrefs.GetInt("TimerChallenge" + currentLevel) == 0)
        {
            if (Mathf.FloorToInt(timer.currentTime) <= timerChallenge)
            {
                atomSprites[0].color = Color.white;
                hasSuccessedTimerChallenge = true;
                PlayerPrefs.SetInt("TimerChallenge" + currentLevel, hasSuccessedTimerChallenge ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
        else
        {
            atomSprites[0].color = Color.white;
        }
    }

    private void CheckAtomsUse()
    {
        if (PlayerPrefs.GetInt("AtomChallenge" + currentLevel) == 0)
        {
            int count = 0;

            foreach (GameObject atom in atomSpawner.atoms)
            {
                PickAndDrop atomLock = atom.GetComponent<PickAndDrop>();
                if (atomLock.isLocked)
                {
                    count++;
                }
            }

            if (count <= atomChallenge)
            {
                atomSprites[1].color = Color.white;
                hasSuccessedAtomChallenge = true;
                PlayerPrefs.SetInt("AtomChallenge" + currentLevel, hasSuccessedAtomChallenge ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
        else
        {
            atomSprites[1].color = Color.white;
        }
    }
}
