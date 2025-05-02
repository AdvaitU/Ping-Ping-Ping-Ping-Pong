using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A special wall that behaves differently based on the game mode (Squash or Ping Pong).
/// In Ping Pong mode, awards a point to the opposing player and respawns the puck.
/// In Squash mode, acts like a normal wall but tracks how many times it's hit.
/// Attach this to a wall GameObject with a BoxCollider2D (set as Trigger).
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class GoalWall : MonoBehaviour
{
    public enum WallMode { Squash, PingPong }

    [Header("Wall Mode")]

    [Tooltip("Set the behavior of this GoalWall.")]
    public WallMode mode = WallMode.PingPong;

    [Header("General Settings")]

    [Tooltip("Transform to respawn the puck at (used in Ping Pong mode).")]
    public Transform puckRespawnPoint;

    [Tooltip("Reference to the puck GameObject.")]
    public GameObject puck;

    [Header("Player Association")]

    [Tooltip("If true, this wall belongs to Player 1. If false, Player 2.")]
    public bool isPlayer1Wall = true;

    [Header("Squash Mode Score")]

    [Tooltip("Score count (only used in Squash mode).")]
    public int squashHitCount = 0;

    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

        if (!GetComponent<Collider2D>().isTrigger)
        {
            Debug.LogWarning("GoalWall collider must be marked as Trigger!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Puck")) return;

        if (mode == WallMode.PingPong)
        {
            if (scoreManager != null)
            {
                int scoringPlayer = isPlayer1Wall ? 2 : 1;
                scoreManager.AddPoint(scoringPlayer);
            }

            StartCoroutine(RespawnPuck());
        }
        else if (mode == WallMode.Squash)
        {
            squashHitCount++;
            Debug.Log("Squash Hits: " + squashHitCount);

            // In squash mode, act like a bounce wall — add velocity manually if needed
            Rigidbody2D puckRb = other.GetComponent<Rigidbody2D>();
            if (puckRb != null)
            {
                puckRb.velocity = new Vector2(-puckRb.velocity.x, puckRb.velocity.y);
            }
        }
    }

    private System.Collections.IEnumerator RespawnPuck()
    {
        puck.SetActive(false);
        yield return new WaitForSeconds(1f);

        puck.transform.position = puckRespawnPoint.position;
        puck.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        puck.SetActive(true);
    }
}
