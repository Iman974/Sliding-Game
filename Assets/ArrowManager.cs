using UnityEngine;

public class ArrowManager : MonoBehaviour {

    public static Arrow SelectedArrow { get; private set; }

    static ArrowManager instance;
    Arrow[] arrows;

    void Awake() {
        #region Singleton
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
            return;
        }
        #endregion
    }

    void Start() {
        arrows = GetComponentsInChildren<Arrow>();
    }

    void NextArrow() {
        // Hide the previous arrow and reset its transform
        if (SelectedArrow != null) {
            SelectedArrow.IsActive = false;
            SelectedArrow.ResetTransform();
        }

        // Randomly select an arrow with weighted probability
        SelectedArrow = arrows[RandomUtility.SelectRandomWeightedIndex(arrows)];

        //CurrentDesiredDirection = DirectionUtility.GetRandomDirection();
        //SelectedArrow.SetOrientation(CurrentDesiredDirection);
        //SelectedArrow.IsActive = true;
        //countdown.Restart(SelectedArrow.Duration);
        //doInputCheck = true;
        //OnNextArrow?.Invoke();
    }
}
