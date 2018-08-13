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
    private Direction currentDirection;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        Game.OnInputValidationEvent += OnMovementValidation;
        Game.OnMissedEvent += OnMissed;

        currentDirection = Game.CurrentDirection;
        Orient();
    }

    private void OnMovementValidation(bool isValidated) {
        if (isValidated) {
            spriteRenderer.color = successColor;

            animator.SetTrigger(currentDirection.ToString());
            Vector2 movement = DirectionUtility.DirectionToVector(currentDirection);
            animator.SetFloat("moveX", movement.x);
            animator.SetFloat("moveY", movement.y);
        } else {
            spriteRenderer.color = failColor;
            animator.SetTrigger("failed");
        }
    }

    private void Orient() {
        Direction matchingDirection = directionBinder[currentDirection];

        float rotation = DirectionUtility.GetRotationFromDirection(matchingDirection);
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }

    private void OnMissed() {
        spriteRenderer.color = skipColor;
        animator.SetTrigger("missed");
    }

    private void DestroyGameObject() {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        Game.OnInputValidationEvent -= OnMovementValidation;
        Game.OnMissedEvent -= OnMissed;
    }
}
