using UnityEngine;

public static class InputUtility {

    public static bool HasTouchMoved(float sensibility) {
        if (Input.touchCount == 0) {
            return false;
        }

        Touch touch = Input.GetTouch(0);
        return touch.phase == TouchPhase.Moved && touch.deltaPosition.sqrMagnitude > sensibility;
    }
}
