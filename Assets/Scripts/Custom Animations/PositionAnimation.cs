using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "New position animation", menuName = "Game/Custom Animations/Position")]
public class PositionAnimation : CustomAnimation {

    [SerializeField] private float moveDistance = 500f;

    public override Type AnimatedComponent {
        get {
            return typeof(RectTransform);
        }
    }

    public override IEnumerator GetAnimation(Component transform) {
        Vector2 slideDirection = DirectionUtility.DirectionToVector(Game.CurrentDirection) * moveDistance;

        return AnimationUtility.MoveToPosition((RectTransform)transform, slideDirection, animationCurve, animationSpeed);
    }
}
