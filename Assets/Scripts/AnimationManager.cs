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

    void Start() {
        GameManager.OnFinalInputEvent += OnFinalInput;
        GameManager.OnMissingInputAndTimeElapsed += OnMissingInputAndTimeElapsed;
        GameManager.OnMoveSuccess += OnMoveSuccess;
    }

    void OnFinalInput(bool isSuccess) {
        Arrow.Animation animation = isSuccess ?
            Arrow.Animation.Success : Arrow.Animation.Fail;
        GameManager.SelectedArrow.TriggerAnimation(animation);
        IsAnimating = true;
    }

    void OnMissingInputAndTimeElapsed() {
        GameManager.SelectedArrow.TriggerAnimation(Arrow.Animation.Fail);
        IsAnimating = true;
    }

    void OnMoveSuccess(int moveIndex) {
        Arrow arrow = GameManager.SelectedArrow;
        string animationName = arrow.GetAnimationName(moveIndex);
        arrow.TriggerAnimation(animationName);
        IsAnimating = true;
    }

    public static void OnAnimationEnd() {
        IsAnimating = false;
    }

    void OnDestroy() {
        GameManager.OnFinalInputEvent -= OnFinalInput;
        GameManager.OnMissingInputAndTimeElapsed -= OnMissingInputAndTimeElapsed;
        GameManager.OnMoveSuccess -= OnMoveSuccess;
    }
}
