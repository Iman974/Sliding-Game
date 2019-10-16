using UnityEngine;

public class SineMover : MonoBehaviour {

    [SerializeField] float moveSpeed = 11f;
    [SerializeField] float amplitude = -10f;

    RectTransform rectTransform;
    Vector2 direction;
    Vector2 startAnchoredPosition;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        direction = transform.up;
        startAnchoredPosition = rectTransform.anchoredPosition;
    }

    void LateUpdate() {
        float normalizedSine = (Mathf.Sin(Time.time * moveSpeed) + 1f) * 0.5f;
        Vector2 newPosition = startAnchoredPosition + (direction * normalizedSine * amplitude);
        rectTransform.anchoredPosition = newPosition;
    }
}
