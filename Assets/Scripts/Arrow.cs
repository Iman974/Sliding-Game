using UnityEngine;

[System.Serializable]
public class Arrow {

    [SerializeField] GameObject instance = null;
    [SerializeField] [Range(0f, 1f)] float probability = 1f;
    [SerializeField] float duration = 1f;

    public GameObject Instance => instance;
    public float Probability => probability;
    public float Duration => duration;

    public void InstantiateArrow() {
        instance = Object.Instantiate(instance);
    }
}
