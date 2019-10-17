using UnityEngine;

[RequireComponent(typeof(Countdown), typeof(LivesManager))]
public class GameManager : MonoBehaviour {

    [SerializeField] Arrow[] arrows = null;
    [SerializeField] float nextDelay = 0.3f;
    [SerializeField] float speedGainOverProgression = 0.002f;
    [SerializeField] float maxPlaybackSpeed = 2f;
    [SerializeField] float restartGameDelay = 1.5f;

    public static Arrow SelectedArrow { get; private set; }
    public static int Highscore { get; private set; }

    public Arrow[] Arrows => arrows;
    public Direction CurrentDesiredDirection { get; private set; }

    public static event System.Action<bool> OnArrowEnd;
    public static event System.Action OnNextArrow;
    public static event System.Action OnGameOver;
    public static event System.Action OnGameRestart;

    static GameManager instance;
    Direction inputDirection;
    float playbackSpeed = 1f;
    Countdown countdown;
    bool doInputCheck;

    void OnEnable() {
        if (countdown == null) {
            countdown = GetComponent<Countdown>();
        }
        NextArrow();
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
            ResetSelectedArrow();
        }

        // Randomly select an arrow with weighted probability
        SelectedArrow = arrows[RandomUtility.SelectRandomWeightedIndex(arrows)];

        CurrentDesiredDirection = DirectionUtility.GetRandomDirection();
        SelectedArrow.SetOrientation(CurrentDesiredDirection);
        SelectedArrow.IsActive = true;
        countdown.Restart(SelectedArrow.Duration);
        doInputCheck = true;
        OnNextArrow?.Invoke();
    }

    public void ResetSelectedArrow() {
        SelectedArrow.IsActive = false;
        SelectedArrow.ResetTransform();
    }

    void HandleReceivedInput() {
        bool isInputCorrect = inputDirection == CurrentDesiredDirection;
        OnArrowEnd?.Invoke(isInputCorrect);
        if (isInputCorrect) {
            playbackSpeed = Mathf.Min(maxPlaybackSpeed, playbackSpeed + speedGainOverProgression);
            StartCoroutine(InvokeNextArrowDelayed());
        } else {
            OnWrongInput();
        }
        doInputCheck = false;
    }

    void OnWrongInput() {
        if (LivesManager.LivesCount <= 0) {
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
        playbackSpeed = 1f;
        OnGameRestart?.Invoke();
    }

    void EnableThisScript() {
        enabled = true;
    }
}
