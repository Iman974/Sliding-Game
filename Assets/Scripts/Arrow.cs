using UnityEngine;

public class Arrow : MonoBehaviour, RandomUtility.IWeighted {

    [SerializeField] int weight = 1;
    [SerializeField] float duration = 1f;
    [SerializeField] int scoreValue = 10;
    [SerializeField] float scoreLossMultiplier = 1f;
    [SerializeField] [Range(0, 3)] int directionToShowModifier = 0;

    public static bool IsAnimating { get; private set; }

    public int Weight { get => weight; set => weight = value; }
    public float Duration => duration;
    public bool IsActive { set { gameObject.SetActive(value); } }
    public int ScoreValue => scoreValue;
    public float ScoreLossMultiplier => scoreLossMultiplier;

    Direction orientation;
    Animator animator;

    void OnEnable() {
        IsAnimating = true;
        GameManager.OnArrowEnd += PlayEndAnimation;
    }

    void Start() {
        animator = GetComponent<Animator>();
    }

    public void SetOrientation(Direction direction) {
        Direction modifiedDirection = (Direction)(((int)direction + directionToShowModifier) %
            DirectionUtility.kDirectionCount);

        transform.eulerAngles = Vector3.forward * DirectionUtility.DirectionToAngle(modifiedDirection);
    }

    void PlayEndAnimation(bool hasScored) {
        animator.SetTrigger(hasScored ? "Success" : "Fail");
        IsAnimating = true;
    }

    void OnAnimationEnd() {
        IsAnimating = false;
    }

    public void ResetTransform() {
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    void OnDisable() {
        GameManager.OnArrowEnd -= PlayEndAnimation;
    }
}
