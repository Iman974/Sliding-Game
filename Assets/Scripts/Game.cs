using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour {

    [Tooltip("The threshold that need to be reached in order to consider the sliding.")]
    [SerializeField] private float slidingSensibility = 0.25f;

    [Header("")]
    [SerializeField] private ArrowType[] arrowDatas;

    private float skipDelay = 1.5f;
    private float nextDelay = 0.4f;
    private float countdown;
    private int totalScore;
    private bool isReady = true;

    public static event System.Action<bool, int> InputValidationEvent;

    public static event System.Action NextEvent;

    public static event System.Action MissEvent;

    public static Game Instance { get; private set; }
    public static SlideDirection CurrentDirection { get; private set; }
    public static ArrowType CurrentArrow { get; private set; }

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
        slidingSensibility *= slidingSensibility;
    }

    private void Start() {
        Next();
    }

    private void Update() {
        if (!isReady) {
            return;
        }

        countdown -= Time.deltaTime;

        if (countdown <= 0f) {
            isReady = false;

            Skip();
        } else if (Input.touchCount > 0 && isReady) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved && touch.deltaPosition.sqrMagnitude > slidingSensibility) {
                isReady = false;

                ValidateMovement(DirectionUtility.VectorToDirection(touch.deltaPosition));
                StartCoroutine(TriggerNextDelayed());
            }
        }
    }

    /// <summary>
    /// Continues the game by changing the current direction.
    /// </summary>
    private void Next() {
        CurrentDirection = DirectionUtility.DirectionValues[Random.Range(0, DirectionUtility.DirectionCount)];
        CurrentArrow = arrowDatas[Random.Range(0, arrowDatas.Length)];

        RecalculateDelays();
        countdown = skipDelay;

        if (NextEvent != null) {
            NextEvent();
        }
    }

    private void RecalculateDelays() {
        skipDelay = CurrentArrow.StayDuration;
        nextDelay = CurrentArrow.NextDelay;
    }

    private void Skip() {
        nextDelay = CurrentArrow.SkipDelay;
        StartCoroutine(TriggerNextDelayed());

        if (MissEvent != null) {
            MissEvent();
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
            totalScore += CalculateScore();
            isValidated = true;
        } else {
            Debug.Log("Lose: " + CurrentDirection + ", input was:" + inputDirection);
            totalScore -= CurrentArrow.ScoreValue;
            isValidated = false;
        }

        if (InputValidationEvent != null) {
            InputValidationEvent(isValidated, totalScore);
        }
    }

    private int CalculateScore() {
        return Mathf.RoundToInt(CurrentArrow.ScoreValue * countdown * (1f / skipDelay));
    }

    public void ResetGame() {
        Next();
    }
}
