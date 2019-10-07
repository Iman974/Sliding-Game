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
    public static int PlayerScore {
        get => playerScore;
        private set {
            int previousScore = playerScore;
            playerScore = value;
            OnScoreUpdated?.Invoke(previousScore);
        }
    }
    public static Arrow SelectedArrow { get; private set; }
    public static int Lives {
        get => lives;
        set {
            lives = value;
            OnLivesUpdated?.Invoke();
        }
    }
    public static int Highscore {
        get => highscore;
        set {
            highscore = value;
            OnHighscoreUpdated?.Invoke();
        }
    }

    public static event System.Action<bool> OnArrowEnd;
    public static event System.Action OnGameOver;
    public static event System.Action OnGameRestart;
    public static event System.Action<int> OnScoreUpdated;
    public static event System.Action OnLivesUpdated;
    public static event System.Action OnHighscoreUpdated;

    public const int kMaxLives = 3;
    const float kFirstStartDelay = 1f;

    Direction inputDirection;
    Direction desiredDirection;
    Direction shownDirection;
    float countdown = kFirstStartDelay;
    bool doInputCheck;
    float playbackSpeed = 1f;
    int successiveSuccessCount;
    bool isPlaying = true;
    static int playerScore;
    static int lives = kMaxLives;
    static int highscore;

    void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
            return;
        }
        #endregion

        Highscore = ProgressSaver.LoadHighscore();
    }

    void Update() {
        if (AnimationManager.IsAnimating || !isPlaying) {
            return;
        }

        countdown -= Time.deltaTime * playbackSpeed;
        if (countdown <= 0f) {
            // Check if we're still handling input. If so, it means
            // no input was received so the player didn't do anything
            if (doInputCheck) {
                int scoreLoss = (int)(SelectedArrow.ScoreValue * 1.5f);
                OnWrongInput(scoreLoss);
                OnArrowEnd?.Invoke(false);
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
        shownDirection = SelectedArrow.GetDirectionToShow(desiredDirection);

        SelectedArrow.CurrentOrientation = shownDirection;
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
        float percentage = countdown / SelectedArrow.Duration;
        bool isInputCorrect = inputDirection == desiredDirection;
        if (isInputCorrect) {
            PlayerScore += (int)(SelectedArrow.ScoreValue * percentage);
            playbackSpeed = Mathf.Min(maxPlaybackSpeed, playbackSpeed +
                speedGainOverProgression);
            successiveSuccessCount++;
            if (successiveSuccessCount >= successCountToRegenerateLife) {
                if (Lives < kMaxLives) {
                    Lives++;
                }
                successiveSuccessCount = 0;
            }
        } else {
            int scoreLoss = (int)(SelectedArrow.ScoreValue * percentage);
            OnWrongInput(scoreLoss);
        }
        OnArrowEnd?.Invoke(isInputCorrect);
        ResetValues();
    }

    void ResetValues() {
        countdown = nextDelay;
        doInputCheck = false;
    }

    void OnWrongInput(int scoreLoss) {
        PlayerScore = Mathf.Max(0, PlayerScore - scoreLoss);
        successiveSuccessCount = 0;
        Lives -= 1;

        if (Lives <= 0) {
            GameOver();
        }
    }

    void GameOver() {
        if (PlayerScore > Highscore) {
            Highscore = PlayerScore;
            ProgressSaver.SaveHighscore(Highscore);
        }
        isPlaying = false;
        OnGameOver?.Invoke();
    }

    public void RestartGame() {
        countdown = restartGameDelay;
        PlayerScore = 0;
        isPlaying = true;
        Lives = kMaxLives;
        playbackSpeed = 1f;
        OnGameRestart?.Invoke();
    }
}
