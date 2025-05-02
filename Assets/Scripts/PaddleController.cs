using UnityEngine;

/// <summary>
/// Locks the paddle to the mouse cursor in world space.
/// Applies force to the puck on collision based on paddle movement velocity,
/// simulating an air hockey-style hit.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PaddleController : MonoBehaviour
{
    [Header("Hit Force Settings")]
    [Tooltip("Multiplier for the force applied to the puck on collision.")]
    public float hitForceMultiplier = 10f;

    private Rigidbody2D rb;
    private Vector2 lastPosition;
    private Vector2 currentVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 currentPosition = mouseWorldPos;
        currentVelocity = (currentPosition - lastPosition) / Time.deltaTime;
        lastPosition = currentPosition;

        transform.position = currentPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Puck"))
        {
            Rigidbody2D puckRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (puckRb != null)
            {
                puckRb.AddForce(currentVelocity * hitForceMultiplier, ForceMode2D.Impulse);
            }
        }
    }
}
