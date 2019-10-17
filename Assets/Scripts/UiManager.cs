using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]
public class UiManager : MonoBehaviour {

    [SerializeField] TMP_Text scoreText = null;
    [SerializeField] TMP_Text highscoreText = null;
    [SerializeField] RectTransform lifeIconsContainer = null;
    [SerializeField] float gameOverFadeInDelay = 2.25f;

    Animator animator;
    Image[] lifeIconImages;
    int lifeIconIndex;

    void Start() {
        ScoreManager.OnScoreUpdated += UpdateScoreText;
        LivesManager.OnLivesUpdated += UpdateLifeIcons;
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameRestart += OnGameRestart;

        animator = GetComponent<Animator>();
        UnityEngine.Assertions.Assert.IsNotNull(animator, "Animator not found on Ui!");
        lifeIconImages = lifeIconsContainer.GetComponentsInChildren<Image>();
        UpdateHighscoreText();
    }

    void UpdateScoreText(int previousScore) {
        int currentScore = ScoreManager.PlayerScore;
        scoreText.text = "Score : " + currentScore;
        if (currentScore > previousScore) {
            animator.SetTrigger("scoreIncrease");
        } else if (currentScore < previousScore) {
            animator.SetTrigger("scoreDecrease");
        }
    }

    void UpdateHighscoreText() {
        highscoreText.text = "Meilleur score : " + GameManager.Highscore;
    }

    void UpdateLifeIcons() {
        int livesCount = LivesManager.LivesCount;
        for (int i = 0; i < LivesManager.kMaxLives; i++) {
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
        animator.SetTrigger("gameOver");
    }

    public void SetStatsBool(bool value) {
        animator.SetBool("showStats", value);
    }

    void OnGameRestart() {
        scoreText.text = "Score: " + ScoreManager.PlayerScore;
    }

    void OnDestroy() {
        ScoreManager.OnScoreUpdated -= UpdateScoreText;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameRestart -= OnGameRestart;
        LivesManager.OnLivesUpdated -= UpdateLifeIcons;
    }
}
