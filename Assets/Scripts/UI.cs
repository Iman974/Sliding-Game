using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private GameObject gameoverUIElementsContainer;

    private void Start() {
        //highScoreText.text = "Highscore: " + GameManager.HighScore;
    }

    private void OnMovementValidation(bool isValidated) {
        UpdateStatsText();
    }

    private void UpdateStatsText() {
        scoreText.text = "Score: " + GameManager.PlayerScore;
        livesText.text = "Lives: " + GameManager.Instance.Lives;
    }

    private void OnGameOver() {
        gameoverUIElementsContainer.SetActive(true);
        //highScoreText.text = "Highscore: " + GameManager.HighScore;
    }

    private void OnGameReset() {
        UpdateStatsText();
    }
}
