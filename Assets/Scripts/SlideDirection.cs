using UnityEngine;
using System;

public static class DirectionUtility {

    /// <summary>
    /// The values of the SlideDirection enum.
    /// </summary>
    public static SlideDirection[] DirectionValues { get; private set; }

    /// <summary>
    /// How many elements the SlideDirection enum contains.
    /// </summary>
    public static int DirectionCount { get; private set; }

    static DirectionUtility() {
        DirectionValues = (SlideDirection[])Enum.GetValues(typeof(SlideDirection));
        DirectionCount = DirectionValues.Length;
    }

    /// <summary>
    /// Converts the given direction to a vector.
    /// </summary>
    /// <param name="direction">
    /// The direction to convert from.
    /// </param>
    /// <returns>
    /// Returns the converted direction as a vector.
    /// </returns>
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

    /// <summary>
    /// Converts the given vector to a direction.
    /// </summary>
    /// <param name="direction">
    /// The vector to convert from.
    /// </param>
    /// <returns>
    /// Returns the converted vector as a direction.
    /// </returns>
    public static SlideDirection VectorToDirection(Vector2 direction) {
        SlideDirection convertedVector;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            convertedVector = Mathf.Sign(direction.x) == 1f ? SlideDirection.Right : SlideDirection.Left;
        } else {
            convertedVector = Mathf.Sign(direction.y) == 1f ? SlideDirection.Up : SlideDirection.Down;
        }

        return convertedVector;
    }
}

[Serializable]
public enum SlideDirection {
    Left,
    Right,
    Up,
    Down
}
