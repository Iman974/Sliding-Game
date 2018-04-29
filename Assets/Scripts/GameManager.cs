using UnityEngine;

public class GameManager : MonoBehaviour {

    private enum SlideDirection {
        Left,
        Right,
        Up,
        Down
    }

    [SerializeField] private float skipDelay = 1.5f;

    private float countdown;
    private SlideDirection currentDirection;

    private void Start() {
        countdown = skipDelay;
    }

    private void Update() {
        countdown -= Time.deltaTime;

        if (countdown <= 0f) {
            countdown = skipDelay;
            // -1 life

            Next();
        } else if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved) {
                SlideDirection dir;

                if (touch.deltaPosition.x > 0f) {
                    dir = SlideDirection.Right;
                } else if (touch.deltaPosition.x < 0f) {
                    dir = SlideDirection.Left;
                } else if (touch.deltaPosition.y > 0f) {
                    dir = SlideDirection.Up;
                } else {
                    dir = SlideDirection.Down;
                }

                ValidateMovement(dir);
                Next();
            }
        }
    }

    private void ValidateMovement(SlideDirection direction) {
        if (direction == currentDirection) {
            // Give points
        } else {
            // Wrong ! -1 life ?
        }
    }

    private void Next() {
        currentDirection = (SlideDirection)Random.Range(0, 4);
    }
}
