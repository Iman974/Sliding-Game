using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaymodeUiManager : MonoBehaviour {

    [SerializeField] Animator uiAnimator = null;
    [SerializeField] TMP_Text scoreText = null;
    [SerializeField] TMP_Text bonusText = null;
    [SerializeField] TMP_Text highscoreText = null;
    [SerializeField] RectTransform lifeIconsContainer = null;
    [SerializeField] float gameOverFadeInDelay = 2.25f;

    Image[] lifeIconImages;
    int lifeIconIndex;

    public static PlaymodeUiManager Instance { get; private set; }

    void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
            return;
        }
        #endregion
    }

    void Start() {
        PlaymodeManager.OnLivesUpdated += UpdateLifeIcons;
        PlaymodeManager.OnGameOver += OnGameOver;

        lifeIconImages = lifeIconsContainer.GetComponentsInChildren<Image>();
        UpdateHighscoreText();
    }

    void OnDestroy() {
        PlaymodeManager.OnGameOver -= OnGameOver;
        PlaymodeManager.OnLivesUpdated -= UpdateLifeIcons;
    }

    public void UpdateScoreTextAnimated(int previousScore) {
        int currentScore = ScoreManager.PlayerScore;
        UpdateScoreText();
        if (currentScore > previousScore) {
            uiAnimator.SetTrigger("scoreIncrease");
            bonusText.text = "+" + (currentScore - previousScore);
        } else if (currentScore < previousScore) {
            uiAnimator.SetTrigger("scoreDecrease");
        }
    }

    public void UpdateScoreText() {
        scoreText.text = "Score: " + ScoreManager.PlayerScore;
    }

    public void UpdateHighscoreText() {
        highscoreText.text = "Meilleur score: " + ScoreManager.Highscore;
    }

    void UpdateLifeIcons() {
        int livesCount = PlaymodeManager.LivesCount;
        for (int i = 0; i < PlaymodeManager.kMaxLives; i++) {
            if (i < livesCount) {
                lifeIconImages[i].enabled = true;
            } else {
                lifeIconImages[i].enabled = false;
            }
        }
    }

    void OnGameOver() {
        Invoke("PlayGameOverAnimation", gameOverFadeInDelay);
    }

    void PlayGameOverAnimation() {
        uiAnimator.SetTrigger("gameOver");
    }

    public void SetStatsBool(bool value) {
        uiAnimator.SetBool("showStats", value);
    }
}
