using UnityEngine;

public class Rotater : CustomStateMachineBehaviour {

    [SerializeField] private Vector3 relativeTargetRotation;

    private Transform transform;
    private Vector3 startRotation;
    private Vector3 targetRotation;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        transform = animator.transform;

        startRotation = transform.eulerAngles;
        targetRotation = startRotation + relativeTargetRotation;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        elapsedTime += Time.deltaTime;
        float lerpPercentage = elapsedTime * speed;
        transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, curve.Evaluate(lerpPercentage));
    }
}
