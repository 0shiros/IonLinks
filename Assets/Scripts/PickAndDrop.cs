using System;
using Unity.VisualScripting;
using UnityEngine;

public class PickAndDrop : MonoBehaviour
{
    [SerializeField] private float lerpTime = 0.1f;
    private Vector2 mousePosition;
    private Collider2D atomCollider;
    private Rigidbody2D atomRigidbody;
    private bool isPicking = false;

    private void Start()
    {
        atomRigidbody = transform.GetComponent<Rigidbody2D>();
        atomCollider = transform.GetComponent<Collider2D>();
    }

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        AtomDetection();
        PickOrDrop();
    }

    private void AtomDetection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            
            if (hit.collider != null && hit.collider == atomCollider)
            {
                isPicking = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
             isPicking = false;
        }
    }

    private void PickOrDrop()
    {
        if (isPicking) 
        {
             atomRigidbody.linearVelocity = Vector2.zero;
             atomRigidbody.MovePosition(Vector2.Lerp(transform.position, mousePosition, lerpTime));
        }
        else
        {
            isPicking = false;
        }
    }
}
