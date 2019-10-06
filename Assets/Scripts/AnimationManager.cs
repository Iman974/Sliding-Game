using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {

    [SerializeField] ParticleSystem backgroudParticles = null;
    [SerializeField] float particleSpeedGainOverProgression = 0.004f;
    [SerializeField] float maxParticlePlaybackSpeed = 6f;
    [SerializeField] float onGameOverGravityModifier = 0.8f;
    [SerializeField] float playbackSpeedFastLevel = 6f;
    [SerializeField] float particlePlaybackSpeedRestoreDuration = 0.7f;
    [SerializeField] float particlePlaybackSpeedFastDuration = 3f;

    public static AnimationManager Instance { get; private set; }
    public static bool IsAnimating { get; private set; }

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
        GameManager.OnArrowEnd += OnArrowEnd;
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameRestart += OnGameRestart;

        particlesMainModule = backgroudParticles.main;
    }

    void OnArrowEnd(bool isSuccess) {
        AnimationTrigger animationTrigger;
        if (isSuccess) {
            float newPlaybackSpeed = Mathf.Min(maxParticlePlaybackSpeed,
                particlesMainModule.simulationSpeed + particleSpeedGainOverProgression);
            particlesMainModule.simulationSpeed = newPlaybackSpeed;
            animationTrigger = AnimationTrigger.Success;
        } else {
            animationTrigger = AnimationTrigger.Fail;
        }
        GameManager.SelectedArrow.PlayAnimation(animationTrigger.ToString());
        IsAnimating = true;
    }

    public static void OnAnimationEnd() {
        IsAnimating = false;
    }

    void OnGameOver() {
        particlesMainModule.gravityModifierMultiplier = onGameOverGravityModifier;
        particlesMainModule.simulationSpeed = 1f;
    }

    void OnGameRestart() {
        particlesMainModule.gravityModifierMultiplier = 0f;
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
        GameManager.OnArrowEnd -= OnArrowEnd;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameRestart -= OnGameRestart;
    }

    enum AnimationTrigger {
        Success,
        Fail
    }
}
