using UnityEngine;

[CreateAssetMenu(menuName = "Arrow", fileName = "New arrow")]
public class Arrow : ScriptableObject {

    [SerializeField] GameObject instance = null;
    [SerializeField] int weight = 1;
    [SerializeField] float duration = 1f;
    [SerializeField] [Range(0, 3)] int displayedDirectionModifier = 0;
    // The moves required to correctly orient the arrow (relative to the displayed direction)
    [SerializeField] [Range(0, 3)] int[] moves = null;

    public int Weight => weight;
    public float Duration => duration;
    public int DisplayedDirectionModifier => displayedDirectionModifier;
    public int MoveCount => moves.Length;
    public bool IsActive { set { instance.SetActive(value); } }
    public Direction Orientation {
        set {
            instance.transform.right = DirectionUtility.DirectionToVector(value);
        }
    }

    Animator animator;
    SpriteRenderer spr;

    //[RuntimeInitializeOnLoadMethod]
    void Awake() {
        Transform arrowPoolTransform = GameObject.FindWithTag("ArrowPool")?.transform;
        if (arrowPoolTransform == null) {
            Debug.LogWarning("Could not find the arrow pool.");
            return;
        }
        instance = arrowPoolTransform.Find(name).gameObject;
        if (instance == null) {
            Debug.LogError("Could not find the arrow with name: " + name);
            return;
        }

        animator = instance.GetComponent<Animator>();
        spr = instance.GetComponent<SpriteRenderer>();
    }

#if UNITY_EDITOR
    void OnEnable() {
        if (Application.isEditor) {
            Awake();
        }
    }
#endif

    public int GetMove(int index) {
        return moves[index];
    }

    public void TriggerAnimation(Animation animation) {
        animator.SetTrigger(animation.ToString());
    }

    public void Reset() {
        instance.transform.position = Vector3.zero;
        spr.color = spr.color + Color.black;
    }

    public enum Animation {
        Rotate,
        Move
    }
}
