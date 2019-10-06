using UnityEngine;

public class AnimationEventRaiser : MonoBehaviour {

    void OnAnimationEnd() {
        AnimationManager.OnAnimationEnd();
    }
}
