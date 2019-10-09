using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]
public class UiManager : MonoBehaviour {

    [SerializeField] TMP_Text scoreText = null;
    [SerializeField] TMP_Text highscoreText = null;
    [SerializeField] RectTransform lifeIconsContainer = null;
    [SerializeField] float restartPanelAppearanceDelay = 2.25f;

    Animator animator;
    Image[] lifeIconImages;
    int lifeIconIndex;

    void Start() {
        GameManager.OnScoreUpdated += UpdateScoreText;
        GameManager.OnLivesUpdated += UpdateLifeIcons;
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameRestart += OnGameRestart;

        animator = GetComponent<Animator>();
        lifeIconImages = lifeIconsContainer.GetComponentsInChildren<Image>();
        UpdateHighscoreText();
    }

    void UpdateScoreText(int previousScore) {
        int currentScore = GameManager.PlayerScore;
        scoreText.text = "Score : " + currentScore;
        if (currentScore > previousScore) {
            animator.SetTrigger("ScorePulsation");
        } else if (previousScore < currentScore) {
            animator.SetTrigger("ScoreColor");
        }
    }

    void UpdateHighscoreText() {
        highscoreText.text = "Meilleur score : " + GameManager.Highscore;
    }

    void UpdateLifeIcons() {
        for (int i = 0; i < GameManager.kMaxLives; i++) {
            if (i < GameManager.Lives) {
                lifeIconImages[i].enabled = true;
            } else {
                lifeIconImages[i].enabled = false;
            }
        }
    }

    void OnGameOver() {
        Invoke("PlayGameOverAnimation", restartPanelAppearanceDelay);
        UpdateHighscoreText();
    }

    void PlayGameOverAnimation() {
        animator.SetTrigger("GameOver");
    }

    void OnGameRestart() {
        scoreText.text = "Score: " + GameManager.PlayerScore;
    }

    void OnDestroy() {
        GameManager.OnScoreUpdated -= UpdateScoreText;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameRestart -= OnGameRestart;
        GameManager.OnLivesUpdated -= UpdateLifeIcons;
    }
}
