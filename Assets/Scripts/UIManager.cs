using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] private Text scoreText;
    [SerializeField] private Image directionImg;

    [Header("Animations")]
    [SerializeField] private AnimationCurve slidingAnimation = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float slideSpeed = 1f;
    [SerializeField] private float slideDistance = 100f;
    [SerializeField] [Range(0.01f, 1f)] private float fadeOutSpeed = 0.2f;
    [SerializeField] [Min(1.01f)] private float fadeInSpeed = 1.39f;
    [SerializeField] [Range(0f, 1f)] private float fadeInStart = 0.4f;
    [SerializeField] [Range(0.01f, 1f)] private float scaleDownSpeed = 0.2f;

    private Vector3 initialArrowScale;

    public void OnMovementValidation(bool isValidated, int scoreValue) {
        scoreText.text = "Score: " + scoreValue;
        StartCoroutine(SlideArrow());
        StartCoroutine(FadeArrow(true));

        if (!isValidated) {
            directionImg.color = Color.red;
        } else {
            StartCoroutine(ScaleDownArrow());
            directionImg.color = Color.green;
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
        directionImg.color = new Color(1f, 1f, 1f, fadeInStart);
        directionImg.transform.localScale = initialArrowScale;
        StartCoroutine(FadeArrow(false));
    }

    private IEnumerator SlideArrow() {
        Vector2 slideDirection = GameManager.DirectionToVector(GameManager.CurrentDirection) * slideDistance;
        Vector2 startPosition = directionImg.rectTransform.anchoredPosition;

        for (float time = 0f; time < 1f; time += slideSpeed * Time.deltaTime) {
            directionImg.rectTransform.anchoredPosition += Vector2.Lerp(startPosition, slideDirection, slidingAnimation.Evaluate(time));
            yield return null;
        }
    }

    private IEnumerator FadeArrow(bool isFadeOut) {
        float fadeMultiplier = isFadeOut ? fadeOutSpeed : fadeInSpeed;
        var evaluator = isFadeOut ? new Func<float, bool>(alpha => alpha > 0.01f) : new Func<float, bool>(alpha => alpha < 1f);

        while (evaluator(directionImg.color.a)) {
            Color arrowColor = directionImg.color;
            directionImg.color = new Color(arrowColor.r, arrowColor.g, arrowColor.b, arrowColor.a * fadeMultiplier);
            yield return null;
        }
    }

    private IEnumerator ScaleDownArrow() {
        Vector3 scale = directionImg.transform.localScale;

        while (scale.x > 0f && scale.y > 0f) {
            scale.Set(scale.x * scaleDownSpeed, scale.y * scaleDownSpeed, 1f);
            directionImg.transform.localScale = scale;
            yield return null;
        }
    }

    private void Awake() {
        fadeOutSpeed = 1f - fadeOutSpeed;
        scaleDownSpeed = 1f - scaleDownSpeed;
        initialArrowScale = directionImg.transform.localScale;
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
