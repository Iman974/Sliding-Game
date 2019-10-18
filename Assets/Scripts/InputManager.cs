using UnityEngine;

public class InputManager : MonoBehaviour {

    [SerializeField] float swipingSensibility = 10f;

    public static event System.Action<bool> OnInputReceived;

    static InputManager instance;
    static float sqrSwipingSensibility;
    static Direction inputDirection;

    void Awake() {
        #region Singleton
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
            return;
        }
        #endregion

        sqrSwipingSensibility = swipingSensibility * swipingSensibility;
    }

    void Update() {
        if (GetInput()) {
            OnInputReceived?.Invoke(inputDirection == ArrowManager.CurrentDesiredDirection);
            enabled = false;
        }
    }

    // Writes the input into the input parameter if there is one (returns this info)
    public static bool GetInput() {
        if (Input.touchCount == 0) {
            return false;
        }
        Touch touch = Input.GetTouch(0);
        Vector2 deltaPos = touch.deltaPosition;
        if (deltaPos.sqrMagnitude < sqrSwipingSensibility) {
            return false;
        }
        inputDirection = DirectionUtility.VectorToDirection(deltaPos);
        return true;
    }
}
