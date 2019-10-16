using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class AnimationEventRaiser : MonoBehaviour {

    [SerializeField] UnityEvent[] unityEvents = null;
    [SerializeField] List<float> delays = null;
    
    int eventIndex;

    void RaiseEvent() {
        eventIndex = 0;
        for (int i = 0; i < unityEvents.Length; i++) {
            float currentDelay = delays[i];
            if (currentDelay > 0.01f) {
                Invoke("CallEvent", currentDelay);
                continue;
            }
            CallEvent();
        }
    }

    void CallEvent() {
        UnityEvent eventToCall = unityEvents[eventIndex];
        if (eventToCall == null) {
            Debug.LogError("Event is null!");
            return;
        }
        eventToCall.Invoke();
        eventIndex++;
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
