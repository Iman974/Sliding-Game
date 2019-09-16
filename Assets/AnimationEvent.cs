using UnityEngine;

public class AnimationEvent : MonoBehaviour {

    public void OnAnimationEnd() {
        AnimationManager.OnFinalAnimationEnd();
        //transform.position = Vector3.zero;
    }
}
