using UnityEngine;

public class PlaymodeManager : MonoBehaviour {

    [SerializeField] float restartGameDelay = 1.5f;
    [SerializeField] int successCountToRegenerateLife = 9;

    //public static int Highscore { get; private set; }
    public static Countdown Countdown { get; private set; } = new Countdown();
    public static int LivesCount {
        get => lives;
        private set {
            lives = value;
            OnLivesUpdated?.Invoke();
        }
    }

    public static event System.Action OnGameOver;
    public static event System.Action OnGameRestart;
    public static event System.Action OnLivesUpdated;

    public const int kMaxLives = 3;

    static int lives = kMaxLives;
    static PlaymodeManager instance;
    int consecutiveSuccessCount;
    bool doInputCheck;
    InputManager inputManager;
    ArrowManager arrowManager;

    void OnEnable() {
        if (inputManager == null) {
            inputManager = InputManager.Instance;
        }
        if (arrowManager == null) {
            arrowManager = ArrowManager.Instance;
        }
        ArrowManager.OnNextArrow += OnNextArrow;
        InputManager.OnInputReceived += OnInputReceived;
        arrowManager.NextArrow();
    }

    void OnDisable() {
        ArrowManager.OnNextArrow -= OnNextArrow;
        InputManager.OnInputReceived -= OnInputReceived;
    }

    void Awake() {
        #region Singleton
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
            return;
        }
        #endregion

        if (enabled) {
            Debug.LogWarning("GameManager was enabled before the start!");
        }
    }

    void Update() {
        if (!inputManager.enabled) {
            return;
        }

        Countdown.Update();
        if (Countdown.IsElapsed) {
            OnTimeElapsed();
            return;
        }
    }

    void OnTimeElapsed() {
        inputManager.enabled = false;
        ArrowManager.SelectedArrow.PlayEndAnimation(false);
        ScoreManager.UpdateScore(false);
        OnWrongInput();
    }

    void OnNextArrow() {
        Countdown.Restart(ArrowManager.SelectedArrow.Duration);
    }

    void OnInputReceived(bool isInputCorrect) {
        ScoreManager.UpdateScore(isInputCorrect);
        if (isInputCorrect) {
            consecutiveSuccessCount++;
            if (consecutiveSuccessCount >= successCountToRegenerateLife) {
                consecutiveSuccessCount = 0;
                if (LivesCount < kMaxLives) {
                    LivesCount++;
                }
            }
            arrowManager.InvokeNextArrowDelayed();
        } else {
            OnWrongInput();
        }
    }

    // TODO: Rename this function since it is also called when the time is elapsed (so no input received)
    void OnWrongInput() {
        consecutiveSuccessCount = 0;
        LivesCount -= 1;
        if (LivesCount <= 0) {
            GameOver();
        } else {
            arrowManager.InvokeNextArrowDelayed();
        }
    }

    void GameOver() {
        enabled = false;
        OnGameOver?.Invoke();
    }

    public void RestartGame() {
        Invoke("EnableThisScript", restartGameDelay);
        ScoreManager.ResetScore();
        LivesCount = kMaxLives;
        OnGameRestart?.Invoke();
    }

    void EnableThisScript() {
        enabled = true;
    }
}
