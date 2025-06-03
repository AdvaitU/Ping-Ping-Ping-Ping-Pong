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
    public bool openCVControlled = false;       // If true, paddle follows mouse cursor
    public ContourFinder openCV;

    private float margin = 0.3f; // Margin for paddle to be in to snap to an edge
    private float topMargin = 0.5f; // Top is a special case as it will need a larger reach
    private float yAlignment = 5.0f; // Y position to align the paddle to regardless of x

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
        if (!openCVControlled)
        {
            UseMouseTracking();
        }
        else
        {
            UseCVTracking();
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
        Debug.Log("Cursor Position: " + currentPosition);

        transform.position = currentPosition;
    }

    private void UseCVTracking()
    {   

        // ------------------------------------------------------------------------------------------------
        Vector2 blobPos = openCV.blobPosition;       // Take blob position from OpenCV ContourFinder script
        // ------------------------------------------------------------------------------------------------\

        Vector2 currentPosition = transform.position;       // Make a copy of the current position

        if (blobPos.y <= margin)                                      // Aligned to bottom
        {
            currentPosition = new Vector2(MapRange(blobPos.x, 0.0f, 1.0f, 10.0f, 20.0f), yAlignment); // Align to floor
        }
        else if (blobPos.y >= 1.0f - topMargin)                             // Aligned to top  --> special topMargin usage
        {
            currentPosition = new Vector2(MapRange(blobPos.x, 0.0f, 1.0f, 30.0f, 40.0f), yAlignment); // Align to ceiling
        }
        else if (blobPos.x <= margin)                                  // Aligned to left
        {
            currentPosition = new Vector2(MapRange(blobPos.y, 0.0f, 1.0f, 0.0f, 10.0f), yAlignment); // Align to Left Wall
        }
        else if (blobPos.x >= 1.0f - margin)                           // Aligned to right
        {
            currentPosition = new Vector2(MapRange(blobPos.y, 0.0f, 1.0f, 20.0f, 30.0f), yAlignment); // Align to Right Wall
        }
        else                                                            // In the middle
        {
            // Do nothing
        }

        currentVelocity = (currentPosition - lastPosition) / Time.deltaTime; // Calculate velocity
        lastPosition = currentPosition; // Update last position
        //Debug.Log("Blob Position: " + blobPos + " | Current Position: " + currentPosition);

        transform.position = currentPosition; // Set the paddle position to the calculated position


    }

    // C# Equivalent of Arduino's map() function
    public static float MapRange(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }

}
