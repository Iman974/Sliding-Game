using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New rotation animation", menuName = "Game/Custom Animations/Rotation")]
public class RotationAnimation : CustomAnimation {

    [SerializeField] private Vector3 targetRotation;

    public override Type AnimatedComponent {
        get {
            return typeof(Transform);
        }
    }

    public override IEnumerator GetAnimation(Component transform) {
        return AnimationUtility.RotateTransform((Transform)transform, Quaternion.Euler(targetRotation), animationCurve, animationSpeed);
    }
}
