using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

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
    }

    private void Start() {
        GameManager.ValidationEvent += OnMovementValidation;
        GameManager.NextEvent += OnNext;
        GameManager.SkipEvent += OnSkip;

        //arrowAnimator = arrowImg.GetComponent<Animator>();
    }

    private void OnMovementValidation(bool isValidated, int scoreValue) {
        scoreText.text = "Score: " + scoreValue;

        ArrowType currentArrow = GameManager.CurrentArrow;

        if (isValidated) {
            arrowImg.color = currentArrow.SuccessColor;
        } else {
            arrowImg.color = currentArrow.FailColor;
        }

        StartCoroutine(PlayAnimations(isValidated));
    }

    private IEnumerator PlayAnimations(bool isValidated) {
        ArrowType currentArrow = GameManager.CurrentArrow;

        float[] animationDelays = currentArrow.GetValidationAnimationsDelays(isValidated);

        if (animationDelays.Length > 0) {

        }

        int animationIndex = 0;
        foreach (CustomAnimation animation in currentArrow.GetValidationAnimations(isValidated)) {
            if (animationDelays[animationIndex] > 0f) {
                yield return new WaitForSeconds(animationDelays[animationIndex]);
            }

            StartCoroutine(animation.GetAnimation(arrowImg.GetComponent(animation.AnimatedComponent))); // we can yield this
            animationIndex++;
        }
    }

    private void OnNext() {
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
        arrowImg.transform.rotation = Quaternion.Euler(0f, 0f, 90f - rotation);
        arrowImg.rectTransform.anchoredPosition = Vector2.zero;
        //arrowImg.color = new Color(1f, 1f, 1f, fadeInStart);
        arrowImg.transform.localScale = initialArrowScale;
        //StartCoroutine(FadeArrow(false));

        arrowImg.color = GameManager.CurrentArrow.BaseColor;
        arrowImg.sprite = GameManager.CurrentArrow.Sprite;

        foreach (CustomAnimation animation in GameManager.CurrentArrow.AppearAnimations) {
            StartCoroutine(animation.GetAnimation(arrowImg.GetComponent(animation.AnimatedComponent)));
        }
    }

    private void OnSkip() {
        if (GameManager.CurrentArrow == null) {
            return;
        }

        arrowImg.color = GameManager.CurrentArrow.SkipColor;
        foreach (CustomAnimation animation in GameManager.CurrentArrow.SkipAnimations) {
            StartCoroutine(animation.GetAnimation(arrowImg.GetComponent(animation.AnimatedComponent)));
        }
    }

    private IEnumerator SlideArrow() {
        Vector2 slideDirection = DirectionUtility.DirectionToVector(GameManager.CurrentDirection) * slideDistance;
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
        GameManager.ValidationEvent -= OnMovementValidation;
        GameManager.NextEvent -= OnNext;
        GameManager.SkipEvent -= OnSkip;
    }
}
