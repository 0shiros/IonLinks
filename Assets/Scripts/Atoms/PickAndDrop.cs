using System;
using UnityEngine;

public class PickAndDrop : MonoBehaviour
{
    [SerializeField] private float lerpTime = 0.1f;
    public Vector2 mousePosition;
    public Collider2D atomCollider;
    private Rigidbody2D atomRigidbody;
    public bool isPicking = false;
    public bool isLocked = false;

    private void Start()
    {
        atomRigidbody = transform.GetComponent<Rigidbody2D>();
        atomCollider = transform.GetComponent<Collider2D>();
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

    private void BreakLinks(Rigidbody2D rb)
    {
        SpringJoint2D[] joints = GetComponents<SpringJoint2D>();
        int count = 0;
        foreach (SpringJoint2D joint in joints)
        {
            if (rb == joint.connectedBody)
            {
                joint.enabled = false;
                
            }

            if (joint.enabled)
            {
                count++;
            }
        }

        if (count < 1)
        {
            isLocked = false;
        }
    }
}
