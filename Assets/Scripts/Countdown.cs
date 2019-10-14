using UnityEngine;

public class Countdown {

    public float RemainingTime { get; private set; }
    public bool IsElapsed { get; private set; } = true;

    bool doUpdate;

    public void Update(float tick) {
        if (!doUpdate) {
            return;
        }

        RemainingTime -= tick;
        if (RemainingTime <= 0f) {
            IsElapsed = true;
        }
    }

    public void Resume() {
        doUpdate = true;
    }

    public void Stop() {
        doUpdate = false;
    }

    public void Restart(float newTime) {
        if (newTime > 0f) {
            RemainingTime = newTime;
            IsElapsed = false;
            Resume();
        }
    }
}
