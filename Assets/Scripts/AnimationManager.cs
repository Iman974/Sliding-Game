using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {

    [SerializeField] ParticleSystem backgroudParticles = null;
    [SerializeField] float particleSpeedGainOverProgression = 0.004f;
    [SerializeField] float onGameOverGravityModifier = 0.8f;
    [SerializeField] float playbackSpeedFastLevel = 6f;
    [SerializeField] float particlePlaybackSpeedRestoreDuration = 0.7f;
    [SerializeField] float particlePlaybackSpeedFastDuration = 3f;

    public static AnimationManager Instance { get; private set; }
    public static bool IsAnimating { get; private set; }

    float playbackSpeed = 1f;

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

        particlesMainModule = backgroudParticles.main;
    }

    void BeforeNextArrow(bool isSuccess) {
        Animation animation;
        if (isSuccess) {
            animation = Animation.Success;
            playbackSpeed += particleSpeedGainOverProgression;
            particlesMainModule.simulationSpeed = playbackSpeed;
        } else {
            animation = Animation.Fail;
        }
        GameManager.SelectedArrow.PlayAnimation(animation.ToString());

        IsAnimating = true;
    }

    void OnMoveSuccess() {
        Arrow arrow = GameManager.SelectedArrow;
        string animationTriggerName = arrow.MoveAnimationTriggerName;
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
        playbackSpeed = 1f;
        particlesMainModule.simulationSpeed = playbackSpeedFastLevel;
        StartCoroutine(RestoreInitialParticlesPlaybackSpeed());
    }

    IEnumerator RestoreInitialParticlesPlaybackSpeed() {
        yield return new WaitForSeconds(particlePlaybackSpeedFastDuration);

        for (float t = 0f; t < particlePlaybackSpeedRestoreDuration; t += Time.deltaTime) {
            particlesMainModule.simulationSpeed = Mathf.Lerp(playbackSpeedFastLevel, 1f,
                t / particlePlaybackSpeedRestoreDuration);
            yield return null;
        }
        particlesMainModule.simulationSpeed = 1f;
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
