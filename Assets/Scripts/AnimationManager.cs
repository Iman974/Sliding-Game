using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {

    [SerializeField] ParticleSystem backgroudParticles = null;
    [SerializeField] float particleSpeedGainOverProgression = 0.004f;
    [SerializeField] float onGameOverGravityModifier = 0.8f;

    public static AnimationManager Instance { get; private set; }
    public static bool IsAnimating { get; private set; }

    float playbackSpeed = 1f;

    ParticleSystem.VelocityOverLifetimeModule particleVelocityModule;
    ParticleSystem.MainModule particlesMainModule;

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
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameRestart += OnGameRestart;

        particleVelocityModule = backgroudParticles.velocityOverLifetime;
        particlesMainModule = backgroudParticles.main;
    }

    void BeforeNextArrow(BeforeNextArrowEventArgs arg) {
        Animation animation;
        if (arg.IsSuccess) {
            animation = Animation.Success;
            playbackSpeed += particleSpeedGainOverProgression;
            particleVelocityModule.speedModifierMultiplier = playbackSpeed;
        } else {
            animation = Animation.Fail;
        }
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

    void OnGameOver() {
        particlesMainModule.gravityModifierMultiplier = onGameOverGravityModifier;
    }

    void OnGameRestart() {
        particlesMainModule.gravityModifierMultiplier = 0f;
    }

    void OnDestroy() {
        GameManager.BeforeNextArrow -= BeforeNextArrow;
        GameManager.OnMoveSuccess -= OnMoveSuccess;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameRestart -= OnGameRestart;
    }

    enum Animation {
        Success,
        Fail
    }
}
