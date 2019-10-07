using UnityEngine;

public class Arrow : MonoBehaviour {

    [SerializeField] int weight = 1;
    [SerializeField] float duration = 1f;
    [SerializeField] int scoreValue = 10;
    [SerializeField] [Range(0, 3)] int directionToShowModifier = 0;

    public int Weight => weight;
    public float Duration => duration;
    public bool IsActive { set { gameObject.SetActive(value); } }
    public Direction CurrentOrientation {
        get {
            return orientation;
        }
        set {
            orientation = value;
            transform.eulerAngles = Vector3.forward *
                DirectionUtility.DirectionToRotation(value);
        }
    }
    public int ScoreValue => scoreValue;

    Direction orientation;
    Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    public Direction GetDirectionToShow(Direction initialDir) {
        return (Direction)(((int)initialDir + directionToShowModifier) %
            DirectionUtility.kDirectionCount);
    }

    public void PlayAnimation(string animationTriggerName) {
        animator.SetTrigger(animationTriggerName);
    }

    public void ResetTransform() {
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;
    }
}
