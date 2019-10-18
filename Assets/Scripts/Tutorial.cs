using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour {

    [SerializeField] int requiredConsecutiveSuccesses = 8;
    [SerializeField] float textFadeInDelay = 0.4f;
    [SerializeField] Transform arrowIndicatorsParent = null;
    [SerializeField] string[] instructions = null;
    [SerializeField] TMP_Text instructionText = null;
    [SerializeField] Animator canvasAnimator = null;
    [SerializeField] UnityEvent onTutorialEnd;

    const int kArrowTypeCount = 4;

    int[] arrowInitialWeights;
    Arrow[] arrows;
    GameObject currentActiveIndicator;
    int consecutiveSuccessCount;
    int currentTutorialStep;
    ArrowManager arrowManager;

    void OnEnable() {
        InputManager.OnInputReceived += OnInputReceived;
        ArrowManager.OnNextArrow += OnNextArrow;
        NextStep();
    }

    void OnDisable() {
        InputManager.OnInputReceived -= OnInputReceived;
        ArrowManager.OnNextArrow -= OnNextArrow;
    }

    void Awake() {
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

    void SetOnlyArrowInitialWeight(int index) {
        for (int i = 0; i < arrows.Length; i++) {
            if (i != index) {
                arrows[i].Weight = 0;
                continue;
            }
            arrows[i].Weight = arrowInitialWeights[i];
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
            NextStep();
            Invoke("SetTextFadeInTrigger", textFadeInDelay);
            return;
        }
        arrowManager.InvokeNextArrowDelayed();
    }

    void NextStep() {
        if (currentTutorialStep == kArrowTypeCount) {
            EndTutorial();
            return;
        }
        SetOnlyArrowInitialWeight(currentTutorialStep);
        instructionText.text = instructions[currentTutorialStep];
        currentTutorialStep++;
    }

    void SetTextFadeInTrigger() {
        canvasAnimator.SetTrigger("fadeInText");
    }

    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            // Go back to main menu and leave tutorial
            // after confirmation (second getbuttondown)
        }
    }

    public void EndTutorial() {
        ResetArrowsInitialWeights();
        //canvasAnimator.SetTrigger("ending");
        currentTutorialStep = 0;
        enabled = false;
        onTutorialEnd.Invoke();
    }

    void ResetArrowsInitialWeights() {
        for (int i = 0; i < arrows.Length; i++) {
            arrows[i].Weight = arrowInitialWeights[i];
        }
    }
}
