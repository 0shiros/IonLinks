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
        }
    }


    private void Pick()
    {
        if (isPicking) 
        {
             atomRigidbody.linearVelocity = Vector2.zero;
             atomRigidbody.MovePosition(Vector2.Lerp(transform.position, mousePosition, lerpTime));
        }
    }
}
