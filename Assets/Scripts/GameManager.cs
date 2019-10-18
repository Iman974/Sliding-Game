using UnityEngine;

[RequireComponent(typeof(Countdown), typeof(LivesManager))]
public class GameManager : MonoBehaviour {

    [SerializeField] Arrow[] arrows = null;
    //[SerializeField] float speedGainOverProgression = 0.002f;
    //[SerializeField] float maxPlaybackSpeed = 2f;
    [SerializeField] float restartGameDelay = 1.5f;

    public static int Highscore { get; private set; }

    public static event System.Action OnGameOver;
    public static event System.Action OnGameRestart;

    static GameManager instance;
    //float playbackSpeed = 1f;
    Countdown countdown;
    bool doInputCheck;
    InputManager inputManager;
    ArrowManager arrowManager;

    void OnEnable() {
        if (countdown == null) {
            countdown = GetComponent<Countdown>();
        }
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

        for (int i = 0; i < arrows.Length; i++) {
            UnityEngine.Assertions.Assert.IsNotNull(arrows[i], "Missing arrow!");
        }
        Highscore = ProgressSaver.LoadHighscore();
        if (enabled) {
            Debug.LogWarning("GameManager was enabled before the start!");
        }
    }

    void Update() {
        if (!inputManager.enabled) {
            return;
        }

        if (countdown.IsElapsed) {
            OnTimeElapsed();
            return;
        }
    }

    void OnTimeElapsed() {
        inputManager.enabled = false;
        ArrowManager.SelectedArrow.PlayEndAnimation(false);
        OnWrongInput();
    }

    void OnNextArrow() {
        countdown.Restart(ArrowManager.SelectedArrow.Duration);
    }

    public void OnInputReceived(bool isInputCorrect) {
        if (isInputCorrect) {
            //playbackSpeed = Mathf.Min(maxPlaybackSpeed, playbackSpeed + speedGainOverProgression);
            arrowManager.InvokeNextArrowDelayed();
        } else {
            OnWrongInput();
        }
    }

    // TODO: Rename this function since it is also called when the time is elapsed (so no input received)
    void OnWrongInput() {
        // TODO: So is LivesManager really necessary now ? Rewind changes or make it a plain CSharp class
        if (LivesManager.LivesCount <= 0) {
            GameOver();
        } else {
            arrowManager.InvokeNextArrowDelayed();
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
        //playbackSpeed = 1f;
        OnGameRestart?.Invoke();
    }

    void EnableThisScript() {
        enabled = true;
    }
}
