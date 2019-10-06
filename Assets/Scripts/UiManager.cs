using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    [SerializeField] Text scoreText = null;
    [SerializeField] RectTransform lifeIconsContainer = null;
    [SerializeField] float restartPanelAppearanceDelay = 2.25f;

    Animator animator;
    GameObject[] lifeIconObjects;
    int lifeIconIndex;

    void Start() {
        GameManager.OnScoreUpdated += UpdateScoreText;
        GameManager.OnLivesUpdated += UpdateLifeIcons;
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameRestart += OnGameRestart;

        animator = GetComponent<Animator>();
        lifeIconObjects = new GameObject[lifeIconsContainer.childCount];
        for (int i = 0; i < lifeIconObjects.Length; i++) {
            lifeIconObjects[i] = lifeIconsContainer.GetChild(i).gameObject;
        }
    }

    void UpdateScoreText() {
        scoreText.text = "Score: " + GameManager.PlayerScore;
    }

    void UpdateLifeIcons() {
        for (int i = 0; i < GameManager.kMaxLives; i++) {
            if (i < GameManager.Lives) {
                lifeIconObjects[i].SetActive(true);
            } else {
                lifeIconObjects[i].SetActive(false);
            }
        }
    }

    void OnGameOver() {
        Invoke("PlayGameOverAnimation", restartPanelAppearanceDelay);
    }

    void PlayGameOverAnimation() {
        animator.SetTrigger("GameOver");
    }

    void OnGameRestart() {
        UpdateScoreText();
    }

    void OnDestroy() {
        GameManager.OnScoreUpdated -= UpdateScoreText;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameRestart -= OnGameRestart;
        GameManager.OnLivesUpdated -= UpdateLifeIcons;
    }
}
