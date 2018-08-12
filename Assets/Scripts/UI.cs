using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private GameObject gameoverUIElementsContainer;

    private void Start() {
        Game.OnInputValidationEvent += OnMovementValidation;
        Game.OnGameOverEvent += OnGameOver;
    }

    private void OnMovementValidation(bool isValidated) {
        scoreText.text = "Score: " + Game.TotalScore;
        livesText.text = "Lives: " + Game.Instance.Lives;
    }

    private void OnGameOver() {
        gameoverUIElementsContainer.SetActive(true);
    }

    private void OnDestroy() {
        Game.OnInputValidationEvent -= OnMovementValidation;
        Game.OnGameOverEvent -= OnGameOver;
    }
}
