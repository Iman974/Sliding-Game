using UnityEngine;
using System;

public class Countdown : MonoBehaviour {

    public float WaitTime { get; set; }

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

    public void ResetCountdown() {
        countdown = WaitTime;
    }
}
