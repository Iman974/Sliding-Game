using UnityEngine;

[RequireComponent(typeof(GameManager), typeof(ScoreManager))]
public class Tutorial : MonoBehaviour {

    [SerializeField] int requiredConsecutiveSuccess = 8;

    Countdown countdown;
    ProgressStep tutorialStep;
    GameManager gameManager;
    ScoreManager scoreManager;
    int[] arrowInitialWeights;

    void Start() {
        countdown = GameManager.Countdown;
        gameManager = GetComponent<GameManager>();
        scoreManager = GetComponent<ScoreManager>();
        UnityEngine.Assertions.Assert.IsNotNull(gameManager, "GameManager not found!");

        Arrow[] arrows = gameManager.Arrows;
        int arrowCount = arrows.Length;
        arrowInitialWeights = new int[arrows.Length];
        for (int i = 0; i < arrows.Length; i++) {
            arrowInitialWeights[i] = arrows[i].Weight;
        }
    }

    public void StartTutorial() {
        tutorialStep = ProgressStep.First;
        SetArrowInitialWeight(0);
        countdown.Stop();
    }

    public void NextStep() {
        if (tutorialStep == ProgressStep.Fourth) {
            return;
        }
        tutorialStep = (ProgressStep)((int)tutorialStep + 1);
        
    }

    void SetArrowInitialWeight(params int[] indexes) {
        Arrow[] arrows = gameManager.Arrows;
        for (int i = 0; i < indexes.Length; i++) {
            int arrowIndex = indexes[i];
            arrows[arrowIndex].Weight = arrowInitialWeights[arrowIndex];
        }
    }

    enum ProgressStep {
        First,
        Second,
        Third,
        Fourth
    }
}
