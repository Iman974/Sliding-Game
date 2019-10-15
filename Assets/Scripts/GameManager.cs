using UnityEngine;

[RequireComponent(typeof(Countdown))]
public class GameManager : MonoBehaviour {

    [SerializeField] Arrow[] arrows = null;
    [SerializeField] float nextDelay = 0.3f;
    [SerializeField] float speedGainOverProgression = 0.002f;
    [SerializeField] int successCountToRegenerateLife = 10;
    [SerializeField] float maxPlaybackSpeed = 2f;
    [SerializeField] float restartGameDelay = 1.5f;

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

    public Arrow[] Arrows => arrows;

    public static event System.Action<bool> OnArrowEnd;
    public static event System.Action OnGameOver;
    public static event System.Action OnGameRestart;
    public static event System.Action OnLivesUpdated;

    public const int kMaxLives = 3;

    Direction inputDirection;
    Direction desiredDirection;
    float playbackSpeed = 1f;
    int consecutiveSuccessCount;
    static int lives = kMaxLives;
    Countdown countdown;
    bool doInputCheck;

    void OnEnable() {
        countdown = GetComponent<Countdown>();
        NextArrow();
    }

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
        if (enabled) {
            Debug.LogWarning("GameManager was enabled before the start!");
        }
    }

    void Update() {
        if (Arrow.IsAnimating || !doInputCheck) {
            return;
        }

        if (countdown.IsElapsed) {
            OnTimeElapsed();
            return;
        }

        if (InputManager.GetInput(ref inputDirection)) {
            HandleReceivedInput();
        }
    }

    void OnTimeElapsed() {
        doInputCheck = false;
        OnArrowEnd?.Invoke(false);
        OnWrongInput();
    }

    System.Collections.IEnumerator InvokeNextArrowDelayed() {
        while (Arrow.IsAnimating) {
            yield return null;
        }
        Invoke("NextArrow", nextDelay);
    }

    void NextArrow() {
        // Hide the previous arrow and reset its transform
        if (SelectedArrow != null) {
            SelectedArrow.IsActive = false;
            SelectedArrow.ResetTransform();
        }

        // Randomly select an arrow with weighted probability
        SelectedArrow = arrows[RandomUtility.SelectRandomWeightedIndex(arrows)];

        desiredDirection = DirectionUtility.GetRandomDirection();
        SelectedArrow.SetOrientation(desiredDirection);
        SelectedArrow.IsActive = true;
        countdown.Restart(SelectedArrow.Duration);
        doInputCheck = true;
    }

    void HandleReceivedInput() {
        bool isInputCorrect = inputDirection == desiredDirection;
        if (isInputCorrect) {
            playbackSpeed = Mathf.Min(maxPlaybackSpeed, playbackSpeed + speedGainOverProgression);
            consecutiveSuccessCount++;
            if (consecutiveSuccessCount >= successCountToRegenerateLife) {
                if (Lives < kMaxLives) {
                    Lives++;
                }
                consecutiveSuccessCount = 0;
            }
            StartCoroutine(InvokeNextArrowDelayed());
        } else {
            OnWrongInput();
        }
        doInputCheck = false;
        OnArrowEnd?.Invoke(isInputCorrect);
    }

    void OnWrongInput() {
        consecutiveSuccessCount = 0;
        Lives -= 1;

        if (Lives <= 0) {
            GameOver();
        } else {
            StartCoroutine(InvokeNextArrowDelayed());
        }
    }

    void GameOver() {
        if (ScoreManager.PlayerScore > Highscore) {
            Highscore = ScoreManager.PlayerScore;
            ProgressSaver.SaveHighscore(Highscore);
        }
        enabled = false;
        OnGameOver?.Invoke();
    }

    public void RestartGame() {
        Invoke("EnableThisScript", restartGameDelay);
        Lives = kMaxLives;
        playbackSpeed = 1f;
        OnGameRestart?.Invoke();
    }

    void EnableThisScript() {
        enabled = true;
    }
}
