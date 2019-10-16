using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class Tutorial : MonoBehaviour {

    //[SerializeField] int requiredConsecutiveSuccess = 8;

    //Countdown countdown;
    ProgressStep tutorialStep;
    GameManager gameManager;
    int[] arrowInitialWeights;
    Arrow[] arrows;

    void Start() {
        //countdown = GetComponent<Countdown>();
        gameManager = GetComponent<GameManager>();
        UnityEngine.Assertions.Assert.IsNotNull(gameManager, "GameManager not found!");

        arrows = gameManager.Arrows;
        int arrowCount = arrows.Length;
        arrowInitialWeights = new int[arrows.Length];
        for (int i = 0; i < arrows.Length; i++) {
            arrowInitialWeights[i] = arrows[i].Weight;
        }
    }

    public void StartTutorial() {
        tutorialStep = ProgressStep.First;
        SetOnlyArrowInitialWeight(0);
    }

    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            // Go back to main menu and leave tutorial
        }
    }

    public void NextStep() {
        if (tutorialStep == ProgressStep.Fourth) {
            return;
        }
        int newTutorialStepInt = (int)tutorialStep + 1;
        tutorialStep = (ProgressStep)newTutorialStepInt;
        SetOnlyArrowInitialWeight(newTutorialStepInt);
    }

    void SetOnlyArrowInitialWeight(int index) {
        for (int i = 0; i < arrows.Length; i++) {
            if (i != index) {
                arrows[i].Weight = 0;
                continue;
            }
            arrows[i].Weight = arrowInitialWeights[i];
        }
    }

    public void EndTutorial() {
        ResetArrowsInitialWeights();
    }

    void ResetArrowsInitialWeights() {
        for (int i = 0; i < arrows.Length; i++) {
            arrows[i].Weight = arrowInitialWeights[i];
        }
    }

    enum ProgressStep {
        First,
        Second,
        Third,
        Fourth
    }
}
