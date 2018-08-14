using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private GameObject gameoverUIElementsContainer;

    private void Start() {
        Game.OnInputValidationEvent += OnMovementValidation;
        Game.OnGameOverEvent += OnGameOver;
        Game.OnGameResetEvent += OnGameReset;

        highScoreText.text = "Highscore: " + Game.HighScore;
    }

    private void OnMovementValidation(bool isValidated) {
        UpdateStatsText();
    }

    private void UpdateStatsText() {
        scoreText.text = "Score: " + Game.PlayerScore;
        livesText.text = "Lives: " + Game.Instance.Lives;
    }

    private void OnGameOver() {
        gameoverUIElementsContainer.SetActive(true);
        highScoreText.text = "Highscore: " + Game.HighScore;
    }

    private void OnGameReset() {
        UpdateStatsText();
    }

    private void OnDestroy() {
        Game.OnInputValidationEvent -= OnMovementValidation;
        Game.OnGameOverEvent -= OnGameOver;
        Game.OnGameResetEvent -= OnGameReset;
    }
}
