using System;
using UnityEngine;

public class Links : MonoBehaviour
{
    [SerializeField] private float radiusAtomDetection;
    [SerializeField] private LayerMask layerMask;

    private void FixedUpdate()
    {
        AtomsNextDetection();
    }

    private void AtomsNextDetection()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radiusAtomDetection, layerMask);

        if (hitColliders.Length > 0)
        {
            Debug.Log(hitColliders.Length);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusAtomDetection);
    }
}
