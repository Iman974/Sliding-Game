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

    void Start() {
        NextArrow();
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

    void NextArrow() {
        desiredDirection = DirectionUtility.GetRandomDirection();
        Arrow[] arrows = ArrowPool.Instance.Arrows;

        int weightSum = arrows.Sum(a => a.Weight);
        for (int i = 0; i < arrows.Length; i++) {
            if (Random.Range(0, weightSum) < arrows[i].Weight) {
                //selectedArrow = arrows[];
                break;
            }
            weightSum -= arrows[i].Weight;
        }

        int arrowId = selectedArrow.Id;

        displayedDirection = (Direction)(((int)desiredDirection + arrowId) % 4);
        countdown = selectedArrow.Duration;
        selectedArrow.SetVisibility(true);
    }
}
