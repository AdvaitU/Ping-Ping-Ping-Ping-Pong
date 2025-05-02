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

    [Tooltip("Drag applied every frame to reduce velocity.")]
    public float drag = 0.1f;

    [Tooltip("Maximum allowed puck speed before clamping.")]
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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
