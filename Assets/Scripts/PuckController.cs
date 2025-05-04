using UnityEngine;

/// <summary>
/// Controls the puck's physics in the game. Handles movement, friction, speed normalization, 
/// and teleportation across boundaries. Requires a Rigidbody2D.
/// Attach this to the puck GameObject. Assign Rigidbody2D via the inspector.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PuckController : MonoBehaviour
{
    [Header("Physics Tuning")]

    [Tooltip("Drag applied every frame to reduce velocity. Defaults to 0.1")]
    public float drag = 0.1f;

    [Tooltip("Maximum allowed puck speed before clamping. Defaults to 15, 20 -25 for the game.")]
    public float maxSpeed = 15f;

    [Tooltip("Minimum speed below which puck will be nudged.")]
    public float minSpeed = 1f;

    [Header("Warp Settings")]

    [Tooltip("X position of the left edge of area A.")]
    public float leftBoundaryX = 0f;

    [Tooltip("X position of the right edge of area D.")]
    public float rightBoundaryX = 40f;

    [Tooltip("Small vertical offset to avoid z-fighting on teleport.")]
    public float warpYOffset = 0.01f;

    private Rigidbody2D rb;

    [Header("Animation Parameters")]

    [Tooltip("Reference to Animator")]
    public Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = rb.GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle") || collision.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("Collision with Paddle or Wall detected.");
            animator.SetBool("isBouncing", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle") || collision.gameObject.CompareTag("Wall"))
        {
            // Stop the animation
            animator.SetBool("isBouncing", false);
        }
    }

    void FixedUpdate()
    {
        ApplyDrag();
        NormalizeSpeed();
        HandleWarp();
    }

    private void ApplyDrag()
    {
        rb.velocity *= (1f - drag * Time.fixedDeltaTime);
    }

    private void NormalizeSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        else if (rb.velocity.magnitude < minSpeed)
        {
            rb.velocity = rb.velocity.normalized * minSpeed;
        }
    }

    private void HandleWarp()
    {
        Vector2 pos = transform.position;

        if (pos.x <= leftBoundaryX)
        {
            transform.position = new Vector2(rightBoundaryX, pos.y + warpYOffset);
        }
        else if (pos.x >= rightBoundaryX)
        {
            transform.position = new Vector2(leftBoundaryX, pos.y + warpYOffset);
        }
    }
}
