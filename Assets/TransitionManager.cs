using System.Collections;
using UnityEngine;

public class TransitionManager : MonoBehaviour {

    [SerializeField] private Transform gameCanvasTransform;
    [SerializeField] private Transform mainMenuCanvasTransform;
    [SerializeField] private AnimationCurve slideCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float slideSpeed = 1f;

    private Transform mainCameraTransform;

    private void Start() {
        mainCameraTransform = Camera.main.transform;
    }

    public void SlideCameraTo(Transform target) {
        StartCoroutine(SlideCamera(target));
    }

    private IEnumerator SlideCamera(Transform target) {
        Vector3 destination = target.position;
        destination.z = mainCameraTransform.position.z;

        return AnimationUtility.MoveToPosition(mainCameraTransform, destination, slideCurve, slideSpeed);
    }
}
