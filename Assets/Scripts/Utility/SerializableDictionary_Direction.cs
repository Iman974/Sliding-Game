using UnityEngine;

[CreateAssetMenu(fileName = "New binder", menuName = "Game/Direction binder")]
public class SerializableDictionary_Direction : SerializableDictionary<Direction, Direction> {

    public SerializableDictionary_Direction() {
        foreach (Direction direction in DirectionUtility.DirectionValues) {
            dictionary.Add(direction, direction);
        }
    }
}
