using UnityEngine;

/// <summary>
/// Teleports the puck from one side of the play area to the opposite side
/// while preserving its velocity and relative Y-position.
/// Attach this script to both left and right portal objects.
/// </summary>
public class PortalTeleporter : MonoBehaviour
{
    [Tooltip("The Transform to teleport the puck to.")]
    public Transform destinationPortal;

    [Tooltip("Should this portal accept teleportation from left or right?")]
    public bool isLeftPortal = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Puck"))
        {
            Rigidbody2D puckRb = other.GetComponent<Rigidbody2D>();
            if (puckRb != null)
            {
                Vector2 velocity = puckRb.velocity;

                // Preserve Y-position, teleport to destination X
                Vector3 newPosition = other.transform.position;
                newPosition.x = destinationPortal.position.x;
                other.transform.position = newPosition;

                // Ensure velocity is preserved
                puckRb.velocity = velocity;
            }
        }
    }
}
