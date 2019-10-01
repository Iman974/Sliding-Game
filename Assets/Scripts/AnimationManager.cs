using UnityEngine;

public class AnimationManager : MonoBehaviour {

    [SerializeField] ParticleSystem backgroudParticles = null;

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
        GameManager.BeforeNextArrow += BeforeNextArrow;
        GameManager.OnMoveSuccess += OnMoveSuccess;
    }

    void BeforeNextArrow(BeforeNextArrowEventArgs arg) {
        Animation animation = arg.IsSuccess ?
            Animation.Success : Animation.Fail;
        GameManager.SelectedArrow.PlayAnimation(animation.ToString());

        IsAnimating = true;
    }

    void OnMoveSuccess(OnMoveSuccessEventArgs args) {
        Arrow arrow = GameManager.SelectedArrow;
        string animationTriggerName = arrow.GetAnimationTriggerName(args.MoveIndex);
        // Same as OnFinalInput, but string & enum
        arrow.PlayAnimation(animationTriggerName);
        IsAnimating = true;
    }

    public static void OnAnimationEnd() {
        IsAnimating = false;
    }

    void OnDestroy() {
        GameManager.BeforeNextArrow -= BeforeNextArrow;
        GameManager.OnMoveSuccess -= OnMoveSuccess;
    }

    enum Animation {
        Success,
        Fail
    }
}
