using UnityEngine;

public class Rotater : StateMachineBehaviour {

    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private Vector3 relativeTargetRotation;

    private Transform transform;
    private Quaternion targetRotation;
    private Quaternion startRotation;
    private float elapsedTime;
    private float speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        transform = animator.transform;
        speed = stateInfo.speed;

        startRotation = transform.rotation;
        targetRotation = startRotation * Quaternion.Euler(relativeTargetRotation);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        elapsedTime += Time.deltaTime;
        float lerpPercentage = elapsedTime * speed;
        transform.rotation = Quaternion.Lerp(startRotation, targetRotation, rotationCurve.Evaluate(lerpPercentage));
    }
}
