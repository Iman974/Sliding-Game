using UnityEngine;

public class GameManager : MonoBehaviour {

    [Tooltip("The threshold that need to be reached in order to consider the sliding.")]
    [SerializeField] float slidingSensibility = 0.25f;
    [SerializeField] int maxLives = 3;
    [SerializeField] float accelerationFactor = 1.03f;
    [SerializeField] float maxAcceleration = 1.75f;

    public static GameManager Instance { get; private set; }
    public static Direction CurrentDirection { get; private set; }
    public static int PlayerScore { get; private set; }

    public int Lives { get; set; }

    Direction inputDirection;
    Direction desiredDirection;

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
        if (Input.touchCount != 0) {
            Touch touch = Input.GetTouch(0);
            Vector2 deltaPos = touch.deltaPosition;

            float sqrSensibility = slidingSensibility * slidingSensibility;
            if (deltaPos.sqrMagnitude >= sqrSensibility) {
                inputDirection = DirectionUtility.VectorToDirection(deltaPos);

                if (inputDirection == desiredDirection) {

                }
            }
        }
    }
}
