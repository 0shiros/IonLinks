using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Links : MonoBehaviour
{
    [SerializeField] private float radiusAtomDetection;
    [SerializeField] private LayerMask layerMask;
    private List<Collider2D> nearestHits;
   
    private PickAndDrop pickAndDrop;
    private bool canCreateLink = false;
    private int currentLinkNumber = 0;
    private int minLinkNumber = 0;
    private int maxLinkNumber = 2;
    
    [SerializeField] List<LineRenderer> previewLineRenderers;
    [SerializeField] List<LineRenderer> lineRenderers;

    private void Start()
    {
        pickAndDrop = GetComponent<PickAndDrop>();
    }

    private void Update()
    {
        AtomsNextDetection();
        LockAtom();
        UnlockAtom();
        PreviewJoints();
        LinksCurrentPositions();
    }

    List<Collider2D> FindNearestHits(Transform atomPicked, List<Collider2D> hits)
    {
        return hits.OrderBy(hitPoint => Vector3.Distance(atomPicked.transform.position, hitPoint.transform.position)).Take(maxLinkNumber).ToList();
    }

    private void AtomsNextDetection()
    {
        if (pickAndDrop.isPicking)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radiusAtomDetection, layerMask);
            List<Collider2D> hitResults = new (hitColliders);
            hitResults.Remove(GetComponent<Collider2D>());
            
            nearestHits = FindNearestHits(transform, hitResults);
            
            if (nearestHits.Count == maxLinkNumber)
            {
                canCreateLink = true;
            }
            else if (nearestHits.Count != maxLinkNumber)
            {
                canCreateLink = false;
            }
            else
            {
                Debug.Log("Se passe un truc pas net");
            }

            if (hitResults.Count < maxLinkNumber)
            {
                nearestHits.Clear();
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
        if (canCreateLink && !pickAndDrop.isPicking)
        {
            if (currentLinkNumber < maxLinkNumber)
            {
                foreach (Collider2D hit in nearestHits)
                {
                    SpringJoint2D joint = gameObject.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;
                    if (joint == null) return;
                    joint.connectedBody = hit.GetComponent<Rigidbody2D>();
                    PickAndDrop jointPickAndDrop = hit.GetComponent<PickAndDrop>();
                    jointPickAndDrop.isLocked = true;
                    currentLinkNumber++;
                }
                
                for (int i = 0; i < maxLinkNumber; i++)
                {
                    lineRenderers[i].positionCount = maxLinkNumber;
                }

                pickAndDrop.isLocked = true;
                
            }
        }
    }

    private void LinksCurrentPositions()
    {
        for (int i = 0; i < maxLinkNumber; i++)
        {
            if (lineRenderers[i].positionCount == maxLinkNumber)
            {
                lineRenderers[i].SetPosition(0, transform.position);
                lineRenderers[i].SetPosition(1, nearestHits[i].transform.position);
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
                    
                    for (int i = 0; i < maxLinkNumber; i++)
                    {
                        lineRenderers[i].positionCount = minLinkNumber;
                    }
                    
                    currentLinkNumber = minLinkNumber;
                    canCreateLink = false;
                }
            }
        }
    }

    private void PreviewJoints()
    {
        if (canCreateLink && pickAndDrop.isPicking)
        {
            for (int i = 0; i < maxLinkNumber; i++)
            {
                previewLineRenderers[i].positionCount = maxLinkNumber;
                previewLineRenderers[i].SetPosition(0, transform.position);
                previewLineRenderers[i].SetPosition(1, nearestHits[i].transform.position);
            }
        }
        else
        {
            for (int i = 0; i < maxLinkNumber; i++)
            {
                previewLineRenderers[i].positionCount = minLinkNumber;
            }
        }
    }
}
