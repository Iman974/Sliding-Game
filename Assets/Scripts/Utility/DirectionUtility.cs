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

    public static float DirectionToAngle(Direction direction) {
        switch (direction) {
            case Direction.Right:
                return 0f;
            case Direction.Down:
                return -90f;
            case Direction.Left:
                return 180f;
            case Direction.Up:
                return 90f;
            default:
                throw new System.ArgumentException("Could not convert direction to angle.");
        }
    }

    public static float DirectionToFloat(Direction direction) {
        if (direction == Direction.Right || direction == Direction.Up) {
            return 1f;
        }
        return -1f;
    }

    public static Direction GetRandomDirection() {
        return (Direction)Random.Range(0, kDirectionCount);
    }
}
