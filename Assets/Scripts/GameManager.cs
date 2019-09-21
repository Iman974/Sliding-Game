using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    [SerializeField] int maxLives = 3;
    [SerializeField] Arrow[] arrows = null;

    public static GameManager Instance { get; private set; }
    public static Direction CurrentDirection { get; private set; }
    public static int PlayerScore { get; private set; }
    public static Arrow SelectedArrow { get; private set; }

    public int Lives { get; private set; }

    public static event System.Action<bool> OnFinalInputEvent;
    //public static event System.Action OnTimeElapsed; // For NextArrow() call
    public static event System.Action OnMissingInputAndTimeElapsed;
    public static event System.Action<int> OnMoveSuccess;

    Direction inputDirection;
    Direction desiredDirection;
    Direction displayedDirection;
    int currentMoveIndex;
    Vector2 previousMousePos;
    float countdown;
    bool countdownDelaySet = true;

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
                if (!countdownDelaySet) {
                    countdown = AnimationManager.NextDelay;
                    OnMissingInputAndTimeElapsed?.Invoke();
                    countdownDelaySet = true;
                } else {
                    NextArrow();
                    countdownDelaySet = false;
                }
                return;
            }

            HandleInput();
        }
    }

    void HandleInput() {
        if (Input.touchCount == 0 || !InputManager.GetInput(ref inputDirection)) {
            return;
        }

        if (currentMoveIndex < SelectedArrow.MoveCount) {
            int move = SelectedArrow.GetMove(currentMoveIndex);
            if ((int)inputDirection == (move + (int)displayedDirection) %
                    DirectionUtility.kDirectionCount) {
                // The input matches the move
                OnMoveSuccess?.Invoke(currentMoveIndex);
                currentMoveIndex++;
            } else {
                
            }
        } else if (inputDirection == desiredDirection) {
            // The arrow has been oriented successfully and the final move is right
            countdown = AnimationManager.NextDelay;
            PlayerScore += SelectedArrow.ScoreValue;
            currentMoveIndex = 0;
            countdownDelaySet = true;
            OnFinalInputEvent?.Invoke(true); // same instructions as input != desired, refactoring ?
        } else {
            // Wrong input on the scoring/final move
            PlayerScore -= SelectedArrow.ScoreValue / 2;
            countdown = AnimationManager.NextDelay;
            OnFinalInputEvent?.Invoke(false);
            countdownDelaySet = true;
        }
    }

    // Hide the previous arrow, randomly select a new one, rotate it and show it.
    void NextArrow() {
        if (SelectedArrow != null) {
            SelectedArrow.IsActive = false;
            SelectedArrow.ResetTransform();
        }

        desiredDirection = DirectionUtility.GetRandomDirection();

        SelectedArrow = arrows[SelectRandomWeightedIndex()];

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
}
