using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Tracks and manages player scores and win conditions.
/// Use this in conjunction with GoalWall.cs to update score.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [Header("Score Limits")]

    [Tooltip("Points needed to win in Ping Pong mode.")]
    public int winScorePingPong = 7;


    //----------------------------------------------------------------
    public int player1Score = 0;
    public int player2Score = 0;

    public int[] squashHighScores = new int[10];


    //----------------------------------------------------------------
    public void AddPoint(int playerNumber)
    {
        if (playerNumber == 1)
        {
            player1Score++;
        }
        else if (playerNumber == 2)
        {
            player2Score++;
        }

        //CheckWinCondition();
    }

    // ----------------------------------------------------------------
    // Called from GoalWall when a player scores only
    public void CheckWinCondition() 
    {
        if ((player1Score >= (winScorePingPong - 1)) && (player2Score >= (winScorePingPong - 1))) // If both scores are at 6
        {
            Debug.Log("Deuce! First to score 2 points in a row wins");
            //(player1Score == player2Score) && 
            if (player1Score - player2Score == 2) // If player 1 scores 2 points in a row
            {
                Debug.Log("Player 1 Wins!");
            }
            else if (player2Score - player1Score == 2) // If player 2 scores 2 points in a row
            {
                Debug.Log("Player 2 Wins!");
            }
            else return; // No one has won yet

        }
        else if (player1Score >= winScorePingPong)                 // If player 1 hits 7 with p2 not at 6
        {
            Debug.Log("Player 1 Wins!");
        }
        else if (player2Score >= winScorePingPong)                 // If player 1 hits 7 with p2 not at 6
        {
            Debug.Log("Player 2 Wins!");
        }
        else return; // No one has won yet

        ResetScores();  // Will only happen if a player wins
    }

    public void PrintScores(bool isSquashMode)
    {
        if (isSquashMode)
        {
            Debug.Log("Score: " + player1Score);
        }
        else
        {
            Debug.Log("Score: " + player1Score + " : " + player2Score + " (Player 1 : Player 2).");
        }
    }

    public void UpdateHighScores()
    {
        // Update high scores for squash mode
        for (int i = 0; i < squashHighScores.Length; i++)
        {
            if (player1Score > squashHighScores[i])
            {
                // Shift lower scores down
                for (int j = squashHighScores.Length - 1; j > i; j--)
                {
                    squashHighScores[j] = squashHighScores[j - 1];
                }
                squashHighScores[i] = player1Score;
                break;
            }
        }
        // Save high scores to PlayerPrefs or a file if needed
    }

    public void ResetScores()
    {
        player1Score = 0;
        player2Score = 0;
    }


}

