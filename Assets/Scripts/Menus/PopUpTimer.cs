using System;
using System.Collections;
using UnityEngine;

public class PopUpTimer : MonoBehaviour
{
    [SerializeField] private int timeToWait = 5;
    
    private void Start()
    {
        StartCoroutine(ActivePopUp());
    }

    IEnumerator ActivePopUp()
    {
        yield return new WaitForSeconds(timeToWait);
        gameObject.SetActive(false);
    }
}
