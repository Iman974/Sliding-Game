using UnityEngine;

public class Game : MonoBehaviour {

    [Tooltip("The threshold that need to be reached in order to consider the sliding.")]
    [SerializeField] private float slidingSensibility = 0.25f;
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float accelerationFactor = 1.03f;
    [SerializeField] private float maxAcceleration = 1.75f;

    [Header("")]
    [SerializeField] private Arrow[] arrowPrefabs;

    private static GameState gameState = new GameState();
    private static string gameStatePath;

    private float stayDuration;
    private float nextDelay;
    private float initialAcceleration;
    private bool checkInput;
    private Countdown countdown;

    public static event System.Action<bool> OnInputValidationEvent;
    public static event System.Action OnMissedEvent;
    public static event System.Action OnGameOverEvent;
    public static event System.Action OnGameResetEvent;

    public static Game Instance { get; private set; }
    public static Direction CurrentDirection { get; private set; }
    public static Arrow CurrentArrow { get; private set; }
    public static int PlayerScore { get; private set; }
    public static int HighScore { get { return gameState.highScore; } }

    public int Lives { get; private set; }

    private void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
            return;
        }
        #endregion

        slidingSensibility *= slidingSensibility;
        accelerationFactor = 1f / accelerationFactor;
        initialAcceleration = accelerationFactor;
        maxAcceleration = 1f / maxAcceleration;

        countdown = GetComponent<Countdown>();
        countdown.Elapsed += OnCountDownElapsed;

        gameStatePath = Application.persistentDataPath + "/gameState.sav";
        if (StateSaver.StateExists(gameStatePath)) {
             StateSaver.RetrieveStateFromJson(gameStatePath, gameState);
        }
    }

    private void OnEnable() {
        Lives = maxLives;

        countdown.Begin();
        Next();
    }

    private void OnCountDownElapsed() {
        checkInput = false;
        Skip();
    }

    private void Update() {
        if (!checkInput) {
            return;
        }

        if (Input.touchCount > 0 && InputUtility.HasTouchMoved(slidingSensibility)) {
            countdown.Stop();
            Touch touch = Input.GetTouch(0);

            Direction inputDirection = DirectionUtility.VectorToDirection(touch.deltaPosition);
            bool hasScored = IsMovementValid(inputDirection);
            UpdateScore(hasScored);

            if (OnInputValidationEvent != null) {
                OnInputValidationEvent(hasScored);
            }
            if (!hasScored && Lives <= 0) {
                GameOver();
                return;
            }

            checkInput = false;
            Invoke("Next", nextDelay);
        }
    }

    private bool IsMovementValid(Direction inputDirection) {
        return inputDirection == CurrentDirection;
    }

    private void UpdateScore(bool hasScored) {
        if (hasScored) {
            PlayerScore += CalculateScore();
        } else {
            PlayerScore -= CurrentArrow.ScoreValue;
            Lives--;

            if (PlayerScore < 0) {
                PlayerScore = 0;
            }
        }
    }

    private void Next() {
        CurrentDirection = RandomUtility.PickRandomItemInArray(DirectionUtility.DirectionValues);
        CurrentArrow = Instantiate(RandomUtility.PickRandomItemInArray(arrowPrefabs));

        UpdateDurationsAndDelays();
        countdown.WaitTime = stayDuration * accelerationFactor;
        countdown.Restart();
        if (accelerationFactor > maxAcceleration) {
            accelerationFactor *= initialAcceleration;
        }

        checkInput = true;
    }

    private void UpdateDurationsAndDelays() {
        stayDuration = CurrentArrow.StayDuration;
        nextDelay = CurrentArrow.NextDelay;
    }

    private void Skip() {
        Invoke("Next", CurrentArrow.SkipDelay);

        if (OnMissedEvent != null) {
            OnMissedEvent();
        }
    }

    private int CalculateScore() {
        return Mathf.RoundToInt((CurrentArrow.ScoreValue * countdown.ElapsedTime) / stayDuration);
    }

    private void GameOver() {
        if (PlayerScore > gameState.highScore) {
            gameState.highScore = PlayerScore;
            StateSaver.SaveStateAsJson(gameStatePath, gameState);
        }

        if (OnGameOverEvent != null) {
            OnGameOverEvent();
        }

        enabled = false;
    }

    public void ResetGame() {
        Lives = maxLives;
        PlayerScore = 0;

        if (OnGameResetEvent != null) {
            OnGameResetEvent();
        }
    }

    private void OnDestroy() {
        countdown.Elapsed -= OnCountDownElapsed;
    }
}
