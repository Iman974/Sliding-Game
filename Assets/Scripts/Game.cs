using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour {

    [Tooltip("The threshold that need to be reached in order to consider the sliding.")]
    [SerializeField] private float slidingSensibility = 0.25f;
    [SerializeField] private int maxLives = 3;

    [Header("")]
    [SerializeField] private Arrow[] arrowPrefabs;

    private float skipDelay = 1.5f;
    private float nextDelay = 0.4f;
    private float countdown;
    private bool wait;

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
        wait = false;

        Next();
    }

    private void Update() {
        if (wait) {
            return;
        }

        countdown -= Time.deltaTime;

        if (countdown <= 0f) {
            wait = true;

            Skip();
        } else if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved && touch.deltaPosition.sqrMagnitude > slidingSensibility) {
                wait = true;

                if (!ValidateMovement(DirectionUtility.VectorToDirection(touch.deltaPosition))) {
                    if (Lives <= 0) {
                        GameOver();
                        return;
                    }
                }
                StartCoroutine(TriggerNextDelayed());
            }
        }
    }

    private void Next() {
        CurrentDirection = DirectionUtility.DirectionValues[Random.Range(0, DirectionUtility.DirectionCount)];
        CurrentArrow = Instantiate(arrowPrefabs[Random.Range(0, arrowPrefabs.Length)]);

        RecalculateDelays();
        countdown = skipDelay;
    }

    private void RecalculateDelays() {
        skipDelay = CurrentArrow.StayDuration;
        nextDelay = CurrentArrow.NextDelay;
    }

    private void Skip() {
        nextDelay = CurrentArrow.SkipDelay;
        StartCoroutine(TriggerNextDelayed());

        if (OnMissedEvent != null) {
            OnMissedEvent();
        }
    }

    private IEnumerator TriggerNextDelayed() {
        yield return new WaitForSeconds(nextDelay);
        Next();
        wait = false;
    }

    private bool ValidateMovement(SlideDirection inputDirection) {
        bool isValidated;

        if (inputDirection == CurrentDirection) {
            TotalScore += CalculateScore();
            isValidated = true;
        } else {
            TotalScore -= CurrentArrow.ScoreValue;
            Lives--;
            isValidated = false;
        }

        if (OnInputValidationEvent != null) {
            OnInputValidationEvent(isValidated);
        }

        return isValidated;
    }

    private int CalculateScore() {
        return Mathf.RoundToInt((CurrentArrow.ScoreValue * countdown) / skipDelay);
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
}
