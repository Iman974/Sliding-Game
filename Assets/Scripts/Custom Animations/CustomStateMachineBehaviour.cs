using UnityEngine;

public class CustomStateMachineBehaviour : StateMachineBehaviour {

    protected float speed;
    protected float elapsedTime;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        speed = stateInfo.speed;
        elapsedTime = Time.time;
    }
}
