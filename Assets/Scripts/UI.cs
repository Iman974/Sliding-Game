using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    [SerializeField] Text scoreText;
    [SerializeField] Text livesText;
    [SerializeField] Text highScoreText;
    [SerializeField] GameObject gameoverUIElementsContainer;

    void Start() {
        GameManager.OnFinalInputEvent += UpdateStatsText;
        //GameManager.OnMoveFail += UpdateStatsText;
        //highScoreText.text = "Highscore: " + GameManager.HighScore;
    }


    void UpdateStatsText(bool isSuccess) {
        scoreText.text = "Score: " + GameManager.PlayerScore;
        //livesText.text = "Lives: " + GameManager.Instance.Lives;
    }

    void OnGameOver() {
        gameoverUIElementsContainer.SetActive(true);
        //highScoreText.text = "Highscore: " + GameManager.HighScore;
    }

    void OnGameReset() {
        //UpdateStatsText();
    }

    void OnDestroy() {
        GameManager.OnFinalInputEvent -= UpdateStatsText;
    }
}
