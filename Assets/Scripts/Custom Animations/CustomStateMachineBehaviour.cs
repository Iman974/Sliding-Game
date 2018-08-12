using UnityEngine;

public abstract class CustomStateMachineBehaviour : StateMachineBehaviour {

    [SerializeField] protected AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    protected float speed;
    protected float elapsedTime;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        speed = stateInfo.speed;
    }
}
