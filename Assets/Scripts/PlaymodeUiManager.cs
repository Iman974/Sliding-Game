using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaymodeUiManager : MonoBehaviour {

    [SerializeField] Animator uiAnimator = null;
    [SerializeField] TMP_Text scoreText = null;
    [SerializeField] TMP_Text highscoreText = null;
    [SerializeField] RectTransform lifeIconsContainer = null;
    [SerializeField] float gameOverFadeInDelay = 2.25f;

    Image[] lifeIconImages;
    int lifeIconIndex;

    void Start() {
        ScoreManager.OnScoreUpdated += UpdateScoreText;
        PlaymodeManager.OnLivesUpdated += UpdateLifeIcons;
        PlaymodeManager.OnGameOver += OnGameOver;
        PlaymodeManager.OnGameRestart += OnGameRestart;

        lifeIconImages = lifeIconsContainer.GetComponentsInChildren<Image>();
        UpdateHighscoreText();
    }

    void OnDestroy() {
        ScoreManager.OnScoreUpdated -= UpdateScoreText;
        PlaymodeManager.OnGameOver -= OnGameOver;
        PlaymodeManager.OnGameRestart -= OnGameRestart;
        PlaymodeManager.OnLivesUpdated -= UpdateLifeIcons;
    }

    void UpdateScoreText(int previousScore) {
        int currentScore = ScoreManager.PlayerScore;
        scoreText.text = "Score : " + currentScore;
        if (currentScore > previousScore) {
            uiAnimator.SetTrigger("scoreIncrease");
        } else if (currentScore < previousScore) {
            uiAnimator.SetTrigger("scoreDecrease");
        }
    }

    void UpdateHighscoreText() {
        highscoreText.text = "Meilleur score : " + PlaymodeManager.Highscore;
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
        UpdateHighscoreText();
    }

    void PlayGameOverAnimation() {
        uiAnimator.SetTrigger("gameOver");
    }

    public void SetStatsBool(bool value) {
        uiAnimator.SetBool("showStats", value);
    }

    void OnGameRestart() {
        scoreText.text = "Score: " + ScoreManager.PlayerScore;
    }
}
