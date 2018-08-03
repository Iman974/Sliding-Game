using UnityEngine;
using System;

public static class DirectionUtility {

    public static SlideDirection[] DirectionValues { get; private set; }

    public static int DirectionCount { get; private set; }

    static DirectionUtility() {
        DirectionValues = (SlideDirection[])Enum.GetValues(typeof(SlideDirection));
        DirectionCount = DirectionValues.Length;
    }

    public static Vector2 DirectionToVector(SlideDirection direction) {
        Vector2 convertedDirection;

        if (direction == SlideDirection.Up) {
            convertedDirection = Vector2.up;
        } else if (direction == SlideDirection.Right) {
            convertedDirection = Vector2.right;
        } else if (direction == SlideDirection.Down) {
            convertedDirection = -Vector2.up;
        } else {
            convertedDirection = -Vector2.right;
        }

        return convertedDirection;
    }

    public static SlideDirection VectorToDirection(Vector2 direction) {
        SlideDirection convertedVector;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            convertedVector = Mathf.Sign(direction.x) == 1f ? SlideDirection.Right : SlideDirection.Left;
        } else {
            convertedVector = Mathf.Sign(direction.y) == 1f ? SlideDirection.Up : SlideDirection.Down;
        }

        return convertedVector;
    }

    public static float GetRotationFromDirection(SlideDirection direction) {
        float rotation = 0f;

        if (direction == SlideDirection.Right) {
            rotation = 90f;
        } else if (direction == SlideDirection.Down) {
            rotation = 180f;
        } else if (direction == SlideDirection.Left) {
            rotation = 270f;
        }

        return 90f - rotation;
    }
}

[Serializable]
public enum SlideDirection {
    Left,
    Right,
    Up,
    Down
}
