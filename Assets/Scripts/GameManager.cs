using UnityEngine;

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

    void NextArrow() {
        desiredDirection = DirectionUtility.GetRandomDirection();
        float randomValue = Random.value;
        Arrow[] arrows = ArrowSpawner.Instance.Arrows;

        int i = 0;
        foreach (Arrow arrow in arrows) {
            if (randomValue < arrow.Probability) {
                break;
            }
            i++;
        }

        //displayedDirection = Random.value < ;
        //countdown = ;
    }
}
