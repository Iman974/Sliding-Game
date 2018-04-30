using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] private Text scoreText;
    [SerializeField] private Image directionImg;
    [SerializeField] private AnimationCurve slidingAnimation = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float slideSpeed = 1f;

    public void OnMovementValidation(int scoreValue) {
        scoreText.text = "Score: " + scoreValue;
        //StartCoroutine(SlideArrow());
    }

    public void OnNext(SlideDirection direction) {
        float rotation = 0;

        if (direction == SlideDirection.Right) {
            rotation = 90f;
        } else if (direction == SlideDirection.Down) {
            rotation = 180f;
        } else if (direction == SlideDirection.Left) {
            rotation = 270f;
        }

        directionImg.transform.rotation = Quaternion.Euler(0f, 0f, 90f - rotation);
    }

    //private IEnumerator SlideArrow() {
    //    Vector3 slideDirection;

    //    for (float time = 0; time < 1f; time += slideSpeed * Time.deltaTime) {
    //        directionImg.transform.position += slideDirection;
    //    }
    //}

    private void Start() {
        GameManager.ValidationEvent += OnMovementValidation;
        GameManager.NextEvent += OnNext;
    }

    private void OnDestroy() {
        GameManager.ValidationEvent -= OnMovementValidation;
        GameManager.NextEvent -= OnNext;
    }
}
