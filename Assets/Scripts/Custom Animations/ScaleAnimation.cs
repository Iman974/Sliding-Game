using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "New scale animation", menuName = "Game/Custom Animations/Scale")]
public class ScaleAnimation : CustomAnimation {

    [SerializeField] private Vector3 targetScale;
    //[SerializeField] private float cycleOffset;

    public override Type AnimatedComponent {
        get {
            return typeof(Transform);
        }
    }

    public override IEnumerator GetAnimation(Component transform) {
        return AnimationUtility.ScaleTransform((Transform)transform, targetScale, animationCurve, animationSpeed);
    }
}
