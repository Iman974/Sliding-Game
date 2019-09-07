using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    // The threshold indicating when to consider the sliding.
    [SerializeField] float slidingSensibility = 0.25f;
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
        HandleInput();

        countdown -= Time.deltaTime;
        if (countdown <= 0f) {
            NextArrow();
        }
    }

    void HandleInput() {
        if (Input.touchCount == 0) {
            return;
        }

        Touch touch = Input.GetTouch(0);
        Vector2 deltaPos = touch.deltaPosition;

        float sqrSensibility = slidingSensibility * slidingSensibility;
        if (deltaPos.sqrMagnitude >= sqrSensibility) {
            inputDirection = DirectionUtility.VectorToDirection(deltaPos);

            if (inputDirection == desiredDirection) {
                // Movement is validated
            }
        }
    }

    // Hide the previous arrow, randomly select a new one and show it.
    void NextArrow() {
        if (selectedArrow != null) {
            selectedArrow.Instance.SetActive(false);
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
            selectedArrow.DirectionModifier) % DirectionUtility.kDirectionCount);

        GameObject selectedArrowObj = selectedArrow.Instance;
        Vector2 orientation = DirectionUtility.DirectionToVector(displayedDirection);
        selectedArrowObj.transform.right = orientation;
        selectedArrowObj.SetActive(true);
        countdown = selectedArrow.Duration;
    }

    Rect rect = new Rect(5f, 10f, 300f, 75f);
    void OnGUI() {
        GUI.Label(rect, "DisplayDir: " + displayedDirection.ToString() + ", DesiredDir: " +
            desiredDirection.ToString());
    }

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
