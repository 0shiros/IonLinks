using System;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAtoms : MonoBehaviour
{
    [SerializeField] private Vector2 size;
    [SerializeField] private LayerMask layer;
    [SerializeField] private AtomSpawner spawner;
    private Vector2 atomSpawnerPosition;

    private void Start()
    {
        atomSpawnerPosition = (Vector2)spawner.gameObject.transform.localPosition + new Vector2(0,6);
    }

    private void Update()
    {
        ResetAtoms();
    }

    private void ResetAtoms()
    {
        Collider2D[] fallAtoms = Physics2D.OverlapBoxAll(transform.position, size, 0, layer);
        List<Collider2D> currentAtoms = new(fallAtoms);

        if (currentAtoms.Count > 0)
        {
            foreach (Collider2D col in currentAtoms)
            {
                PickAndDrop atomPickAndDrop = col.GetComponent<PickAndDrop>();
                CreateLinks atomCreateLinks = col.GetComponentInChildren<CreateLinks>();

                atomPickAndDrop.isPicking = false;
                atomCreateLinks.UnlockAtom();
                atomCreateLinks.DestroyLineRenderers();
                
                col.transform.position = atomSpawnerPosition;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (transform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}
