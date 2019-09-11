using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    [SerializeField] float swipingSensibility = 0.25f;
    [SerializeField] int maxLives = 3;

    public static GameManager Instance { get; private set; }
    public static Direction CurrentDirection { get; private set; }
    public static int PlayerScore { get; private set; }

    public int Lives { get; private set; }

    Direction inputDirection;
    Direction desiredDirection;
    Direction displayedDirection;
    float countdown;
    Arrow selectedArrow;
    int currentMoveIndex;
    Vector2 previousMousePos;
    bool animationPhase;

    void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
            return;
        }
        #endregion
    }

    void Update() {
        if (!animationPhase) {
            countdown -= Time.deltaTime;
        }
        if (countdown <= 0f) {
            NextArrow();
        }

        HandleInput();
    }

    void HandleInput() {
        if (!CheckInput(ref inputDirection)) {
            return;
        }

        if (currentMoveIndex < selectedArrow.MoveCount) {
            int move = selectedArrow.GetMove(currentMoveIndex);
            if ((int)inputDirection == (move + (int)displayedDirection) %
                    DirectionUtility.kDirectionCount) {
                // The input matches the move
                currentMoveIndex++;
            } else {
                
            }
        } else if (inputDirection == desiredDirection) {
            // The arrow has been oriented successfully and the final move is right
            selectedArrow.TriggerAnimation(Arrow.Animation.Move);
            currentMoveIndex = 0;
            animationPhase = true;
        } else {
            // Wrong input on the scoring/final move
        }
    }

    bool CheckInput(ref Direction input) {
#if UNITY_EDITOR
        if (!Input.GetMouseButton(0)) {
            return false;
        }

        //Touch touch = Input.GetTouch(0);
        Vector2 deltaPos = previousMousePos - (Vector2)Input.mousePosition;
        previousMousePos = Input.mousePosition;
        float sqrSensibility = swipingSensibility * swipingSensibility;
        if (deltaPos.sqrMagnitude < sqrSensibility) {
            return false;
        }
        Debug.Log("Input Write");
        input = DirectionUtility.VectorToDirection(deltaPos);
        return true;
#elif UNITY_ANDROID
        if (Input.touchCount == 0) {
            return false;
        }

        Touch touch = Input.GetTouch(0);
        Vector2 deltaPos = touch.deltaPosition;
        float sqrSensibility = swipingSensibility * swipingSensibility;
        if (deltaPos.sqrMagnitude < sqrSensibility) {
            return false;
        }
        input = DirectionUtility.VectorToDirection(deltaPos);
        return true;
#endif
    }

    // Hide the previous arrow, randomly select a new one, rotate it and show it.
    void NextArrow() {
        if (selectedArrow != null) {
            selectedArrow.IsActive = false;
        }

        desiredDirection = DirectionUtility.GetRandomDirection();
        Arrow[] arrows = ArrowPool.Instance.Arrows;

        int weightSum = arrows.Sum(a => a.Weight);
        for (int i = 0; i < arrows.Length; i++) {
            if (Random.Range(0, weightSum) < arrows[i].Weight) {
                selectedArrow = arrows[i];
                break;
            }
            weightSum -= arrows[i].Weight;
        }

        displayedDirection = (Direction)(((int)desiredDirection +
            selectedArrow.DisplayedDirectionModifier) % DirectionUtility.kDirectionCount);

        selectedArrow.Orientation = displayedDirection;
        selectedArrow.IsActive = true;
        countdown = selectedArrow.Duration;
        animationPhase = false;
    }

    public void OnAnimationEnd() {
        animationPhase = false;
        NextArrow();
    }

    // While or for loop ? make a choice. Algorithm (to be improved by
    // using random function only once) from the website
    // https://forum.unity.com/threads/random-numbers-with-a-weighted-chance.442190/
    int SelectRandomWeightedIndex(int[] weights) {
        int weightSum = weights.Sum();
        for (int i = 0; i < weights.Length - 1; i++) {
            if (Random.Range(0, weightSum) < weights[i]) {
                return i;
            }
            weightSum -= weights[i];
        }
        return weights.Length - 1;
    }
}
