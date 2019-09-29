using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    [SerializeField] Text scoreText = null;

    void Start() {
        GameManager.BeforeNextArrow += UpdateStatsText;
    }

    void UpdateStatsText(IEventArgs args) {
        scoreText.text = "Score: " + GameManager.PlayerScore;
    }

    void OnDestroy() {
        GameManager.BeforeNextArrow -= UpdateStatsText;
    }
}
