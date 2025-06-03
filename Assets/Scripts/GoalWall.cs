using System;
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

    //----------------------------------------------------------------

    [Header("Wall's Current Mode of Operation")]

    [Tooltip("Set the behavior of this GoalWall.")]
    public WallMode wallMode = WallMode.Squash;   // Defaults to Ping Pong Mode, but can be set in the Inspector using a dropdown.

    //----------------------------------------------------------------

    [Header("Object References")]

    [Tooltip("Reference to the puck GameObject.")]
    public GameObject puck;

    [Tooltip("Physics Material 2D to control bounce behavior.")]
    public PhysicsMaterial2D wallMaterial;
    public Collider2D wallCollider;
    public SpriteRenderer wallSpriteRenderer;
    public GoalWall otherGoalWall; // Reference to the other goal wall (for Ping Pong mode)
    private ScoreManager scoreManager;


    //----------------------------------------------------------------

    [Header("Player Association")]

    [Tooltip("If true, this wall belongs to Player 1. If false, Player 2.")]
    public bool isPlayer1Wall = true;

    //----------------------------------------------------------------
    [Header("Respawn Settings")]
    [Tooltip("Time in seconds to wait before respawning the puck.")]
    public float respawnTime = 1f;

    [Tooltip("Transform to respawn the puck at (used in Ping Pong mode).")]
    public Transform puckRespawnPoint;

    // Start() ---------------------------------------------------------
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        SetWallMode(wallMode); // Set the initial game mode

    }

    // In Squash Mode -------------------------------------------

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Puck")) return; // Only Puck collisions are relevant
        if (wallMode == WallMode.Squash)
        {
                int scoringPlayer = isPlayer1Wall ? 2 : 1;
                scoreManager.AddPoint(scoringPlayer);
        }
    }

    // In Ping Pong Mode ----------------------------------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Puck")) return;        // Only Puck collisions are relevant

        if (wallMode == WallMode.PingPong)
        {
            if (otherGoalWall.wallMode == WallMode.PingPong)
            {
                int scoringPlayer = isPlayer1Wall ? 2 : 1;
                scoreManager.AddPoint(scoringPlayer);

                scoreManager.CheckWinCondition(); // Check for win condition after scoring
                scoreManager.PrintScores(false);
                StartCoroutine(RespawnPuck());
            }
            else if (otherGoalWall.wallMode == WallMode.Squash)
            {
                scoreManager.PrintScores(true);
                scoreManager.UpdateHighScores();
                scoreManager.ResetScores();

                StartCoroutine(RespawnPuck());
            }

        }
    }

    // Respawn the puck after a delay ---------------------------
    private System.Collections.IEnumerator RespawnPuck()
    {
        puck.SetActive(false);
        yield return new WaitForSeconds(respawnTime);

        puck.transform.position = puckRespawnPoint.position;
        puck.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        puck.SetActive(true);
    }

    // Called in Start() to set up wall properties based on the game mode -----------------
    public void SetWallMode(WallMode mode)
    {
        //Debug.Log("Setting" + this.name + "'s mode to: " + mode);
        // Settings for Squash vs. Ping Pong mode
        if (mode == WallMode.Squash)
        {
            scoreManager.ResetScores();
            wallCollider.isTrigger = false; // Ensure collider is not a trigger so that ball bounces
            wallCollider.sharedMaterial = wallMaterial; // Set the physics material for squash mode
            wallSpriteRenderer.color = Color.white; // Set color to white for squash mode to visually be a sprite // CHANGE WHEN SWITCHING TO GRAPHICS
        }
        else if (mode == WallMode.PingPong)
        {
            scoreManager.ResetScores();
            wallCollider.isTrigger = true; // Ensure collider is a trigger so that ball passes through
            wallCollider.sharedMaterial = null; // Remove physics material for ping pong mode
            wallSpriteRenderer.color = Color.black; // Set color to red for ping pong mode to visually be a sprite // CHANGE WHEN SWITCHING TO GRAPHICS
        }
    }

    
}
