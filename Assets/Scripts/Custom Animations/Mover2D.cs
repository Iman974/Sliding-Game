using UnityEngine;

public class Mover2D : CustomStateMachineBehaviour {

    [SerializeField] private Vector2 targetPosition;

    private Transform transform;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        transform = animator.transform;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        elapsedTime += Time.deltaTime;
        float lerpPercentage = elapsedTime * speed;
        //transform
    }
}
