using UnityEngine;

public class ScoreManager : MonoBehaviour {

    [SerializeField] AnimationCurve speedRewardCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    public static event System.Action<int> OnScoreUpdated;

    public static int PlayerScore { get; private set; }

    const float kNoTryScoreLossMultiplier = 0.75f;

    Countdown countdown;

    void OnEnable() {
        GameManager.OnArrowEnd += UpdateScore;
        GameManager.OnGameRestart += ResetScore;
    }

    void Start() {
        countdown = GameManager.Countdown;
    }

    void UpdateScore(bool hasScored) {
        Arrow currentArrow = GameManager.SelectedArrow;
        int scoreValue = currentArrow.ScoreValue;
        int newScore = PlayerScore;
        if (hasScored) {
            // The faster the player scores and the higher the reward is
            float speedPercentage = countdown.RemainingTime / currentArrow.Duration;
            int reward = (int)(speedRewardCurve.Evaluate(speedPercentage) * scoreValue);
            newScore += scoreValue + reward;
        } else {
            if (countdown.IsElapsed) {
                newScore -= (int)(scoreValue * kNoTryScoreLossMultiplier);
            } else {
                newScore -= (int)(scoreValue * currentArrow.ScoreLossMultiplier);
            }
        }

        SetPoints(newScore);
    }

    void ResetScore() {
        PlayerScore = 0;
    }

    void SetPoints(int value) {
        int previousScore = PlayerScore;
        PlayerScore = Mathf.Max(0, value);
        OnScoreUpdated?.Invoke(previousScore);
    }

    void OnDisable() {
        GameManager.OnArrowEnd -= UpdateScore;
        GameManager.OnGameRestart -= ResetScore;
    }
}
