using UnityEngine;

public class Arrow : MonoBehaviour {

    [SerializeField] private SerializableDictionary_SlideDirection directionBinder;

    [SerializeField] private int scoreValue = 50;
    public int ScoreValue {
        get { return scoreValue; }
    }

    [Header("Time")]
    [SerializeField] private float stayDuration = 1.25f;
    public float StayDuration {
        get { return stayDuration; }
    }

    [SerializeField] private float nextDelay = 0.5f;
    public float NextDelay {
        get { return nextDelay; }
    }

    [SerializeField] private float skipDelay = 0.3f;
    public float SkipDelay {
        get { return skipDelay; }
    }

    [Header("Colors")]
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color failColor = Color.red;
    [SerializeField] private Color skipColor = Color.blue;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private SlideDirection currentDirection;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        Game.OnInputValidationEvent += OnMovementValidation;

        Orient();
    }

    private void OnMovementValidation(bool isValidated, int scoreValue) {
        if (isValidated) {
            spriteRenderer.color = successColor;
        } else {
            spriteRenderer.color = failColor;
        }

        animator.SetBool("skip", !isValidated);
        animator.SetTrigger(Game.CurrentDirection.ToString());
    }

    private void Orient() {
        float rotation = 0f;
        SlideDirection matchingDirection = directionBinder[Game.CurrentDirection];

        if (matchingDirection == SlideDirection.Right) {
            rotation = 90f;
        } else if (matchingDirection == SlideDirection.Down) {
            rotation = 180f;
        } else if (matchingDirection == SlideDirection.Left) {
            rotation = 270f;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }

    private void OnMiss() {
        spriteRenderer.color = skipColor;
        //TODO: play skip animations
    }

    private void OnDestroy() {
        Game.OnInputValidationEvent -= OnMovementValidation;
    }
}
