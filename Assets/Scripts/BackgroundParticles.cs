using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class BackgroundParticles : MonoBehaviour {

    [SerializeField] float speedGainOverProgression = 0.01f;
    [SerializeField] float maxPlaybackSpeed = 6f;
    [SerializeField] float onGameOverGravityModifier = 0.8f;
    [SerializeField] float playbackSpeedFastLevel = 6f;
    [SerializeField] float playbackSpeedRestoreDuration = 0.4f;
    [SerializeField] float playbackSpeedFastDuration = 2f;

    ParticleSystem.MainModule mainModule;

    void Start() {
        GameManager.OnArrowEnd += OnArrowEnd;
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameRestart += OnGameRestart;

        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        mainModule = particleSystem.main;
    }

    void OnArrowEnd(bool isSuccess) {
        if (isSuccess) {
            float newPlaybackSpeed = Mathf.Min(maxPlaybackSpeed,
        mainModule.simulationSpeed + speedGainOverProgression);
            mainModule.simulationSpeed = newPlaybackSpeed;
        }
    }

    void OnGameOver() {
        mainModule.gravityModifierMultiplier = onGameOverGravityModifier;
        mainModule.simulationSpeed = 1f;
    }

    void OnGameRestart() {
        mainModule.gravityModifierMultiplier = 0f;
        mainModule.simulationSpeed = playbackSpeedFastLevel;
        StartCoroutine(RestoreInitialParticlesPlaybackSpeed());
    }

    IEnumerator RestoreInitialParticlesPlaybackSpeed() {
        yield return new WaitForSeconds(playbackSpeedFastDuration);

        for (float t = 0f; t < playbackSpeedRestoreDuration; t += Time.deltaTime) {
            mainModule.simulationSpeed = Mathf.Lerp(playbackSpeedFastLevel, 1f,
                t / playbackSpeedRestoreDuration);
            yield return null;
        }
        mainModule.simulationSpeed = 1f;
    }

    void OnDestroy() {
        GameManager.OnArrowEnd -= OnArrowEnd;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameRestart -= OnGameRestart;
    }
}
