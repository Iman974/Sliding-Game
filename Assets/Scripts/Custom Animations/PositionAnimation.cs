using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "New position animation", menuName = "Game/Custom Animations/Position")]
public class PositionAnimation : CustomAnimation {

    [SerializeField] private float moveDistance = 100f;

    public override Type AnimatedComponent {
        get {
            return typeof(RectTransform);
        }
    }

    public override IEnumerator GetAnimation(Component transform) {
        Vector2 slideDirection = DirectionUtility.DirectionToVector(GameManager.CurrentDirection) * moveDistance;

        return AnimationUtility.MoveToPosition((RectTransform)transform, slideDirection, animationCurve, animationSpeed);
    }
}
