using UnityEngine;
using TMPro;

[RequireComponent(typeof(GameManager), typeof(Animator))]
public class Tutorial : MonoBehaviour {

    [SerializeField] int requiredConsecutiveSuccesses = 8;
    [SerializeField] Transform arrowIndicatorsParent = null;
    [SerializeField] string[] instructions = null;
    [SerializeField] TMP_Text instructionText = null;
    [SerializeField] TMP_Text endText = null;
    [SerializeField] Animator canvasAnimator = null;

    ProgressStep tutorialStep;
    GameManager gameManager;
    int[] arrowInitialWeights;
    Arrow[] arrows;
    GameObject currentActiveIndicator;
    int consecutiveSuccessCount;
    int currentInstructionIndex;

    enum ProgressStep {
        First,
        Second,
        Third,
        Fourth
    }

    void OnEnable() {
        GameManager.OnArrowEnd += OnArrowEnd;
        GameManager.OnNextArrow += OnNextArrow;
        StartTutorial();
    }

    void Awake() {
        gameManager = GetComponent<GameManager>();
        UnityEngine.Assertions.Assert.IsNotNull(gameManager, "GameManager not found!");

        arrows = gameManager.Arrows;
        int arrowCount = arrows.Length;
        arrowInitialWeights = new int[arrows.Length];
        for (int i = 0; i < arrows.Length; i++) {
            arrowInitialWeights[i] = arrows[i].Weight;
        }

        for (int i = 0; i < instructions.Length; i++) {
            string s = instructions[i];
            string[] lines = s.Split(';');
            instructions[i] = string.Join("\n", lines);
        }
    }

    void StartTutorial() {
        tutorialStep = ProgressStep.First;
        SetOnlyArrowInitialWeight(0);
    }

    void OnNextArrow() {
        if (consecutiveSuccessCount >= requiredConsecutiveSuccesses) {
            consecutiveSuccessCount = 0;
            NextStep();
            return;
        }

        int indicatorChildIndex = (int)gameManager.CurrentDesiredDirection;
        Transform indicatorTransform = arrowIndicatorsParent.GetChild(indicatorChildIndex);
        currentActiveIndicator = indicatorTransform.gameObject;
        currentActiveIndicator.SetActive(true);
    }

    void OnArrowEnd(bool hasScored) {
        currentActiveIndicator.SetActive(false);
        if (hasScored) {
            consecutiveSuccessCount++;
        } else {
            consecutiveSuccessCount = 0;
        }
    }

    public void NextStep() {
        if (tutorialStep == ProgressStep.Fourth) {
            EndTutorial();
            return;
        }
        int newTutorialStepInt = (int)tutorialStep + 1;
        tutorialStep = (ProgressStep)newTutorialStepInt;
        SetOnlyArrowInitialWeight(newTutorialStepInt);
        instructionText.text = instructions[currentInstructionIndex];
        currentInstructionIndex++;
        canvasAnimator.SetTrigger("fadeInText");
        gameManager.ResetSelectedArrow();
        gameManager.enabled = false;
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

    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            // Go back to main menu and leave tutorial
            // after confirmation (second getbuttondown)
        }
    }

    public void EndTutorial() {
        ResetArrowsInitialWeights();
        canvasAnimator.SetTrigger("ending");
    }

    void ResetArrowsInitialWeights() {
        for (int i = 0; i < arrows.Length; i++) {
            arrows[i].Weight = arrowInitialWeights[i];
        }
    }

    void OnDisable() {
        GameManager.OnArrowEnd -= OnArrowEnd;
        GameManager.OnNextArrow -= OnNextArrow;
    }
}
