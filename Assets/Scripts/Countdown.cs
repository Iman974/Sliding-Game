using UnityEngine;
using System;

public class Countdown : MonoBehaviour {

    public float WaitTime { get; set; }
    public float ElapsedTime {
        get { return countdown; }
    }

    public event Action Elapsed;

    private bool runCountdown;
    private float countdown;

    public void Begin() {
        runCountdown = true;
    }

    private void Update() {
        if (!runCountdown) {
            return;
        }

        countdown -= Time.deltaTime;

        if (countdown <= 0f) {
            Stop();
            ResetCountdown();

            if (Elapsed != null) {
                Elapsed();
            }
        }
    }

    public void Stop() {
        runCountdown = false;
    }

    private void ResetCountdown() {
        countdown = WaitTime;
    }

    public void Restart() {
        ResetCountdown();
        Begin();
    }
}
