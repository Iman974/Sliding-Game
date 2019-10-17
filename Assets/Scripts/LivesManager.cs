using UnityEngine;

public class LivesManager : MonoBehaviour {

    [SerializeField] int successCountToRegenerateLife = 10;

    public static int LivesCount {
        get => lives;
        private set {
            lives = value;
            OnLivesUpdated?.Invoke();
        }
    }

    public static event System.Action OnLivesUpdated;

    public const int kMaxLives = 3;

    static int lives = kMaxLives;
    int consecutiveSuccessCount;

    void OnEnable() {
        GameManager.OnArrowEnd += OnArrowEnd;
        GameManager.OnGameRestart += OnGameRestart;
    }

    void OnArrowEnd(bool hasScored) {
        if (hasScored) {
            consecutiveSuccessCount++;
            if (consecutiveSuccessCount >= successCountToRegenerateLife) {
                consecutiveSuccessCount = 0;
                if (LivesCount < kMaxLives) {
                    LivesCount++;
                }
            }
        } else {
            consecutiveSuccessCount = 0;
            LivesCount -= 1;
        }
    }

    void OnGameRestart() {
        LivesCount = kMaxLives;
    }

    void OnDisable() {
        GameManager.OnArrowEnd -= OnArrowEnd;
        GameManager.OnGameRestart -= OnGameRestart;
    }
}
