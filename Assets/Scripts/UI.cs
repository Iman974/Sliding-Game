using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    [SerializeField] private Text scoreText;

    private void Start() {
        Game.OnInputValidationEvent += OnMovementValidation;
    }

    private void OnMovementValidation(bool isValidated, int scoreValue) {
        scoreText.text = "Score: " + scoreValue;
    }

    private void OnDestroy() {
        Game.OnInputValidationEvent -= OnMovementValidation;
    }
}
