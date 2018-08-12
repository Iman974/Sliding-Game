using UnityEngine;

public class Mover2D : CustomStateMachineBehaviour {

    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private bool multiplierParamX;
    [SerializeField] private bool multiplierParamY;

    private Transform transform;
    private Vector2 startPosition;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        transform = animator.transform;

        if (multiplierParamX) {
            targetPosition.x = targetPosition.x * animator.GetFloat("moveX");
        }
        if (multiplierParamY) {
            targetPosition.y = targetPosition.y * animator.GetFloat("moveY");
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        elapsedTime += Time.deltaTime;
        float lerpPercentage = elapsedTime * speed;
        transform.position = Vector2.Lerp(startPosition, targetPosition, curve.Evaluate(lerpPercentage));
    }
}
