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
    //[SerializeField] [Range(0.01f, 1f)] private float fadeOutSpeed = 0.2f;
    //[SerializeField] [Min(1.01f)] private float fadeInSpeed = 1.39f;
    //[SerializeField] [Range(0f, 1f)] private float fadeInStart = 0.4f;
    [SerializeField] [Range(0.01f, 1f)] private float scaleDownSpeed = 0.2f;

    private Vector3 initialArrowScale;
    //private Animator arrowAnimator;

    //public static UIManager Instance { get; private set; }

    private void Awake() {
        //#region Singleton
        //if (Instance == null) {
        //    Instance = this;
        //} else if (Instance != this) {
        //    Destroy(this);
        //    return;
        //}
        //#endregion

        //fadeOutSpeed = 1f - fadeOutSpeed;
        scaleDownSpeed = 1f - scaleDownSpeed;
        initialArrowScale = arrowImg.transform.localScale;

        Game.InputValidationEvent += OnMovementValidation;
        Game.NextEvent += OnNext;
        Game.MissEvent += OnSkip;
    }

    private void OnMovementValidation(bool isValidated, int scoreValue) {
        scoreText.text = "Score: " + scoreValue;

        ArrowType currentArrow = Game.CurrentArrow;

        if (isValidated) {
            arrowImg.color = currentArrow.SuccessColor;
        } else {
            arrowImg.color = currentArrow.FailColor;
        }

        StartCoroutine(PlayAnimations(isValidated));
    }

    private IEnumerator PlayAnimations(bool isValidated) {
        ArrowType.AnimationsHolder animationsHolder = Game.CurrentArrow.GetValidationAnimations(isValidated);

        int delayIndex = 0;
        foreach (CustomAnimation animation in animationsHolder.Animations) {
            if (animationsHolder.Delays[delayIndex] > 0f) {
                yield return new WaitForSeconds(animationsHolder.Delays[delayIndex]);
            }

            StartCoroutine(animation.GetAnimation(arrowImg.GetComponent(animation.AnimatedComponent))); // we can yield this
            delayIndex++;
        }
    }

    private void OnNext() {
        float rotation = 0f;
        SlideDirection newDirection = Game.CurrentArrow.DirectionBinder[Game.CurrentDirection];

        if (newDirection == SlideDirection.Right) {
            rotation = 90f;
        } else if (newDirection == SlideDirection.Down) {
            rotation = 180f;
        } else if (newDirection == SlideDirection.Left) {
            rotation = 270f;
        }

        StopAllCoroutines();
        arrowImg.transform.rotation = Quaternion.Euler(0f, 0f, 90f - rotation);
        arrowImg.rectTransform.anchoredPosition = Vector2.zero;
        //arrowImg.color = new Color(1f, 1f, 1f, fadeInStart);
        arrowImg.transform.localScale = initialArrowScale;
        //StartCoroutine(FadeArrow(false));

        arrowImg.color = Game.CurrentArrow.BaseColor;
        arrowImg.sprite = Game.CurrentArrow.Sprite;

        foreach (CustomAnimation animation in Game.CurrentArrow.AppearAnimations) {
            StartCoroutine(animation.GetAnimation(arrowImg.GetComponent(animation.AnimatedComponent)));
        }
    }

    private void OnSkip() {
        if (Game.CurrentArrow == null) {
            return;
        }

        arrowImg.color = Game.CurrentArrow.SkipColor;
        foreach (CustomAnimation animation in Game.CurrentArrow.SkipAnimations) {
            StartCoroutine(animation.GetAnimation(arrowImg.GetComponent(animation.AnimatedComponent)));
        }
    }

    private IEnumerator SlideArrow() {
        Vector2 slideDirection = DirectionUtility.DirectionToVector(Game.CurrentDirection) * slideDistance;
        Vector2 startPosition = arrowImg.rectTransform.anchoredPosition;

        for (float time = 0f; time < 1f; time += slideSpeed * Time.deltaTime) {
            arrowImg.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, slideDirection, slidingAnimation.Evaluate(time));
            yield return null;
        }
    }

    //private IEnumerator FadeArrow(bool isFadeOut) {
    //    float fadeMultiplier = isFadeOut ? fadeOutSpeed : fadeInSpeed;
    //    var evaluator = isFadeOut ? new Func<float, bool>(alpha => alpha > 0.01f) : new Func<float, bool>(alpha => alpha < 1f);

    //    while (evaluator(arrowImg.color.a)) {
    //        Color arrowColor = arrowImg.color;
    //        arrowImg.color = new Color(arrowColor.r, arrowColor.g, arrowColor.b, arrowColor.a * fadeMultiplier);
    //        yield return null;
    //    }
    //}

    private IEnumerator ScaleDownArrow() {
        Vector3 scale = arrowImg.transform.localScale;

        while (scale.x > 0f && scale.y > 0f) {
            scale.Set(scale.x * scaleDownSpeed, scale.y * scaleDownSpeed, 1f);
            arrowImg.transform.localScale = scale;
            yield return null;
        }
    }

    private void OnDestroy() {
        Game.InputValidationEvent -= OnMovementValidation;
        Game.NextEvent -= OnNext;
        Game.MissEvent -= OnSkip;
    }
}
