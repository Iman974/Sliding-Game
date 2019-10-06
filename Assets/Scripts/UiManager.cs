using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    [SerializeField] Text scoreText = null;
    [SerializeField] float restartPanelAppearanceDelay = 2.25f;

    Animator animator;

    void Start() {
        GameManager.BeforeNextArrow += UpdateStatsText;
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameRestart += OnGameRestart;

        animator = GetComponent<Animator>();
    }

    void UpdateStatsText(bool isSuccess) {
        scoreText.text = "Score: " + GameManager.PlayerScore;
    }

    void OnGameOver() {
        Invoke("PlayGameOverAnimation", restartPanelAppearanceDelay);
    }

    void PlayGameOverAnimation() {
        animator.SetTrigger("GameOver");
    }

    void OnGameRestart() {
        UpdateStatsText(false);
    }

    void OnDestroy() {
        GameManager.BeforeNextArrow -= UpdateStatsText;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameRestart -= OnGameRestart;
    }
}
