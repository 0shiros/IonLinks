using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Links : MonoBehaviour
{
    private List<Collider2D> hitResults = new();
    [SerializeField] private float radiusAtomDetection;
    [SerializeField] private LayerMask layerMask;

    private PickAndDrop pickAndDrop;
    private bool canCreateLink = false;
    private int currentLinkNumber = 0;
    private int minLinkNumber = 0;
    private int maxLinkNumber = 2;

    private void Start()
    {
        pickAndDrop = GetComponent<PickAndDrop>();
    }

    private void Update()
    {
        AtomsNextDetection();
        LockAtom();
        UnlockAtom();
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
            else
            {
                Debug.Log("Se passe un truc pas net");
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
                
                pickAndDrop.isLocked = true;
                
            }
        }
    }
    
    private void UnlockAtom()
    {
        if (pickAndDrop.isLocked)
        {
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit2D hitMouse = Physics2D.Raycast(pickAndDrop.mousePosition, Vector2.zero);

                if (hitMouse.collider == pickAndDrop.atomCollider)
                {
                    pickAndDrop.isLocked = false;
                    SpringJoint2D[] currentJoint = GetComponents<SpringJoint2D>();
                    foreach (SpringJoint2D joint in currentJoint)
                    {
                        Destroy(joint);
                    }

                    currentLinkNumber = minLinkNumber;
                    canCreateLink = false;
                }
            }
        }
    }
    
}
