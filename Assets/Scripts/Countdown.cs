using UnityEngine;

public class Countdown {

    public float RemainingTime { get; private set; }
    public bool IsElapsed { get; private set; }
    public float PlaybackSpeed { get; set; } = 1f;

    public void Update() {
        RemainingTime -= Time.deltaTime * PlaybackSpeed;
        if (RemainingTime <= 0f) {
            IsElapsed = true;
        }
    }

    public void Restart(float newTime) {
        if (newTime > 0f) {
            RemainingTime = newTime;
            IsElapsed = false;
        }
    }
}
