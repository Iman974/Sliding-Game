using UnityEngine;
using System.Collections.Generic;

public class ArrowManager : MonoBehaviour {

    [SerializeField] float nextArrowDelay = 0.3f;

    public static Arrow SelectedArrow { get; private set; }
    public static Arrow[] Arrows { get; private set; }
    public static ArrowManager Instance { get; private set; }
    public static Direction DesiredDirection { get; private set; }

    public static event System.Action OnNextArrow;

    const int kMaxDirectionRepetition = 1;

    int sameDirectionCount;

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

        Arrow previousArrow = SelectedArrow;
        // Randomly select an arrow with weighted probability
        SelectedArrow = Arrows[RandomUtility.SelectRandomWeightedIndex(Arrows)];

        if (sameDirectionCount < kMaxDirectionRepetition) {
            Direction previousDir = DesiredDirection;
            DesiredDirection = DirectionUtility.GetRandomDirection();
            if (previousArrow != null && previousArrow == SelectedArrow && previousDir == DesiredDirection) {
                sameDirectionCount++;
            } else {
                sameDirectionCount = 0;
            }
        } else if (previousArrow == SelectedArrow) {
            List<Direction> directions = new List<Direction>(DirectionUtility.kDirections);
            directions.Remove(DesiredDirection);
            DesiredDirection = directions[Random.Range(0, directions.Count)];
            sameDirectionCount = 0;
        } else {
            DesiredDirection = DirectionUtility.GetRandomDirection();
            sameDirectionCount = 0;
        }
        SelectedArrow.SetOrientation(DesiredDirection);
        SelectedArrow.IsActive = true;
        OnNextArrow?.Invoke();
    }
}
