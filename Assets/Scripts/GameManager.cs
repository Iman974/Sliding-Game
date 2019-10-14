using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    [SerializeField] Arrow[] arrows = null;
    [SerializeField] float nextDelay = 0.3f;
    [SerializeField] float speedGainOverProgression = 0.002f;
    [SerializeField] int successCountToRegenerateLife = 10;
    [SerializeField] float restartGameDelay = 1.5f;
    [SerializeField] float maxPlaybackSpeed = 2f;

    public static GameManager Instance { get; private set; }
    public static Direction CurrentDirection { get; private set; }
    public static Arrow SelectedArrow { get; private set; }
    public static int Lives {
        get => lives;
        set {
            lives = value;
            OnLivesUpdated?.Invoke();
        }
    }
    public static int Highscore { get; private set; }
    public static Countdown Countdown { get; } = new Countdown();

    public static event System.Action<bool> OnArrowEnd;
    public static event System.Action OnGameOver;
    public static event System.Action OnGameRestart;
    public static event System.Action OnLivesUpdated;

    public const int kMaxLives = 3;

    Direction inputDirection;
    Direction desiredDirection;
    bool doInputCheck;
    float playbackSpeed = 1f;
    int successiveSuccessCount;
    static int lives = kMaxLives;

    void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
            return;
        }
        #endregion

        for (int i = 0; i < arrows.Length; i++) {
            UnityEngine.Assertions.Assert.IsNotNull(arrows[i], "Missing arrow!");
        }
        Highscore = ProgressSaver.LoadHighscore();
        enabled = false;
    }

    void Update() {
        if (Arrow.IsAnimating) {
            return;
        }

        Countdown.Update(Time.deltaTime * playbackSpeed);
        if (Countdown.Elapsed) {
            OnTimeElapsed();
            return;
        }

        if (doInputCheck && InputManager.GetInput(ref inputDirection)) {
            HandleInput();
        }
    }

    void OnTimeElapsed() {
        // Check if we're still handling input. If so, it means
        // no input was received so the player didn't do anything
        if (doInputCheck) {
            OnWrongInput();
            OnArrowEnd?.Invoke(false);
            ResetValues();
        } else {
            NextArrow();
        }
    }

    public void NextArrow() {
        // Hide the previous arrow and reset it
        if (SelectedArrow != null) {
            SelectedArrow.IsActive = false;
            SelectedArrow.ResetTransform();
        }

        // Randomly select an arrow based randomly on the weights
        SelectedArrow = arrows[SelectRandomWeightedIndex()];

        desiredDirection = DirectionUtility.GetRandomDirection();
        SelectedArrow.SetOrientation(desiredDirection);
        SelectedArrow.IsActive = true;
        Countdown.ResetTime(SelectedArrow.Duration);
        doInputCheck = true;
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
        float percentage = Countdown.Time / SelectedArrow.Duration;
        bool isInputCorrect = inputDirection == desiredDirection;
        if (isInputCorrect) {
            playbackSpeed = Mathf.Min(maxPlaybackSpeed, playbackSpeed + speedGainOverProgression);
            successiveSuccessCount++;
            if (successiveSuccessCount >= successCountToRegenerateLife) {
                if (Lives < kMaxLives) {
                    Lives++;
                }
                successiveSuccessCount = 0;
            }
        } else {
            OnWrongInput();
        }
        OnArrowEnd?.Invoke(isInputCorrect);
        ResetValues();
    }

    void ResetValues() {
        Countdown.ResetTime(nextDelay);
        doInputCheck = false;
    }

    void OnWrongInput() {
        successiveSuccessCount = 0;
        Lives -= 1;

        if (Lives <= 0) {
            GameOver();
        }
    }

    void GameOver() {
        if (Score.PlayerScore > Highscore) {
            Highscore = Score.PlayerScore;
            ProgressSaver.SaveHighscore(Highscore);
        }
        enabled = false;
        OnGameOver?.Invoke();
    }

    public void RestartGame() {
        enabled = true;
        Countdown.ResetTime(restartGameDelay);
        Lives = kMaxLives;
        playbackSpeed = 1f;
        OnGameRestart?.Invoke();
    }
}
