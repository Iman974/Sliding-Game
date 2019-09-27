using UnityEngine;

public static class DirectionUtility {

    public static readonly int kDirectionCount;

    static DirectionUtility() {
        kDirectionCount = System.Enum.GetValues(typeof(Direction)).Length;
    }

    public static Vector2 DirectionToVector(Direction direction) {
        Vector2 matchingVector;
        switch (direction) {
            case Direction.Left:
                matchingVector = Vector2.left;
                break;
            case Direction.Right:
                matchingVector = Vector2.right;
                break;
            case Direction.Up:
                matchingVector = Vector2.up;
                break;
            default:
                matchingVector = Vector2.down;
                break;
        }
        return matchingVector;
    }

    public static Direction VectorToDirection(Vector2 vector) {
        Direction matchingDirection;
        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y)) {
            matchingDirection = vector.x >= 0f ? Direction.Right : Direction.Left;
        } else {
            matchingDirection = vector.y >= 0f ? Direction.Up : Direction.Down;
        }
        return matchingDirection;
    }

    public static float DirectionToRotation(Direction direction) {
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

    public static Direction GetRandomDirection() {
        return (Direction)Random.Range(0, kDirectionCount);
    }
}
