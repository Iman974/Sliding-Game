using UnityEngine;

[System.Serializable]
public class Arrow {

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

    public void Init() {
        animator = instance.GetComponent<Animator>();
    }

    public int GetMove(int index) {
        return moves[index];
    }

    public void TriggerAnimation(Animation animation) {
        animator.SetTrigger(animation.ToString());
    }

    public enum Animation {
        Rotate,
        Move
    }
}
