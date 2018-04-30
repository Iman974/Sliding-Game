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
    private SlideDirection currentDirection = SlideDirection.Right;
    private int totalScore;
    private bool isReady = true;

    private event System.Action<int> _ValidationEvent;
    private event System.Action<SlideDirection> _NextEvent;

    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Event triggered when the player sliding input is validated or not.
    /// </summary>
    public static event System.Action<int> ValidationEvent {
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
    public static event System.Action<SlideDirection> NextEvent {
        add {
            Instance._NextEvent += value;
        }
        remove {
            Instance._NextEvent -= value;
        }
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
                countdown = skipDelay;
                isReady = false;
                SlideDirection inputDirection;

                if (Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y)) {
                    inputDirection = Mathf.Sign(touch.deltaPosition.x) == 1f ? SlideDirection.Right : SlideDirection.Left;
                } else {
                    inputDirection = Mathf.Sign(touch.deltaPosition.y) == 1f ? SlideDirection.Up : SlideDirection.Down;
                }

                Debug.Log("Input: " + currentDirection + ", " + touch.deltaPosition);
                ValidateMovement(inputDirection);
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
        if (inputDirection == currentDirection) {
            Debug.Log("Win: " + inputDirection);
            totalScore += CalculateScore();
        } else {
            Debug.Log("Lose: " + inputDirection);
            totalScore -= baseScoreValue;
        }

        if (_ValidationEvent != null) {
            _ValidationEvent(totalScore);
        }
    }

    private int CalculateScore() {
        float timeElapsed = countdown * inverseSkipDelay;

        return Mathf.RoundToInt(baseScoreValue * timeElapsed);
    }

    /// <summary>
    /// Continues the game by changing the current direction.
    /// </summary>
    private void Next() {
        currentDirection = (SlideDirection)Random.Range(0, 4);

        if (_NextEvent != null) {
            _NextEvent(currentDirection);
        }
    }
}

public enum SlideDirection {
    Left,
    Right,
    Up,
    Down
}
