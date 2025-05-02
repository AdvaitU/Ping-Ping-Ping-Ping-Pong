using UnityEngine;

/// <summary>
/// Manages game mode (Squash or Ping Pong), central game state, and win condition checking.
/// Use this to sync all components (GoalWalls, ScoreManager, etc.).
/// </summary>
public class GameManager : MonoBehaviour
{
    public enum GameMode { Squash, PingPong }

    [Header("Game Mode")]

    [Tooltip("Current game mode.")]
    public GameMode currentMode = GameMode.PingPong;

    [Header("References")]

    [Tooltip("Reference to the ScoreManager.")]
    public ScoreManager scoreManager;

    [Tooltip("All goal walls in the scene.")]
    public GoalWall[] goalWalls;

    void Start()
    {
        UpdateAllGoalWalls();
    }

    public void SetGameMode(GameMode newMode)
    {
        currentMode = newMode;
        UpdateAllGoalWalls();
    }

    private void UpdateAllGoalWalls()
    {
        foreach (var wall in goalWalls)
        {
            wall.mode = (currentMode == GameMode.PingPong)
                ? GoalWall.WallMode.PingPong
                : GoalWall.WallMode.Squash;
        }
    }

    public void ResetGame()
    {
        if (scoreManager != null)
        {
            scoreManager.ResetScores();
        }

        foreach (var wall in goalWalls)
        {
            wall.squashHitCount = 0;
        }
    }
}
