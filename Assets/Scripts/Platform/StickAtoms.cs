using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StickAtoms : MonoBehaviour
{
    [SerializeField] private Vector2 size;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Timer timer;
    
    private void Update()
    {
        GlueAtoms();
    }

    private void GlueAtoms()
    {
        Collider2D[] hitAtoms = Physics2D.OverlapBoxAll(transform.position, size, 0,layerMask);
        List<Collider2D> currentAtoms = new(hitAtoms);
        
        if (currentAtoms.Count > 0)
        {
            foreach (Collider2D atom in currentAtoms)
            {
                PickAndDrop atomPickAndDrop = atom.GetComponent<PickAndDrop>();
                CreateLinks atomCreateLinks = atom.GetComponentInChildren<CreateLinks>();
                
                if (!atomPickAndDrop.isPicking)
                {
                    atom.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    atomCreateLinks.isMagnetised = true;
                    atomPickAndDrop.isLocked = true;
                    timer.launchTimer = true;
                }
            }
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
}
