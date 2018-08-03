using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    [SerializeField] private Text scoreText;
    [SerializeField] private Image arrowImg;

    [Header("Animations")]
    [SerializeField] private AnimationCurve slidingAnimation = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float slideSpeed = 1f;
    [SerializeField] private float slideDistance = 100f;
    [SerializeField] [Range(0.01f, 1f)] private float scaleDownSpeed = 0.2f;

    private void Awake() {
        scaleDownSpeed = 1f - scaleDownSpeed;

        Game.OnInputValidationEvent += OnMovementValidation;
    }

    private void OnMovementValidation(bool isValidated, int scoreValue) {
        scoreText.text = "Score: " + scoreValue;
    }

    private IEnumerator SlideArrow() {
        Vector2 slideDirection = DirectionUtility.DirectionToVector(Game.CurrentDirection) * slideDistance;
        Vector2 startPosition = arrowImg.rectTransform.anchoredPosition;

        for (float time = 0f; time < 1f; time += slideSpeed * Time.deltaTime) {
            arrowImg.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, slideDirection, slidingAnimation.Evaluate(time));
            yield return null;
        }
    }

    private IEnumerator ScaleDownArrow() {
        Vector3 scale = arrowImg.transform.localScale;

        while (scale.x > 0f && scale.y > 0f) {
            scale.Set(scale.x * scaleDownSpeed, scale.y * scaleDownSpeed, 1f);
            arrowImg.transform.localScale = scale;
            yield return null;
        }
    }

    private void OnDestroy() {
        Game.OnInputValidationEvent -= OnMovementValidation;
    }
}
