using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Delays")]
    [SerializeField] private float skipDelay = 1.5f;
    [SerializeField] private float nextDelay = 0.5f;
    [Header("")]
    [SerializeField] private int baseScoreValue = 10;
    [Tooltip("The threshold that need to be reached in order to consider the sliding. This is a value squared.")]
    [SerializeField] private float sensibility = 0.07f;

    private float countdown;
    private float inverseSkipDelay; // Used to remap values to 0 to 1
    private int totalScore;
    private bool isReady = true;

    private event System.Action<bool, int> _ValidationEvent;
    private event System.Action _NextEvent;

    public static GameManager Instance { get; private set; }
    public static SlideDirection CurrentDirection { get; private set; }

    /// <summary>
    /// Event triggered when the player sliding input is validated or not.
    /// </summary>
    public static event System.Action<bool, int> ValidationEvent {
        add {
            Instance._ValidationEvent += value;
        }
        remove {
            Instance._ValidationEvent -= value;
        }
    }

    /// <summary>
    /// Event triggered when the next direction is chosen.
    /// </summary>
    public static event System.Action NextEvent {
        add {
            Instance._NextEvent += value;
        }
        remove {
            Instance._NextEvent -= value;
        }
    }

    /// <summary>
    /// Converts the given direction to a vector.
    /// </summary>
    /// <param name="direction">
    /// The direction to convert from.
    /// </param>
    /// <returns>
    /// Returns the converted direction as a vector.
    /// </returns>
    public static Vector2 DirectionToVector(SlideDirection direction) {
        Vector2 convertedDirection;

        if (direction == SlideDirection.Up) {
            convertedDirection = Vector2.up;
        } else if (direction == SlideDirection.Right) {
            convertedDirection = Vector2.right;
        } else if (direction == SlideDirection.Down) {
            convertedDirection = -Vector2.up;
        } else {
            convertedDirection = -Vector2.right;
        }

        return convertedDirection;
    }

    /// <summary>
    /// Converts the given vector to a direction.
    /// </summary>
    /// <param name="direction">
    /// The vector to convert from.
    /// </param>
    /// <returns>
    /// Returns the converted vector as a direction.
    /// </returns>
    public static SlideDirection VectorToDirection(Vector2 direction) {
        SlideDirection convertedVector;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            convertedVector = Mathf.Sign(direction.x) == 1f ? SlideDirection.Right : SlideDirection.Left;
        } else {
            convertedVector = Mathf.Sign(direction.y) == 1f ? SlideDirection.Up : SlideDirection.Down;
        }

        return convertedVector;
    }

    private void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
            return;
        }
        #endregion

        countdown = skipDelay;
        inverseSkipDelay = 1f / skipDelay;
        CurrentDirection = SlideDirection.Right;
    }

    private void Update() {
        if (!isReady) {
            return;
        }

        countdown -= Time.deltaTime;

        if (countdown <= 0f) {
            countdown = skipDelay;
            // -1 life
            Debug.Log("Missed !");

            Next();
        } else if (Input.touchCount > 0 && isReady) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved && touch.deltaPosition.sqrMagnitude > sensibility) {
                Debug.Log("Slide: " + touch.deltaPosition);
                isReady = false;

                Debug.Log("Input: " + CurrentDirection + ", " + touch.deltaPosition);
                ValidateMovement(VectorToDirection(touch.deltaPosition));
                countdown = skipDelay;
                StartCoroutine(TriggerNextDelayed());
            }
        }
    }

    private IEnumerator TriggerNextDelayed() {
        yield return new WaitForSeconds(nextDelay);
        Next();
        isReady = true;
    }

    /// <summary>
    /// Validates the movement.
    /// </summary>
    /// <param name="inputDirection">
    /// The direction the player slided to.
    /// </param>
    private void ValidateMovement(SlideDirection inputDirection) {
        bool isValidated;

        if (inputDirection == CurrentDirection) {
            Debug.Log("Win: " + inputDirection);
            totalScore += CalculateScore();
            isValidated = true;
        } else {
            Debug.Log("Lose: " + inputDirection);
            totalScore -= baseScoreValue;
            isValidated = false;
        }

        if (_ValidationEvent != null) {
            _ValidationEvent(isValidated, totalScore);
        }
    }

    /// <summary>
    /// Calculates the score proportionally to the elapsed time.
    /// </summary>
    private int CalculateScore() {
        float factor = countdown * inverseSkipDelay;
        Debug.Log("+" + Mathf.RoundToInt(baseScoreValue * factor));
        return Mathf.RoundToInt(baseScoreValue * factor);
    }

    /// <summary>
    /// Continues the game by changing the current direction.
    /// </summary>
    private void Next() {
        CurrentDirection = (SlideDirection)Random.Range(0, 4);

        if (_NextEvent != null) {
            _NextEvent();
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, Mathf.Sqrt(sensibility));
    }
}

public enum SlideDirection {
    Left,
    Right,
    Up,
    Down
}
