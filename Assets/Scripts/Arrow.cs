using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Arrow : MonoBehaviour, RandomUtility.IWeighted {

    [SerializeField] int weight = 1;
    [SerializeField] float duration = 1f;
    [SerializeField] int scoreValue = 10;
    [SerializeField] AnimationCurve speedRewardCurve = AnimationCurve.Constant(1f, 0f, 0f);
    [SerializeField] float scoreLossMultiplier = 1f;
    [SerializeField] [Range(0, 3)] int directionToShowModifier = 0;

    public static bool IsAnimating { get; private set; }

    public int Weight { get => weight; set => weight = value; }
    public float Duration => duration;
    public bool IsActive { set { gameObject.SetActive(value); } }
    public int ScoreValue => scoreValue;
    public AnimationCurve SpeedRewardCurve => speedRewardCurve;
    public float ScoreLossMultiplier => scoreLossMultiplier;

    Direction orientation;
    Animator animator;
    static InputManager inputManager;

    void OnEnable() {
        IsAnimating = true;
        InputManager.OnInputReceived += PlayEndAnimation;
    }

    void Start() {
        animator = GetComponent<Animator>();
        if (inputManager == null) {
            inputManager = InputManager.Instance;
        }
    }

    public void SetOrientation(Direction direction) {
        Direction directionToShow = (Direction)(((int)direction + directionToShowModifier) %
            DirectionUtility.kDirectionCount);

        transform.eulerAngles = Vector3.forward * DirectionUtility.DirectionToAngle(directionToShow);
    }

    public void PlayEndAnimation(bool hasScored) {
        animator.SetTrigger(hasScored ? "Success" : "Fail");
        IsAnimating = true;
    }

    void OnAnimationEnd() {
        IsAnimating = false;
    }

    void OnFadeInAnimationEnd() {
        inputManager.enabled = true;
        IsAnimating = false;
    }

    public void ResetTransform() {
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    void OnDisable() {
        InputManager.OnInputReceived -= PlayEndAnimation;
    }
}
