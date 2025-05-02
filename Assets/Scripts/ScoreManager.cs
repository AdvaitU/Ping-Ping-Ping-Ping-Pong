using UnityEngine;

/// <summary>
/// Tracks and manages player scores and win conditions.
/// Use this in conjunction with GoalWall.cs to update score.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [Header("Score Limits")]

    [Tooltip("Points needed to win in Ping Pong mode.")]
    public int winScore = 7;

    [Header("Scores")]

    [Tooltip("Player 1's score.")]
    public int player1Score = 0;

    [Tooltip("Player 2's score.")]
    public int player2Score = 0;

    public void AddPoint(int playerNumber)
    {
        if (playerNumber == 1)
        {
            player1Score++;
            Debug.Log("Player 1 Score: " + player1Score);
        }
        else if (playerNumber == 2)
        {
            player2Score++;
            Debug.Log("Player 2 Score: " + player2Score);
        }

        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (player1Score >= winScore)
        {
            Debug.Log("Player 1 Wins!");
        }
        else if (player2Score >= winScore)
        {
            Debug.Log("Player 2 Wins!");
        }
    }

    public void ResetScores()
    {
        player1Score = 0;
        player2Score = 0;
    }
}

