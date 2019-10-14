using UnityEngine;

public class Countdown {

    public float Time { get; private set; }
    public bool Elapsed { get; private set; } = true;

    bool doUpdate;

    public void Update(float tick) {
        if (!doUpdate) {
            return;
        }

        Time -= tick;
        if (Time <= 0f) {
            Elapsed = true;
        }
    }

    public void Resume() {
        doUpdate = true;
    }

    public void Stop() {
        doUpdate = false;
    }

    public void ResetTime(float newTime) {
        if (newTime > 0f) {
            Time = newTime;
            Elapsed = false;
            Resume();
        }
    }
}
