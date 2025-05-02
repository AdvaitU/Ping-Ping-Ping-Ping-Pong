using UnityEngine;

/// <summary>
/// Teleports the puck to the opposite edge while preserving velocity and Y position.
/// Attach this to side edge triggers and assign the direction.
/// </summary>
public class EdgeTeleporter : MonoBehaviour
{
    public enum Edge { Left, Right }

    [Tooltip("Which edge this teleporter represents.")]
    public Edge edgeType;

    [Tooltip("X-position to teleport the puck to.")]
    public float teleportX; // Set to far side’s X-position

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Puck"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            Vector2 velocity = rb.velocity;
            Vector2 newPosition = new Vector2(teleportX, other.transform.position.y);

            other.transform.position = newPosition;
            rb.velocity = velocity;
        }
    }
}
