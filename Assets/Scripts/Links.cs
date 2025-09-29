using System.Collections.Generic;
using UnityEngine;

public class Links : MonoBehaviour
{
    private List<Collider2D> hitResults = new();
    [SerializeField] private float radiusAtomDetection;
    [SerializeField] private LayerMask layerMask;

    private PickAndDrop pickAndDrop;
    private bool canCreateLink = false;
    private int currentLinkNumber = 0;
    private int maxLinkNumber = 2;

    private void Start()
    {
        pickAndDrop = GetComponent<PickAndDrop>();
    }

    private void FixedUpdate()
    {
        AtomsNextDetection();
        LockAtom();
    }

    private void AtomsNextDetection()
    {
        if (pickAndDrop.isPicking)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radiusAtomDetection, layerMask);
            hitResults = new List<Collider2D>(hitColliders);
            hitResults.Remove(GetComponent<Collider2D>());

            if (hitResults.Count == 2)
            {
                canCreateLink = true;
            }
            else if (hitResults.Count != 2)
            {
                canCreateLink = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusAtomDetection);
    }

    private void LockAtom()
    {
        if (canCreateLink && pickAndDrop.isPicking == false)
        {
            if (currentLinkNumber < maxLinkNumber)
            {
                foreach (Collider2D hit in hitResults)
                {
                    SpringJoint2D joint = gameObject.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;
                    joint.connectedBody = hit.GetComponent<Rigidbody2D>();
                    PickAndDrop jointPickAndDrop = hit.GetComponent<PickAndDrop>();
                    jointPickAndDrop.isLocked = true;
                    currentLinkNumber++;
                }
            }
            
            pickAndDrop.isLocked = true;
        }
    }
    
}
