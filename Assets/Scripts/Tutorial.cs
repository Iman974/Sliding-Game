using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class Tutorial : MonoBehaviour {

    [SerializeField] int requiredConsecutiveSuccesses = 8;
    [SerializeField] Transform arrowIndicatorsParent = null;
    [SerializeField] string[] instructions = null;
    [SerializeField] TMP_Text instructionText = null;

    Animator canvasAnimator;
    ProgressStep tutorialStep = ProgressStep.First;
    int[] arrowInitialWeights;
    Arrow[] arrows;
    GameObject currentActiveIndicator;
    int consecutiveSuccessCount;
    int currentInstructionIndex;
    ArrowManager arrowManager;

    enum ProgressStep {
        First,
        Second,
        Third,
        Fourth
    }

    void OnEnable() {
        InputManager.OnInputReceived += OnInputReceived;
        ArrowManager.OnNextArrow += OnNextArrow;
    }

    void OnDisable() {
        InputManager.OnInputReceived -= OnInputReceived;
        ArrowManager.OnNextArrow -= OnNextArrow;
    }

    void Start() {
        canvasAnimator = GetComponent<Animator>();
        arrowManager = ArrowManager.Instance;

        // Split the instructions string accross multiple lines (separator ';')
        for (int i = 0; i < instructions.Length; i++) {
            string s = instructions[i];
            string[] lines = s.Split(';');
            instructions[i] = string.Join("\n", lines);
        }

        arrows = ArrowManager.Arrows;
        int arrowCount = arrows.Length;
        arrowInitialWeights = new int[arrows.Length];
        for (int i = 0; i < arrows.Length; i++) {
            arrowInitialWeights[i] = arrows[i].Weight;
        }
    }

    void OnNextArrow() {
        int indicatorChildIndex = (int)ArrowManager.CurrentDesiredDirection;
        Transform indicatorTransform = arrowIndicatorsParent.GetChild(indicatorChildIndex);
        currentActiveIndicator = indicatorTransform.gameObject;
        currentActiveIndicator.SetActive(true);
    }

    void OnInputReceived(bool isInputCorrect) {
        currentActiveIndicator.SetActive(false);
        if (isInputCorrect) {
            consecutiveSuccessCount++;
        } else {
            consecutiveSuccessCount = 0;
        }
        if (consecutiveSuccessCount >= requiredConsecutiveSuccesses) {
            consecutiveSuccessCount = 0;
            canvasAnimator.SetTrigger("fadeInText");
            return;
        }
        arrowManager.InvokeNextArrowDelayed();
    }

    public void NextStep() {
        Debug.Log(tutorialStep);
        if (tutorialStep == ProgressStep.Fourth) {
            EndTutorial();
            return;
        }
        int currentTutorialStepInt = (int)tutorialStep;
        SetOnlyArrowInitialWeight(currentTutorialStepInt);
        instructionText.text = instructions[currentInstructionIndex];
        tutorialStep = (ProgressStep)(currentTutorialStepInt + 1);
        currentInstructionIndex++;
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
        tutorialStep = ProgressStep.First;
    }

    void ResetArrowsInitialWeights() {
        for (int i = 0; i < arrows.Length; i++) {
            arrows[i].Weight = arrowInitialWeights[i];
        }
    }
}
