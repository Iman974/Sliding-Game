using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour {

    [Tooltip("The threshold that need to be reached in order to consider the sliding.")]
    [SerializeField] private float slidingSensibility = 0.25f;

    [Header("")]
    [SerializeField] private Arrow[] arrowPrefabs;

    private float skipDelay = 1.5f;
    private float nextDelay = 0.4f;
    private float countdown;
    private int totalScore;
    private bool isReady = true;

    public static event System.Action<bool, int> OnInputValidationEvent;

    public static event System.Action OnNextEvent;

    public static event System.Action OnMissEvent;

    public static Game Instance { get; private set; }
    public static SlideDirection CurrentDirection { get; private set; }
    public static Arrow CurrentArrow { get; private set; }

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

    private void Next() {
        CurrentDirection = DirectionUtility.DirectionValues[Random.Range(0, DirectionUtility.DirectionCount)];
        //CurrentArrow = arrowDatas[Random.Range(0, arrowDatas.Length)];
        CurrentArrow = Instantiate(arrowPrefabs[Random.Range(0, arrowPrefabs.Length)]).GetComponent<Arrow>();

        RecalculateDelays();
        countdown = skipDelay;

        if (OnNextEvent != null) {
            OnNextEvent(); // delete this event call later ?
        }
    }

    private void RecalculateDelays() {
        skipDelay = CurrentArrow.StayDuration;
        nextDelay = CurrentArrow.NextDelay;
    }

    private void Skip() {
        nextDelay = CurrentArrow.SkipDelay;
        StartCoroutine(TriggerNextDelayed());

        if (OnMissEvent != null) {
            OnMissEvent();
        }
    }

    private IEnumerator TriggerNextDelayed() {
        yield return new WaitForSeconds(nextDelay);
        Next();
        isReady = true;
    }

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

        if (OnInputValidationEvent != null) {
            OnInputValidationEvent(isValidated, totalScore);
        }
    }

    private int CalculateScore() {
        return Mathf.RoundToInt((CurrentArrow.ScoreValue * countdown) / skipDelay);
    }

    public void ResetGame() {
        Next();
    }
}
