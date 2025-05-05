using UnityEngine;

/// <summary>
/// Manages game mode (Squash or Ping Pong), central game state, and win condition checking.
/// Use this to sync all components (GoalWalls, ScoreManager, etc.).
/// </summary>
public class GameManager : MonoBehaviour
{
    public enum GameMode { Squash, PingPong, PhysicsTesting , FreeForm }

    [Header("Game Mode")]

    [Tooltip("Current game mode.")]
    public GameMode currentMode = GameMode.FreeForm;

    [Header("References")]

    [Tooltip("All goal walls in the scene.")]
    public GoalWall[] goalWalls = new GoalWall[2];

    void Awake()      // Before First Frame
    {
        switch(currentMode)
        {
            case GameMode.Squash:
                goalWalls[0].wallMode = GoalWall.WallMode.PingPong;
                goalWalls[1].wallMode = GoalWall.WallMode.Squash;
                break;
            case GameMode.PingPong:
                goalWalls[0].wallMode = GoalWall.WallMode.PingPong;
                goalWalls[1].wallMode = GoalWall.WallMode.PingPong;
                break;
            case GameMode.PhysicsTesting:
                goalWalls[0].wallMode = GoalWall.WallMode.Squash;
                goalWalls[1].wallMode = GoalWall.WallMode.Squash;
                break;
            case GameMode.FreeForm:
                // Follow the behaviour set in the Inspector per wall
                break;
            default:
                Debug.LogError("GameManager: Invalid game mode selected.");
                break;
        }
    }


}
