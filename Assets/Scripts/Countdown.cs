using UnityEngine;

public class Countdown : MonoBehaviour {

    public float RemainingTime { get; private set; }
    public bool IsElapsed { get; private set; } = true;
    public float PlaybackSpeed { get; set; } = 1f;
    public bool DoUpdate { get; set; }

    void Update() {
        if (!DoUpdate) {
            return;
        }

        RemainingTime -= Time.deltaTime * PlaybackSpeed;
        if (RemainingTime <= 0f) {
            IsElapsed = true;
            Stop();
        }
    }

    public void Resume() {
        DoUpdate = true;
    }

    public void Stop() {
        DoUpdate = false;
    }

    public void Restart(float newTime) {
        if (newTime > 0f) {
            RemainingTime = newTime;
            IsElapsed = false;
            Resume();
        }
    }
}
