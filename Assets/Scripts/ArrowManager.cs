using UnityEngine;

public class ArrowManager : MonoBehaviour {

    [SerializeField] float nextArrowDelay = 0.3f;

    public static Arrow SelectedArrow { get; private set; }
    public static Arrow[] Arrows { get; private set; }

    public static ArrowManager Instance { get; private set; }

    public static Direction CurrentDesiredDirection { get; private set; }

    public static event System.Action OnNextArrow;

    void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
            return;
        }
        #endregion
        Arrows = GetComponentsInChildren<Arrow>(includeInactive: true);
    }

    public void InvokeNextArrowDelayed() {
        StartCoroutine(NextArrowDelayed());
    }

    System.Collections.IEnumerator NextArrowDelayed() {
        yield return new WaitForEndOfFrame();
        while (Arrow.IsAnimating) {
            yield return null;
        }
        Invoke("NextArrow", nextArrowDelay);
    }

    // Static (Invoke function as well) ?? -> Is it called from a UnityEvent in the inspector ?
    public void NextArrow() {
        // Hide the previous arrow and reset its transform
        if (SelectedArrow != null) {
            SelectedArrow.IsActive = false;
            SelectedArrow.ResetTransform();
        }

        // Randomly select an arrow with weighted probability
        SelectedArrow = Arrows[RandomUtility.SelectRandomWeightedIndex(Arrows)];

        CurrentDesiredDirection = DirectionUtility.GetRandomDirection();
        SelectedArrow.SetOrientation(CurrentDesiredDirection);
        SelectedArrow.IsActive = true;
        OnNextArrow?.Invoke();
    }
}
