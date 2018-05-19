using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New rotation animation", menuName = "Game/Custom Animations/Rotation")]
public class RotationAnimation : CustomAnimation {

    [SerializeField] private Vector3 targetRotation;
    //[SerializeField] private VectorUtility.Vector3Bool leaveUnchanged;
    //[SerializeField] private VectorUtility.Vector3Axis axisToRotate;
    //[SerializeField] private VectorUtility.Vector3Axis rotateAxisWith;

    //private Vector3 unchangedRotation;

    public override Type AnimatedComponent {
        get {
            return typeof(Transform);
        }
    }

    private void OnEnable() {
        //unchangedRotation = targetRotation;
    }

    public override IEnumerator GetAnimation(Component transform) {
        //for (int i = 0; i < 3; i++) { // The use of this every animation call is not very optimised !
        //    if (leaveUnchanged[i]) {
        //        unchangedRotation[i] = ((Transform)transform).localEulerAngles[i];
        //    }
        //}

        //((Transform)transform).Rotate()
        //float axisValue = VectorUtility.GetMatchingComponent(unchangedRotation, axisToRotate);
        //unchangedRotation = VectorUtility.GetMatchingVector(axisValue, rotateAxisWith);

        return AnimationUtility.RotateTransform((Transform)transform, targetRotation, animationCurve, animationSpeed);
    }
}
