using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A debug/developer UI for tweaking game mode, resetting scores, and inspecting runtime data.
/// Requires a Canvas with UI elements and proper linkage.
/// </summary>
public class GodModeUI : MonoBehaviour
{
    [Header("References")]

    [Tooltip("GameManager reference.")]
    public GameManager gameManager;

    [Tooltip("Toggle to switch between game modes.")]
    public Toggle modeToggle;

    [Tooltip("Button to reset the game.")]
    public Button resetButton;

    [Tooltip("Text field to show scores.")]
    public Text scoreDisplay;

    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

        modeToggle.onValueChanged.AddListener(OnModeToggleChanged);
        resetButton.onClick.AddListener(OnResetClicked);
    }

    void Update()
    {
        if (scoreManager != null && scoreDisplay != null)
        {
            scoreDisplay.text = $"P1: {scoreManager.player1Score} | P2: {scoreManager.player2Score}";
        }
    }

    private void OnModeToggleChanged(bool isPingPong)
    {
        if (gameManager != null)
        {
            gameManager.SetGameMode(isPingPong
                ? GameManager.GameMode.PingPong
                : GameManager.GameMode.Squash);
        }
    }

    private void OnResetClicked()
    {
        gameManager?.ResetGame();
    }
}
