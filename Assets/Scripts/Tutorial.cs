using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour {

    [SerializeField] int requiredConsecutiveSuccesses = 8;
    [SerializeField] float textFadeInDelay = 0.4f;
    [SerializeField] float endingDelay = 0.5f;
    [SerializeField] Transform arrowIndicatorsParent = null;
    [SerializeField] string[] instructions = null;
    [SerializeField] TMP_Text instructionText = null;
    [SerializeField] Animator canvasAnimator = null;

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
        int indicatorChildIndex = (int)ArrowManager.DesiredDirection;
        Transform indicatorTransform = arrowIndicatorsParent.GetChild(indicatorChildIndex);
        currentActiveIndicator = indicatorTransform.gameObject;
        currentActiveIndicator.SetActive(true);
    }

    void OnInputReceived(bool isInputCorrect) {
        currentActiveIndicator.SetActive(false);
        if (isInputCorrect) {
            consecutiveSuccessCount++;
            if (consecutiveSuccessCount >= requiredConsecutiveSuccesses) {
                consecutiveSuccessCount = 0;
                NextStep();
                return;
            }
        } else {
            const int kFailBackCount = 4;
            consecutiveSuccessCount = Mathf.Max(0, consecutiveSuccessCount - kFailBackCount);
        }
        arrowManager.InvokeNextArrowDelayed();
    }

    void NextStep() {
        if (currentTutorialStep == kArrowTypeCount) {
            EndTutorial();
            StartCoroutine(SetAnimatorTrigger(endingDelay, "ending"));
            return;
        }
        SetOnlyArrowInitialWeight(currentTutorialStep);
        instructionText.text = instructions[currentTutorialStep];
        currentTutorialStep++;
        StartCoroutine(SetAnimatorTrigger(textFadeInDelay, "fadeInText"));
    }

    System.Collections.IEnumerator SetAnimatorTrigger(float delay, string triggerName) {
        yield return new WaitForSeconds(delay);
        canvasAnimator.SetTrigger(triggerName);
    }

    void EndTutorial() {
        ResetArrowsInitialWeights();
        currentTutorialStep = 0;
        enabled = false;
    }

    void ResetArrowsInitialWeights() {
        for (int i = 0; i < arrows.Length; i++) {
            arrows[i].Weight = arrowInitialWeights[i];
        }
    }
}
