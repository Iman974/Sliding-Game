using UnityEngine;

public class AnimationManager : MonoBehaviour {

    [SerializeField] float nextDelay = 0.1f;

    public static AnimationManager Instance { get; private set; }
    public static bool IsAnimating { get; private set; }
    public static float NextDelay { get; private set; }

    void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
            return;
        }
        #endregion

        NextDelay = nextDelay;
    }

    private void Start() {
        GameManager.OnCorrectFinalInputEvent += OnFinalInputRight;
    }

    public static void OnFinalInputRight() {
        GameManager.SelectedArrow.TriggerAnimation(Arrow.Animation.Move);
        IsAnimating = true;
    }

    public static void OnFinalAnimationEnd() {
        IsAnimating = false;
    }

    void OnDestroy() {
        GameManager.OnCorrectFinalInputEvent -= OnFinalInputRight;
    }
}
