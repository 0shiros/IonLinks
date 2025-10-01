using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateLinks : MonoBehaviour
{
    [SerializeField] private float radiusAtomDetection;
    [SerializeField] private LayerMask layerMask;
    public List<Collider2D> nearestHits;

    public PickAndDrop pickAndDrop;
    public Transform atomsTransform;
    public bool canCreateLink = false;
    private int currentLinkNumber = 0;
    public int minLinkNumber = 0;
    public int maxLinkNumber = 2;
    public float frequencyJoints = 10;
    public float forceToBreak = 400;
    
    private List<LineRenderer> lineRenderers;

    private void Start()
    {
        atomsTransform = transform.parent;
        pickAndDrop = atomsTransform.GetComponent<PickAndDrop>();
        LineRenderer[] getLineRenderers = GetComponentsInChildren<LineRenderer>();
        lineRenderers = new(getLineRenderers);
    }

    private void Update()
    {
        AtomsNextDetection();
        LockAtom();
        UnlockAtom();
        LinksCurrentPositions();
        DestroyLineRenderers();
    }

    List<Collider2D> FindNearestHits(Transform atomPicked, List<Collider2D> hits)
    {
        return hits.OrderBy(hitPoint => Vector3.Distance(atomPicked.transform.position, hitPoint.transform.position)).Take(maxLinkNumber).ToList();
    }

    private void AtomsNextDetection()
    {
        if (pickAndDrop.isPicking)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(atomsTransform.position, radiusAtomDetection, layerMask);
            List<Collider2D> hitResults = new (hitColliders);
            hitResults.Remove(atomsTransform.GetComponent<Collider2D>());
            
            foreach (Collider2D hit in hitResults)
            {
                PickAndDrop hitPickAndDrop = hit.GetComponent<PickAndDrop>();
                
                if (!hitPickAndDrop.isLocked)
                {
                    hitResults.Remove(hit.GetComponent<Collider2D>());
                }
                
                Debug.Log(hit.name);
            }
            
            //Check s'ils sont locked et les remove s'ils ne le sont pas
            
            nearestHits = FindNearestHits(atomsTransform, hitResults);
            
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
        if (atomsTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(atomsTransform.position, radiusAtomDetection);
        }
    }

    private void LockAtom()
    {
        if (canCreateLink && !pickAndDrop.isPicking)
        {
            if (currentLinkNumber < maxLinkNumber)
            {
                foreach (Collider2D hit in nearestHits)
                {
                    SpringJoint2D joint = atomsTransform.gameObject.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;
                    if (joint == null) return;
                    joint.connectedBody = hit.GetComponent<Rigidbody2D>();
                    joint.frequency = frequencyJoints;
                    joint.breakForce = forceToBreak;
                    joint.breakAction = JointBreakAction2D.Disable;
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
                lineRenderers[i].SetPosition(0, atomsTransform.position);
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
                    SpringJoint2D[] currentJoint = atomsTransform.GetComponents<SpringJoint2D>();

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

    private void DestroyLineRenderers()
    {
        SpringJoint2D[] currentJoint = atomsTransform.GetComponents<SpringJoint2D>();

        foreach (SpringJoint2D joint in currentJoint)
        {
            if (!joint.enabled)
            {
                for (int i = 0; i < maxLinkNumber; i++)
                {
                    lineRenderers[i].positionCount = minLinkNumber;
                }
            }
        }
    }
}
