using UnityEngine;

public class AnimationEventRaiser : MonoBehaviour {

    public void OnAnimationEnd() {
        AnimationManager.OnAnimationEnd();
    }
}
