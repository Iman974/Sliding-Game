using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    [SerializeField] Arrow[] arrows = null;
    [SerializeField] float nextDelay = 0.3f;

    public static GameManager Instance { get; private set; }
    public static Direction CurrentDirection { get; private set; }
    public static int PlayerScore { get; private set; }
    public static Arrow SelectedArrow { get; private set; }

    public static event System.Action<BeforeNextArrowEventArgs> BeforeNextArrow;
    public static event System.Action<OnMoveSuccessEventArgs> OnMoveSuccess;

    Direction inputDirection;
    Direction desiredDirection;
    Direction displayedDirection;
    float countdown;
    bool doInputCheck;
    bool doInputProcessing;
    int currentMoveIndex;

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
                    var eventArgs = new BeforeNextArrowEventArgs(false);
                    PlayerScore -= (int)(SelectedArrow.ScoreValue * 1.5f);
                    BeforeNextArrow?.Invoke(eventArgs);
                    ResetValues();
                } else {
                    NextArrow();
                    doInputCheck = true;
                }
                return;
            }

            if (doInputCheck && InputManager.GetInput(ref inputDirection)) {
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

    // Algorithm found on the Unity forum:
    // https://forum.unity.com/threads/random-numbers-with-a-weighted-chance.442190/
    int SelectRandomWeightedIndex() {
        int weightSum = arrows.Sum(a => a.Weight);
        int p = 0;
        int randomValue = Random.Range(0, weightSum);
        for (int i = 0; i < arrows.Length - 1; i++) {
            p += arrows[i].Weight;
            if (randomValue < p) {
                return i;
            }
        }
        return arrows.Length - 1;
    }

    void HandleInput() {
        // Check if the move list has been entirely iterated through
        if (currentMoveIndex < SelectedArrow.MoveCount) {
            int move = SelectedArrow.GetMove(currentMoveIndex);
            if ((int)inputDirection == (move + (int)displayedDirection) %
                    DirectionUtility.kDirectionCount) {
                // The input matches the move
                var eventArgs = new OnMoveSuccessEventArgs(currentMoveIndex);
                OnMoveSuccess?.Invoke(eventArgs);
                currentMoveIndex++;
            } else {
                PlayerScore -= SelectedArrow.ScoreValue;
                var eventArgs = new BeforeNextArrowEventArgs(false);
                BeforeNextArrow?.Invoke(eventArgs);
                ResetValues();
            }
        } else {
            // The arrow has been oriented successfully (no moves are left)
            bool isSuccess = inputDirection == desiredDirection;
            float percentage = countdown / SelectedArrow.Duration;
            PlayerScore += isSuccess ? (int)(SelectedArrow.ScoreValue * percentage):
                -(int)(SelectedArrow.ScoreValue * percentage);

            var beforeNextArrowEventArgs = new BeforeNextArrowEventArgs(isSuccess);
            BeforeNextArrow?.Invoke(beforeNextArrowEventArgs);
            ResetValues();
        }
    }

    void ResetValues() {
        countdown = nextDelay;
        currentMoveIndex = 0;
        doInputCheck = false;
    }
}
