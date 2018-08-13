using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour {

    [Tooltip("The threshold that need to be reached in order to consider the sliding.")]
    [SerializeField] private float slidingSensibility = 0.25f;
    [SerializeField] private int maxLives = 3;

    [Header("")]
    [SerializeField] private Arrow[] arrowPrefabs;

    private float stayDuration = 1.5f;
    private float nextDelay = 0.4f;
    //private float countdown;
    private bool checkInput;
    private Countdown countdown;

    public static event System.Action<bool> OnInputValidationEvent;
    public static event System.Action OnMissedEvent;
    public static event System.Action OnGameOverEvent;
    public static event System.Action OnGameResetEvent;

    public static Game Instance { get; private set; }
    public static SlideDirection CurrentDirection { get; private set; }
    public static Arrow CurrentArrow { get; private set; }
    public static int TotalScore { get; private set; }

    public int Lives { get; private set; }

    private void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
            return;
        }
        #endregion

        slidingSensibility *= slidingSensibility;
    }

    private void OnEnable() {
        Lives = maxLives;

        Next();
        countdown.Begin();
    }

    private void Start() {
        countdown = GetComponent<Countdown>();
        countdown.Elapsed += OnCountDownElapsed;
    }

    private void OnCountDownElapsed() {
        checkInput = false;
        Skip();
    }

    private void Update() {
        if (!checkInput) {
            return;
        }

        if (Input.touchCount > 0 && InputUtility.HasTouchMoved(slidingSensibility)) {
            Touch touch = Input.GetTouch(0);

            SlideDirection inputDirection = DirectionUtility.VectorToDirection(touch.deltaPosition);
            bool hasScored = IsMovementValid(inputDirection);
            UpdateScore(hasScored);

            if (!hasScored) {
                Lives--;
                if (Lives <= 0) {
                    GameOver();
                    return;
                }
            }

            checkInput = false;
            Invoke("Next", nextDelay);
        }
    }

    private bool IsMovementValid(SlideDirection inputDirection) {
        bool isValidated = inputDirection == CurrentDirection;

        if (OnInputValidationEvent != null) {
            OnInputValidationEvent(isValidated);
        }

        return isValidated;
    }

    private void UpdateScore(bool hasScored) {
        if (hasScored) {
            TotalScore += CalculateScore();
        } else {
            TotalScore -= CurrentArrow.ScoreValue;
        }
    }

    private void Next() {
        CurrentDirection = DirectionUtility.DirectionValues[Random.Range(0, DirectionUtility.DirectionCount)];
        CurrentArrow = Instantiate(arrowPrefabs[Random.Range(0, arrowPrefabs.Length)]);

        stayDuration = CurrentArrow.StayDuration;
        nextDelay = CurrentArrow.NextDelay;
        countdown.WaitTime = stayDuration;
        countdown.ResetCountdown();

        checkInput = true;
    }

    private void Skip() {
        nextDelay = CurrentArrow.SkipDelay;
        Invoke("Next", nextDelay);

        if (OnMissedEvent != null) {
            OnMissedEvent();
        }
    }

    private int CalculateScore() {
        return Mathf.RoundToInt((CurrentArrow.ScoreValue * countdown.WaitTime) / stayDuration);
    }

    private void GameOver() {
        if (OnGameOverEvent != null) {
            OnGameOverEvent();
        }

        enabled = false;
    }

    public void ResetGame() {
        Lives = maxLives;
        TotalScore = 0;

        if (OnGameResetEvent != null) {
            OnGameResetEvent();
        }
    }

    private void OnDestroy() {
        countdown.Elapsed -= OnCountDownElapsed;
    }
}
