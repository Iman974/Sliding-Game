using UnityEngine;

[CreateAssetMenu(menuName = "Arrow", fileName = "New arrow")]
public class Arrow : ScriptableObject {

    [SerializeField] GameObject instance = null;
    [SerializeField] int weight = 1;
    [SerializeField] float duration = 1f;
    [SerializeField] int scoreValue = 10;
    [SerializeField] [Range(0, 3)] int directionToShowModifier = 0;

    public int Weight => weight;
    public float Duration => duration;
    public bool IsActive { set { instance.SetActive(value); } }
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
    Transform transform;

#if UNITY_EDITOR
    void OnEnable() {
        if (Application.isEditor) {
            Awake();
        }
    }
#endif

    void Awake() {
        Transform arrowPoolTransform = GameObject.FindWithTag("ArrowPool")?.transform;
        if (arrowPoolTransform == null) {
            Debug.LogWarning("Could not find the arrow pool.");
            return;
        }
        instance = arrowPoolTransform.Find(name).gameObject;
        if (instance == null) {
            Debug.LogError("Could not find the " + name + " arrow");
            return;
        }

        animator = instance.GetComponent<Animator>();
        transform = instance.transform;
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
