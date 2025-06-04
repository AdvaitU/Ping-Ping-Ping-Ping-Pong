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

    private float yMin = 0.0f;
    private float yMax = 5.0f;

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
                puckRb.AddForce(currentVelocity + new Vector2(0, 1) * hitForceMultiplier, ForceMode2D.Impulse);  // Adding new Vector2(1, 1) to avoid zero vector
                
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

        Vector2 blobPos = openCV.blobPosition;       // Take blob position from OpenCV ContourFinder script
        float blobSize = openCV.blobSizeNormalised;                // Take blob size from OpenCV ContourFinder script
        if (blobPos == Vector2.zero) return;         // If no blob is detected, do not update position 

        Vector2 centrePt = new Vector2(0.5f, 0.5f);         // Center of the screen in normalized coordinates
        DiagonalQuadrant quadrant = GetQuadrantByAngle(blobPos.x, blobPos.y, 1.0f, 1.0f);

        //Debug.Log("The Blob is in the " + quadrant + " quadrant.");

        Vector2 currentPosition = transform.position;       // Make a copy of the current position
        float yAlignment = MapRange(blobSize, 0.0f, 1.0f, yMin, yMax); // Map blob size to y position (0.0f to 5.0f)

        //Switch-Case using quadrant to determine the position of the paddle
        switch (quadrant)
        {
            case DiagonalQuadrant.Top:
                currentPosition = new Vector2(40.0f - (blobPos.x * 10.0f), yAlignment); // Align to ceiling (30.0f to 40.f) --> Using (40.0f - x) to invert it
                break;
            case DiagonalQuadrant.Left:
                currentPosition = new Vector2(10.0f - (blobPos.y * 10.0f), yAlignment); // Align to Left Wall
                break;
            case DiagonalQuadrant.Bottom:
                currentPosition = new Vector2(10.0f + (blobPos.x * 10.0f), yAlignment); // Align to ceiling
                break;
            case DiagonalQuadrant.Right:
                currentPosition = new Vector2(20.0f + (blobPos.y * 10.0f), yAlignment); // Align to Right Wall
                break;
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


    // For quadrant calculation
    public enum DiagonalQuadrant
    {
        Top,
        Left,
        Bottom,
        Right
    }

    public static DiagonalQuadrant GetQuadrantByAngle(float x, float y, float screenWidth, float screenHeight)
    {
        float centerX = screenWidth / 2f;
        float centerY = screenHeight / 2f;

        float dx = x - centerX;
        float dy = y - centerY;

        float angleDeg = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        if (angleDeg < 0) angleDeg += 360f;

        if (angleDeg >= 45f && angleDeg < 135f)
            return DiagonalQuadrant.Top;
        else if (angleDeg >= 135f && angleDeg < 225f)
            return DiagonalQuadrant.Left;
        else if (angleDeg >= 225f && angleDeg < 315f)
            return DiagonalQuadrant.Bottom;
        else
            return DiagonalQuadrant.Right;
    }



}
