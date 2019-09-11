using UnityEngine;

public class AnimationEvent : MonoBehaviour {

    public void OnAnimationEnd() {
        GameManager.Instance.OnAnimationEnd();
        transform.position = Vector3.zero;
    }
}
