using UnityEngine;

[System.Serializable]
public class Arrow {

    [SerializeField] GameObject instance = null;
    [SerializeField] [Min(0f)] int weight = 1;
    [SerializeField] float duration = 1f;

    public GameObject Instance => instance;
    public int Weight => weight;
    public float Duration => duration;
    public int Id { get; private set; }

    bool isInitialized;

    public void Init() {
        if (isInitialized) {
            return;
        }
        Id = (int)System.Enum.Parse(typeof(ArrowId), instance.name);
        instance = Object.Instantiate(instance);
        isInitialized = true;
    }

    public void SetVisibility(bool isVisible) {
        instance.SetActive(isVisible);
    }

}
