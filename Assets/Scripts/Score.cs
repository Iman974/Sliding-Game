using UnityEngine;

public class Score : MonoBehaviour {

    [SerializeField] AnimationCurve speedReward = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    public static event System.Action<int> OnScoreUpdated;

    public static int PlayerScore { get; private set; }

    const float kPassiveBehaviorScoreLossMultiplier = 0.75f;

    Countdown countdown;

    void OnEnable() {
        GameManager.OnArrowEnd += UpdateScore;
        GameManager.OnGameRestart += ResetScore;
    }

    void Start() {
        countdown = GameManager.Countdown;
    }

    void UpdateScore(bool isSuccess) {
        Arrow currentArrow = GameManager.SelectedArrow;
        int scoreValue = currentArrow.ScoreValue;
        int newScore = PlayerScore;
        if (isSuccess) {
            // Add a percentage of the scoreValue : the faster the player solves the arrow
            // and the higher the reward is (too slow = no reward for speed)
            newScore += scoreValue /*+ (scoreValue * percentage)*/;
        } else {
            if (countdown.Elapsed) {
                newScore -= (int)(scoreValue * kPassiveBehaviorScoreLossMultiplier);
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
