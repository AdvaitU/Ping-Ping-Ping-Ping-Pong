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
    public bool mouseControlled = true;       // If true, paddle follows mouse cursor

    private Rigidbody2D rb;
    private Vector2 lastPosition;
    private Vector2 currentVelocity;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.isKinematic = true;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (mouseControlled)
        {
            UseMouseTracking();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Puck"))
        {
            animator.SetBool("isSwatting", true);
            Rigidbody2D puckRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (puckRb != null)
            {
                puckRb.AddForce(currentVelocity * hitForceMultiplier, ForceMode2D.Impulse);  // Adding new Vector2(1, 1) to avoid zero vector
                
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Puck"))
        {
            animator.SetBool("isSwatting", false);
        }
    }

    /// <summary> 
    /// Mouse tracking method to follow the mouse cursor in 2D space.
    private void UseMouseTracking()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 currentPosition = mouseWorldPos;
        currentVelocity = (currentPosition - lastPosition) / Time.deltaTime;
        lastPosition = currentPosition;

        transform.position = currentPosition;
    }

    private void UseCVTracking()
    {
        // Placeholder for future computer vision tracking implementation
        // This method can be expanded to use computer vision data to control the paddle position.
    }
}
