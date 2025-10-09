using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ReachIt : MonoBehaviour
{

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector2 size;

    private float timer = 0;
    private float startTime = 0;
    [SerializeField] private float maxTime;
    [SerializeField] private Shine shine;
    private Animator animator;
    
    private Light2D spotLight;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spotLight = GetComponentInChildren<Light2D>();
        spotLight.intensity = 0;
    }

    private void Update()
    {
        CheckAtoms();
    }

    private void CheckAtoms()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, size,0, layerMask);
        List<Collider2D> currentHits = new(hitColliders);
        currentHits.Remove(transform.GetComponent<Collider2D>());

        List<Collider2D> filteredHits = new();
        
        for (int i = 0; i < currentHits.Count; i++)
        {
            PickAndDrop hitPickAndDrop = currentHits[i].GetComponent<PickAndDrop>();
            if (hitPickAndDrop.isLocked)
            {
                filteredHits.Add(currentHits[i]);
            }
        }
        
        if (filteredHits.Count >= 1)
        {
            animator.SetBool("canShine", true);
            
            if ((timer += Time.deltaTime) > maxTime)
            {
                Debug.Log("Reach It");
                shine.ShineAnimation();
            }
        }
        else
        {
            animator.SetBool("canShine", false);
            spotLight.intensity = 0;
            timer = startTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (transform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, size);
        }
    }

    public void SwitchSpotLightState()
    {
        if (spotLight.intensity == 1)
        {
            spotLight.intensity = 0;
        }
        else
        {
            spotLight.intensity = 1;
        }
    }
}

