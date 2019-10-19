using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class AnimationDelayedEvent : MonoBehaviour {

    [SerializeField] UnityEvent[] unityEvents = null;
    [SerializeField] List<float> delays = new List<float>();
    
    int eventIndex;

    void RaiseEvent(int index) {
        float currentDelay = delays[index];
        eventIndex = index;
        const float kFrameUpdateTime = 0.02f;
        if (currentDelay > kFrameUpdateTime) {
            Invoke("CallEvent", currentDelay);
            return;
        }
        CallEvent();
    }

    void CallEvent() {
        unityEvents[eventIndex].Invoke();
    }

    void OnValidate() {
        // Makes sure that unityEvents array and delays list have the same Length
        for (int i = 0; i < delays.Count; i++) {
            delays[i] = Mathf.Max(delays[i], 0f);
        }

        int eventsCount = unityEvents.Length;
        int countDiff = delays.Count - eventsCount;
        if (countDiff > 0) {
            delays.RemoveRange(eventsCount, countDiff);
        } else if (countDiff < 0) {
            delays.AddRange(new float[-countDiff]);
        }
    }
}
