using UnityEngine;

[RequireComponent(typeof(PlaymodeManager))]
public class ScoreManager : MonoBehaviour {

    public static int PlayerScore { get; private set; }
    public static int Highscore {
        get => highscore;
        set {
            highscore = value;
            uiManager.UpdateHighscoreText();
        }
    }

    const float kNoTryScoreLossMultiplier = 0.75f;

    static Countdown countdown;
    static ScoreManager instance;
    static int highscore;
    static PlaymodeUiManager uiManager;

    void OnEnable() {
        PlaymodeManager.OnGameRestart += ResetScore;
        PlaymodeManager.OnGameOver += OnGameOver;
    }

    void OnDisable() {
        PlaymodeManager.OnGameRestart -= ResetScore;
        PlaymodeManager.OnGameOver -= OnGameOver;
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
    }

    void Start() {
        countdown = PlaymodeManager.Countdown;
        uiManager = PlaymodeUiManager.Instance;
        Highscore = ProgressSaver.LoadHighscore();
    }

    public static void UpdateScore(bool hasScored) {
        Arrow selectedArrow = ArrowManager.SelectedArrow;
        int scoreValue = selectedArrow.ScoreValue;
        int newScore = PlayerScore;
        if (hasScored) {
            // The faster the player scores and the higher the reward is
            float speedPercentage = countdown.RemainingTime / selectedArrow.Duration;
            AnimationCurve rewardCurve = selectedArrow.SpeedRewardCurve;
            int reward = (int)(rewardCurve.Evaluate(speedPercentage) * scoreValue);
            newScore += scoreValue + reward;
        } else {
            if (countdown.IsElapsed) {
                newScore -= (int)(scoreValue * kNoTryScoreLossMultiplier);
            } else {
                newScore -= (int)(scoreValue * selectedArrow.ScoreLossMultiplier);
            }
        }

        SetPoints(newScore);
    }

    void OnGameOver() {
        if (PlayerScore > Highscore) {
            Highscore = PlayerScore;
            ProgressSaver.SaveHighscore(Highscore);
        }
    }

    public static void ResetScore() {
        PlayerScore = 0;
        uiManager.UpdateScoreText();
    }

    static void SetPoints(int newScore) {
        int previousScore = PlayerScore;
        PlayerScore = Mathf.Max(0, newScore);
        uiManager.UpdateScoreTextAnimated(previousScore);
    }
}
