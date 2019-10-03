using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    [SerializeField] Arrow[] arrows = null;
    [SerializeField] float nextDelay = 0.3f;
    [SerializeField] float speedGainOverProgression = 0.002f;
    [SerializeField] int maxLives = 3;
    [SerializeField] int successCountToRegenerateLife = 10;

    public static GameManager Instance { get; private set; }
    public static Direction CurrentDirection { get; private set; }
    public static int PlayerScore { get; private set; }
    public static Arrow SelectedArrow { get; private set; }
    public static float SpeedGainOverProgression => SpeedGainOverProgression;

    public static event System.Action<BeforeNextArrowEventArgs> BeforeNextArrow;
    public static event System.Action<OnMoveSuccessEventArgs> OnMoveSuccess;
    public static event System.Action OnGameOver;

    Direction inputDirection;
    Direction desiredDirection;
    Direction displayedDirection;
    float countdown;
    bool doInputCheck;
    bool doInputProcessing;
    int currentMoveIndex;
    float playbackSpeed = 1f;
    int lives;
    int successiveSuccessCount;
    bool isGameOver;

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

    void Start() {
        lives = maxLives;
    }

    void Update() {
        if (AnimationManager.IsAnimating || isGameOver) {
            return;
        }

        countdown -= Time.deltaTime * playbackSpeed;
        if (countdown <= 0f) {
            // Check if we're still handling input. If so, it means
            // no input was received so the player didn't do anything
            if (doInputCheck) {
                var eventArgs = new BeforeNextArrowEventArgs(false);
                OnFail((int)(SelectedArrow.ScoreValue * 1.5f));
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
                OnFail((int)(SelectedArrow.ScoreValue * 0.6f));
                var eventArgs = new BeforeNextArrowEventArgs(false);
                BeforeNextArrow?.Invoke(eventArgs);
                ResetValues();
            }
        } else {
            // The arrow has been oriented successfully (no moves are left)
            bool isSuccess = inputDirection == desiredDirection;
            float percentage = countdown / SelectedArrow.Duration;
            if (isSuccess) {
                PlayerScore += (int)(SelectedArrow.ScoreValue * percentage);
                playbackSpeed += speedGainOverProgression;
                successiveSuccessCount++;
                if (successiveSuccessCount >= successCountToRegenerateLife) {
                    if (lives < maxLives) {
                        lives++;
                    }
                    successiveSuccessCount = 0;
                }
            } else {
                OnFail((int)(SelectedArrow.ScoreValue * percentage));
            }
             
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

    void OnFail(int scoreLoss) {
        PlayerScore = Mathf.Max(0, PlayerScore - scoreLoss);
        successiveSuccessCount = 0;
        lives -= 1;

        if (lives <= 0) {
            GameOver();
        }
    }

    void GameOver() {
        OnGameOver?.Invoke();
        isGameOver = true;
    }
}
