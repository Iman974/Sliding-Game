using UnityEngine;
using System;

public static class DirectionUtility {

    public static Direction[] DirectionValues { get; private set; }

    static DirectionUtility() {
        DirectionValues = (Direction[])Enum.GetValues(typeof(Direction));
    }

    public static Vector2 DirectionToVector(Direction direction) {
        Vector2 matchingVector;

        if (direction == Direction.Up) {
            matchingVector = Vector2.up;
        } else if (direction == Direction.Right) {
            matchingVector = Vector2.right;
        } else if (direction == Direction.Down) {
            matchingVector = -Vector2.up;
        } else {
            matchingVector = -Vector2.right;
        }

        return matchingVector;
    }

    public static Direction VectorToDirection(Vector2 vector) {
        Direction matchingDirection;

        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y)) {
            matchingDirection = Mathf.Sign(vector.x) == 1f ? Direction.Right : Direction.Left;
        } else {
            matchingDirection = Mathf.Sign(vector.y) == 1f ? Direction.Up : Direction.Down;
        }

        return matchingDirection;
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
