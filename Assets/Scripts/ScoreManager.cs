using UnityEngine;

[RequireComponent(typeof(PlaymodeManager))]
public class ScoreManager : MonoBehaviour {

    public static event System.Action<int> OnScoreUpdated;

    public static int PlayerScore { get; private set; }

    const float kNoTryScoreLossMultiplier = 0.75f;

    Countdown countdown;

    void OnEnable() {
        Arrow.OnArrowEnd += UpdateScore;
        PlaymodeManager.OnGameRestart += ResetScore;
    }

    void Start() {
        countdown = PlaymodeManager.Countdown;
    }

    void UpdateScore(bool hasScored) {
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

    void ResetScore() {
        PlayerScore = 0;
    }

    void SetPoints(int value) {
        int previousScore = PlayerScore;
        PlayerScore = Mathf.Max(0, value);
        OnScoreUpdated?.Invoke(previousScore);
    }

    void OnDisable() {
        Arrow.OnArrowEnd -= UpdateScore;
        PlaymodeManager.OnGameRestart -= ResetScore;
    }
}
