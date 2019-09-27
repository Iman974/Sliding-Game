using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    [SerializeField] int maxLives = 3;
    [SerializeField] Arrow[] arrows = null;
    [SerializeField] float nextDelay = 0.1f;

    public static GameManager Instance { get; private set; }
    public static Direction CurrentDirection { get; private set; }
    public static int PlayerScore { get; private set; }
    public static Arrow SelectedArrow { get; private set; }

    public int Lives { get; private set; }

    public static event System.Action<bool> OnFinalInputEvent;
    //public static event System.Action OnTimeElapsed; // For NextArrow() call
    public static event System.Action OnMissingInputAndTimeElapsed;
    public static event System.Action<int> OnMoveSuccess;
    public static event System.Action OnMoveFail;

    Direction inputDirection;
    Direction desiredDirection;
    Direction displayedDirection;
    int currentMoveIndex;
    float countdown;
    bool doInputCheck;

    void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
            return;
        }
        #endregion
    }

    void Update() {
        if (!AnimationManager.IsAnimating) {
            countdown -= Time.deltaTime;
            if (countdown <= 0f) {
                // Check if we're still handling input. If so, it means
                // no input was received so the player didn't do anything
                if (doInputCheck) {
                    countdown = nextDelay;
                    doInputCheck = false;
                    currentMoveIndex = 0;
                    OnMissingInputAndTimeElapsed?.Invoke();
                } else {
                    NextArrow();
                    doInputCheck = true;
                }
                return;
            }

            if (doInputCheck) {
                HandleInput();
            }
        }
    }

    void NextArrow() {
        // Hide the previous arrow and reset it
        if (SelectedArrow != null) {
            SelectedArrow.IsActive = false;
            SelectedArrow.ResetTransform();
        }

        // Randomly select an arrow based randomly on the weights
        SelectedArrow = arrows[SelectRandomWeightedIndex()];

        // Randomly choose a direction and set display direction based on the arrow modifier
        desiredDirection = DirectionUtility.GetRandomDirection();
        displayedDirection = (Direction)(((int)desiredDirection +
            SelectedArrow.DisplayedDirectionModifier) % DirectionUtility.kDirectionCount);

        SelectedArrow.Orientation = displayedDirection;
        SelectedArrow.IsActive = true;
        countdown = SelectedArrow.Duration;
    }

    // While or for loop ? make a choice. Algorithm (to be improved by
    // using random function only once) from the website
    // https://forum.unity.com/threads/random-numbers-with-a-weighted-chance.442190/
    int SelectRandomWeightedIndex() {
        int weightSum = arrows.Sum(a => a.Weight);
        for (int i = 0; i < arrows.Length - 1; i++) {
            if (Random.Range(0, weightSum) < arrows[i].Weight) {
                return i;
            }
            weightSum -= arrows[i].Weight;
        }
        return arrows.Length - 1;
    }

    void HandleInput() {
        if (Input.touchCount == 0 || !InputManager.GetInput(ref inputDirection)) {
            return;
        }

        // Check if the move list has been entirely iterated through
        if (currentMoveIndex < SelectedArrow.MoveCount) {
            int move = SelectedArrow.GetMove(currentMoveIndex);
            if ((int)inputDirection == (move + (int)displayedDirection) %
                    DirectionUtility.kDirectionCount) {
                // The input matches the move
                OnMoveSuccess?.Invoke(currentMoveIndex);
                currentMoveIndex++;
            } else {
                countdown = nextDelay;
                PlayerScore -= SelectedArrow.ScoreValue / 2;
                doInputCheck = false;
                OnMoveFail?.Invoke();
                currentMoveIndex = 0;
            }
        } else if (inputDirection == desiredDirection) {
            // The arrow has been oriented successfully and the final move is right
            countdown = nextDelay;
            PlayerScore += SelectedArrow.ScoreValue;
            currentMoveIndex = 0;
            doInputCheck = false;
            OnFinalInputEvent?.Invoke(true); // same instructions as input != desired, refactoring ?
        } else {
            // Wrong input on the scoring/final move
            PlayerScore -= SelectedArrow.ScoreValue / 2;
            countdown = nextDelay;
            doInputCheck = false;
            currentMoveIndex = 0;
            OnFinalInputEvent?.Invoke(false);
        }
    }
}
