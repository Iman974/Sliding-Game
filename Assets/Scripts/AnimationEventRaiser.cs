using UnityEngine;
using UnityEngine.Events;

public class AnimationEventRaiser : MonoBehaviour {

    [SerializeField] UnityEvent[] unityEvents = null;
    [SerializeField] float[] delays = null;
    
    UnityEvent eventToCall;

    void Start() {
        for (int i = 0; i < delays.Length; i++) {
            delays[i] = Mathf.Max(delays[i], 0f);
        }
    }

    void RaiseEvent() {
        for (int i = 0; i < unityEvents.Length; i++) {
            float delay = delays[i];
            eventToCall = unityEvents[i];
            if (delay > 0.01f) {
                Invoke("CallEvent", delays[i]);
                continue;
            }
            eventToCall.Invoke();
        }
    }

    void CallEvent() {
        if (eventToCall == null) {
            Debug.LogError("Event is null!");
            return;
        }
        eventToCall.Invoke();
    }
}
