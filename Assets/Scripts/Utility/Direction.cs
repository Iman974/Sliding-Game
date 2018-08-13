using UnityEngine;
using System;

public static class DirectionUtility {

    public static Direction[] DirectionValues { get; private set; }

    public static int DirectionCount { get; private set; }

    static DirectionUtility() {
        DirectionValues = (Direction[])Enum.GetValues(typeof(Direction));
        DirectionCount = DirectionValues.Length;
    }

    public static Vector2 DirectionToVector(Direction direction) {
        Vector2 convertedDirection;

        if (direction == Direction.Up) {
            convertedDirection = Vector2.up;
        } else if (direction == Direction.Right) {
            convertedDirection = Vector2.right;
        } else if (direction == Direction.Down) {
            convertedDirection = -Vector2.up;
        } else {
            convertedDirection = -Vector2.right;
        }

        return convertedDirection;
    }

    public static Direction VectorToDirection(Vector2 direction) {
        Direction convertedVector;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            convertedVector = Mathf.Sign(direction.x) == 1f ? Direction.Right : Direction.Left;
        } else {
            convertedVector = Mathf.Sign(direction.y) == 1f ? Direction.Up : Direction.Down;
        }

        return convertedVector;
    }

    public static float GetRotationFromDirection(Direction direction) {
        float rotation = 0f;

        if (direction == Direction.Right) {
            rotation = 90f;
        } else if (direction == Direction.Down) {
            rotation = 180f;
        } else if (direction == Direction.Left) {
            rotation = 270f;
        }

        return 90f - rotation;
    }
}

[Serializable]
public enum Direction {
    Left,
    Right,
    Up,
    Down
}
