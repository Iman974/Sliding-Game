using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] private Text scoreText;
    [SerializeField] private Image directionImg;
    [SerializeField] private AnimationCurve slidingAnimation = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float slideSpeed = 1f;
    [SerializeField] private float slideDistance = 5f;

    public void OnMovementValidation(bool isValidated, int scoreValue) {
        scoreText.text = "Score: " + scoreValue;
        StartCoroutine(SlideArrow());

        if (!isValidated) {
            directionImg.color = Color.red;
        }
    }

    public void OnNext() {
        float rotation = 0;
        SlideDirection newDirection = GameManager.CurrentDirection;

        if (newDirection == SlideDirection.Right) {
            rotation = 90f;
        } else if (newDirection == SlideDirection.Down) {
            rotation = 180f;
        } else if (newDirection == SlideDirection.Left) {
            rotation = 270f;
        }

        StopAllCoroutines();
        directionImg.transform.rotation = Quaternion.Euler(0f, 0f, 90f - rotation);
        directionImg.rectTransform.anchoredPosition = Vector2.zero;
        directionImg.color = Color.white;
    }

    private IEnumerator SlideArrow() {
        Vector2 slideDirection = GameManager.DirectionToVector(GameManager.CurrentDirection) * slideDistance;
        Vector2 startPosition = directionImg.rectTransform.anchoredPosition;

        for (float time = 0f; time < 1f; time += slideSpeed * Time.deltaTime) {
            directionImg.rectTransform.anchoredPosition += Vector2.Lerp(startPosition, slideDirection, slidingAnimation.Evaluate(time));
            yield return null;
        }
    }

    private void Start() {
        GameManager.ValidationEvent += OnMovementValidation;
        GameManager.NextEvent += OnNext;
    }

    private void OnDestroy() {
        GameManager.ValidationEvent -= OnMovementValidation;
        GameManager.NextEvent -= OnNext;
    }
}
