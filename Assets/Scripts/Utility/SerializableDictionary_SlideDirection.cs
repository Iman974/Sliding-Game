using UnityEngine;

[CreateAssetMenu(fileName = "New dictionary", menuName = "Game/Utility/Dictionary")]
public class SerializableDictionary_SlideDirection : SerializableDictionary<Direction, Direction> {

    //public static SerializableDictionnary_SlideDirection Default { get; private set; }

    //private void OnEnable() {
    //    Default = new SerializableDictionnary_SlideDirection();

    //    foreach (SlideDirection direction in DirectionUtility.DirectionValues) {
    //        Default.Add(direction, direction);
    //    }
    //}

    public SerializableDictionary_SlideDirection() {
        foreach (Direction direction in DirectionUtility.DirectionValues) {
            dictionary.Add(direction, direction);
        }
    }
}
