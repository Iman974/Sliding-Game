using UnityEngine;

public class AnimationManager : MonoBehaviour {

    public static AnimationManager Instance { get; private set; }
    public static bool IsAnimating { get; private set; }

    void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
            return;
        }
        #endregion
    }

    void Start() {
        GameManager.OnFinalInputEvent += OnFinalInput;
        GameManager.OnMissingInputAndTimeElapsed += OnMissingInputAndTimeElapsed;
        GameManager.OnMoveSuccess += OnMoveSuccess;
        GameManager.OnMoveFail += OnMoveFail;
    }

    void OnFinalInput(bool isSuccess) {
        Animation animation = isSuccess ?
            Animation.Success : Animation.Fail;
        GameManager.SelectedArrow.PlayAnimation(animation.ToString());
        IsAnimating = true;
    }

    void OnMissingInputAndTimeElapsed() {
        //OnFinalInput(isSuccess: false)
        GameManager.SelectedArrow.PlayAnimation(Animation.Fail.ToString());
        IsAnimating = true;
    }

    void OnMoveSuccess(int moveIndex) {
        Arrow arrow = GameManager.SelectedArrow;
        string animationTriggerName = arrow.GetAnimationTriggerName(moveIndex);
        // Same as OnFinalInput, but string & enum
        arrow.PlayAnimation(animationTriggerName);
        IsAnimating = true;
    }

    void OnMoveFail() {
        OnFinalInput(isSuccess: false);
    }

    public static void OnAnimationEnd() {
        IsAnimating = false;
    }

    void OnDestroy() {
        GameManager.OnFinalInputEvent -= OnFinalInput;
        GameManager.OnMissingInputAndTimeElapsed -= OnMissingInputAndTimeElapsed;
        GameManager.OnMoveSuccess -= OnMoveSuccess;
        GameManager.OnMoveFail -= OnMoveFail;
    }

    enum Animation {
        Success,
        Fail
    }
}
