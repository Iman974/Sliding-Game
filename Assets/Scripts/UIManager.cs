using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] private Text scoreText;
    [SerializeField] private Image directionImg;

    public void OnMovementValidation(int scoreValue) {
        scoreText.text = "Score: " + scoreValue;
    }

    private void Start() {
        GameManager.ValidationEvent += OnMovementValidation;
        GameManager.NextEvent += OnNext;
    }

    public void OnNext(GameManager.SlideDirection direction) {
        float rotation = 0;

        if (direction == GameManager.SlideDirection.Right) {
            rotation = 90f;
        } else if (direction == GameManager.SlideDirection.Down) {
            rotation = 180f;
        } else if (direction == GameManager.SlideDirection.Left) {
            rotation = 270f;
        }

        directionImg.transform.rotation = Quaternion.Euler(0f, 0f, 90f - rotation);
    }

    private void OnDestroy() {
        GameManager.ValidationEvent -= OnMovementValidation;
        GameManager.NextEvent -= OnNext;
    }
}
