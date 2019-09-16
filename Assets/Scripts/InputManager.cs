using UnityEngine;

public class InputManager : MonoBehaviour {

    [SerializeField] float swipingSensibility = 10f;

    static InputManager instance;
    //static Vector2 previousMousePos;
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
    }

    void Start() {
        sqrSwipingSensibility = swipingSensibility * swipingSensibility;
    }

    // Writes the input into the input parameter if there is one (returns this info)
    public static bool GetInput(ref Direction input) {
#if UNITY_STANDALONE_WIN
        //Touch touch = Input.GetTouch(0);
        Vector2 deltaPos = previousMousePos - (Vector2)Input.mousePosition;
        previousMousePos = Input.mousePosition;
        if (deltaPos.sqrMagnitude < sqrSwipingSensibility) {
            //return null;
        }
        //return DirectionUtility.VectorToDirection(deltaPos);
#elif UNITY_ANDROID
        Touch touch = Input.GetTouch(0);
        Vector2 deltaPos = touch.deltaPosition;
        if (deltaPos.sqrMagnitude < sqrSwipingSensibility) {
            return false;
        }
        input = DirectionUtility.VectorToDirection(deltaPos);
        return true;
#endif
    }
}
