using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateLinks : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float radiusAtomDetection;
    public int quantityOfLinkGenerate;
    public float frequencyJoints = 10;
    public float forceToBreak = 400;
    public bool canCreateLink = false;
    public bool isMagnetised = false;

    [Header("References")]
    public PickAndDrop pickAndDrop;
    public Transform atomsTransform;
    private Collider2D atomCollider;
    private Rigidbody2D atomRigidbody2D;
    public List<Collider2D> nearestHits;
    public List<SpringJoint2D> currentJoints = new();
    public List<LineRenderer> lineRenderers;
    
    [Header("Events")]
    public static Action<Rigidbody2D> breakLinks;
        
    private void Start()
    {
        atomsTransform = transform.parent;
        pickAndDrop = atomsTransform.GetComponent<PickAndDrop>();
        atomCollider = atomsTransform.GetComponent<Collider2D>();
        atomRigidbody2D = atomsTransform.GetComponent<Rigidbody2D>();
        CreateSpringJoints2D();
        lineRenderers = new(GetComponentsInChildren<LineRenderer>());
    }

    private void Update()
    {
        AtomsNextDetection();
        LockAtom();
        UnlockAtomWithMouse();
        LinksCurrentPositions();
        DestroyLineRenderers();
    }

    private void CreateSpringJoints2D()
    {
        for (int i = 0; i < quantityOfLinkGenerate; i++)
        {
            SpringJoint2D joint2D = atomsTransform.gameObject.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;
            joint2D.enabled = false;
            joint2D.frequency = frequencyJoints;
            joint2D.breakForce = forceToBreak;
            joint2D.breakAction = JointBreakAction2D.Disable;
            currentJoints.Add(joint2D);
        }
    }

    List<Collider2D> FindNearestHits(Transform atomPicked, List<Collider2D> hits)
    {
        return hits.OrderBy(hitPoint => Vector3.Distance(atomPicked.transform.position, hitPoint.transform.position))
            .Take(2).ToList();
    }

    private void AtomsNextDetection()
    {
        if (pickAndDrop.isPicking)
        {
            List<Collider2D> hitResults = new(Physics2D.OverlapCircleAll(atomsTransform.position, radiusAtomDetection, layerMask));
            hitResults.Remove(atomCollider);

            List<Collider2D> filteredHits = new();

            for (int i = 0; i < hitResults.Count; i++)
            {
                PickAndDrop hitPickAndDrop = hitResults[i].GetComponent<PickAndDrop>();
                
                if (hitPickAndDrop.isLocked)
                {
                    filteredHits.Add(hitResults[i]);
                }
            }

            nearestHits = FindNearestHits(atomsTransform, filteredHits);

            canCreateLink = (nearestHits.Count == 2);

            if (filteredHits.Count < 2)
            {
                filteredHits.Clear();
                nearestHits.Clear();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (atomsTransform != null && pickAndDrop.isPicking)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(atomsTransform.position, radiusAtomDetection);
        }
    }

    private void LockAtom()
    {
        if (canCreateLink && !pickAndDrop.isPicking)
        {
            for (int i = 0; i < 2; i++)
            {
                if (currentJoints[i] == null) return;
                currentJoints[i].connectedBody = nearestHits[i].GetComponent<Rigidbody2D>();
                currentJoints[i].enabled = true;
                
                PickAndDrop jointPickAndDrop = nearestHits[i].GetComponent<PickAndDrop>();
                jointPickAndDrop.isLocked = true;
                
                lineRenderers[i].positionCount = 2;
            }
            
            pickAndDrop.isLocked = true;
            canCreateLink = false;
        }
    }

    private void LinksCurrentPositions()
    {
        for (int i = 0; i < quantityOfLinkGenerate; i++)
        {
            if (lineRenderers[i].positionCount == 2)
            {
                lineRenderers[i].SetPosition(0, atomsTransform.position);
                lineRenderers[i].SetPosition(1, nearestHits[i].transform.position);
            }
        }
    }

    public void UnlockAtom()
    {
        canCreateLink =  false;
        pickAndDrop.isLocked = false;
        
        foreach (SpringJoint2D joint in currentJoints)
        {
            joint.enabled = false;
        }

        for (int i = 0; i < quantityOfLinkGenerate; i++)
        {
            lineRenderers[i].positionCount = 0;
        }
                    
        breakLinks?.Invoke(atomRigidbody2D);
    }

    private void UnlockAtomWithMouse()
    {
        if (pickAndDrop.isLocked)
        {
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit2D hitMouse = Physics2D.Raycast(pickAndDrop.mousePosition, Vector2.zero);

                if (hitMouse.collider == pickAndDrop.atomCollider)
                {
                   UnlockAtom();
                }
            }
        }
    }

    public void DestroyLineRenderers()
    {
        Dictionary<SpringJoint2D, LineRenderer> links = new();

        if (currentJoints.Count != lineRenderers.Count)
        {   
            return;
        }

        for (int i = 0; i < lineRenderers.Count; i++)
        {
            links[currentJoints[i]] = lineRenderers[i];
        }
        
        foreach (var link in links)
        {
            if (!link.Key.enabled)
            {
                link.Value.positionCount = 0;
            }
        }
        
        if (pickAndDrop.isLocked && !isMagnetised)
        {
            if (currentJoints.Count == 0)
            {
                pickAndDrop.isLocked = false;
            }
        }
    }
}
