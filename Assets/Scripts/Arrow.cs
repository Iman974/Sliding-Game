using UnityEngine;

[System.Serializable]
public class Arrow {

    [SerializeField] GameObject instance = null;
    [SerializeField] [Min(0f)] int weight = 1;
    [SerializeField] float duration = 1f;
    [SerializeField] [Range(0, 3)] int directionModifier = 0;

    public GameObject Instance => instance;
    public int Weight => weight;
    public float Duration => duration;
    public int Id { get; private set; }
    public int DirectionModifier => directionModifier;

    bool isInitialized;

    public void Init() {
        if (isInitialized) {
            return;
        }
        Id = (int)System.Enum.Parse(typeof(ArrowId), instance.name);
        instance = Object.Instantiate(instance);
        instance.SetActive(false);
        isInitialized = true;
    }
}
