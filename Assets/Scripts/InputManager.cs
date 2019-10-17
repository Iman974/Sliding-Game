using UnityEngine;

public class InputManager : MonoBehaviour {

    [SerializeField] float swipingSensibility = 10f;

    static InputManager instance;
    static float sqrSwipingSensibility;

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

    // Writes the input into the input parameter if there is one (returns this info)
    public static bool GetInput(ref Direction input) {
        if (Input.touchCount == 0) {
            return false;
        }
        Touch touch = Input.GetTouch(0);
        Vector2 deltaPos = touch.deltaPosition;
        if (deltaPos.sqrMagnitude < sqrSwipingSensibility) {
            return false;
        }
        input = DirectionUtility.VectorToDirection(deltaPos);
        return true;
    }
}
