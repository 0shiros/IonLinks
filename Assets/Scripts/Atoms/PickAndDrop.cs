using System;
using System.Collections.Generic;
using UnityEngine;

public class PickAndDrop : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lerpTime = 0.1f;
    public Vector2 mousePosition;
    public bool isPicking = false;
    public bool isLocked = false;
    
    [Header("References")]
    public Collider2D atomCollider;
    private Rigidbody2D atomRigidbody;
    public CreateLinks createLinks;
    private Rigidbody2D rigidBody;
    private List<GameObject> allAtoms;

    private void Start()
    {
        atomRigidbody = transform.GetComponent<Rigidbody2D>();
        atomCollider = transform.GetComponent<Collider2D>();
        createLinks = GetComponentInChildren<CreateLinks>();
        rigidBody = GetComponent<Rigidbody2D>();
        allAtoms = GetComponentInParent<AtomSpawner>().atoms;
    }

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        AtomDetection();
    }

    private void FixedUpdate()
    {
        Pick();
    }

    private void AtomDetection()
    {
        if (Input.GetMouseButtonDown(0) && !isLocked)
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider == atomCollider)
            {
                isPicking = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPicking = false;
            atomRigidbody.excludeLayers = default;
        }
    }


    private void Pick()
    {
        if (isPicking) 
        {
             atomRigidbody.linearVelocity = Vector2.zero;
             atomRigidbody.MovePosition(Vector2.Lerp(transform.position, mousePosition, lerpTime));
             atomRigidbody.excludeLayers = LayerMask.GetMask("Atom");
        }
    }

    private void OnEnable()
    {
        CreateLinks.breakLinks += BreakLinks;
    }

    private void OnDisable()
    {
        CreateLinks.breakLinks -= BreakLinks;
    }

    public void BreakLinks(Rigidbody2D rb)
    {
        foreach (SpringJoint2D joint in createLinks.currentJoints)
        {
            if (joint.connectedBody == rb)
            {
                joint.enabled = false;
            }
        }

        if (!HasAnyActiveLink())
        {
            isLocked = false;
        }
    }

    private bool HasAnyActiveLink()
    {
        foreach (SpringJoint2D joint in createLinks.currentJoints)
        {
            if (joint.enabled && joint.connectedBody != null)
                return true;
        }
            
        foreach (GameObject atom in allAtoms)
        {
            if (atom == this.gameObject) continue;

            PickAndDrop atomPickAndDrop = atom.GetComponent<PickAndDrop>();
            
            foreach (SpringJoint2D joint in atomPickAndDrop.createLinks.currentJoints)
            {
                if (joint.enabled && joint.connectedBody == rigidBody)
                    return true;
            }
        }

        return false;
    }
}
