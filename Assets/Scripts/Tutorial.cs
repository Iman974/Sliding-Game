using UnityEngine;

[RequireComponent(typeof(GameManager), typeof(Animator))]
public class Tutorial : MonoBehaviour {

    //[SerializeField] int requiredConsecutiveSuccess = 8;
    [SerializeField] Transform arrowIndicatorsParent = null;

    ProgressStep tutorialStep;
    GameManager gameManager;
    int[] arrowInitialWeights;
    Arrow[] arrows;
    GameObject currentActiveIndicator;
    Animator animator;

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
        //countdown = GetComponent<Countdown>();
        gameManager = GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        UnityEngine.Assertions.Assert.IsNotNull(gameManager, "GameManager not found!");

        arrows = gameManager.Arrows;
        int arrowCount = arrows.Length;
        arrowInitialWeights = new int[arrows.Length];
        for (int i = 0; i < arrows.Length; i++) {
            arrowInitialWeights[i] = arrows[i].Weight;
        }
    }

    void StartTutorial() {
        tutorialStep = ProgressStep.First;
        SetOnlyArrowInitialWeight(0);
        //gameManager.SetLivesCount(int.MaxValue);
    }

    void OnNextArrow() {
        int indicatorChildIndex = (int)gameManager.CurrentDesiredDirection;
        Transform indicatorTransform = arrowIndicatorsParent.GetChild(indicatorChildIndex);
        currentActiveIndicator = indicatorTransform.gameObject;
        currentActiveIndicator.SetActive(true);
    }

    void OnArrowEnd(bool hasScored) {
        currentActiveIndicator.SetActive(false);
    }

    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            // Go back to main menu and leave tutorial
            // after confirmation (second getbuttondown)
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

    void OnDisable() {
        GameManager.OnArrowEnd -= OnArrowEnd;
        GameManager.OnNextArrow -= OnNextArrow;
    }
}
