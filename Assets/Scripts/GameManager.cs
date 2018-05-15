﻿using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //[SerializeField] private int baseScoreValue = 10;
    [Tooltip("The threshold that need to be reached in order to consider the sliding. This is a value squared.")]
    [SerializeField] private float sensibility = 0.07f;

    [Header("")]
    [SerializeField] private ArrowType[] arrowDatas;

    private float skipDelay = 1.5f;
    private float nextDelay = 0.4f;
    private float countdown;
    private int totalScore;
    private bool isReady = true;

    private event System.Action<bool, int> _ValidationEvent;
    private event System.Action _NextEvent;
    private event System.Action _SkipEvent;

    public static GameManager Instance { get; private set; }
    public static SlideDirection CurrentDirection { get; private set; }
    public static ArrowType CurrentArrow { get; private set; }

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

    public static event System.Action SkipEvent {
        add {
            Instance._SkipEvent += value;
        }
        remove {
            Instance._SkipEvent -= value;
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
        CurrentDirection = SlideDirection.Right;
    }

    private void Start() {
        //UIManager.AnimationEnd += OnAnimationEnd;
    }

    private void Update() {
        if (!isReady) {
            return;
        }

        countdown -= Time.deltaTime;

        if (countdown <= 0f) {
            isReady = false;
            // -1 life
            //Debug.Log("Missed !");

            Skip();
        } else if (Input.touchCount > 0 && isReady) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved && touch.deltaPosition.sqrMagnitude > sensibility) {
                //Debug.Log("Slide: " + touch.deltaPosition);
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

        if (_NextEvent != null) {
            _NextEvent();
        }
    }

    private void RecalculateDelays() {
        skipDelay = CurrentArrow.StayDuration;
        nextDelay = CurrentArrow.NextDelay;
    }

    private void Skip() {
        StartCoroutine(TriggerNextDelayed());

        if (_SkipEvent != null) {
            _SkipEvent();
        }
    }

    private IEnumerator TriggerNextDelayed() {
        yield return new WaitForSeconds(nextDelay);
        Next();
        isReady = true;
        //Debug.Log("Ready !");
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
            Debug.Log("Lose, input was:" + inputDirection);
            totalScore -= CurrentArrow.ScoreValue;
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
        return Mathf.RoundToInt(CurrentArrow.ScoreValue * countdown * (1f / skipDelay));
    }

    private void OnAnimationEnd() {
        //isReady = true;
    }

    private void OnDestroy() {
        //UIManager.AnimationEnd -= OnAnimationEnd;
    }
}
