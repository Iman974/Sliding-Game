using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private GameObject gameoverUIElementsContainer;

    private void Start() {
        Game.OnInputValidationEvent += OnMovementValidation;
        Game.OnGameOverEvent += OnGameOver;
        Game.OnGameResetEvent += OnGameReset;
    }

    private void OnMovementValidation(bool isValidated) {
        UpdateStatsText();
    }

    private void UpdateStatsText() {
        scoreText.text = "Score: " + Game.TotalScore;
        livesText.text = "Lives: " + Game.Instance.Lives;
    }

    private void OnGameOver() {
        gameoverUIElementsContainer.SetActive(true);
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
