using UnityEngine;

[CreateAssetMenu(fileName = "New dictionnary", menuName = "Game/Utility/Dictionnary")]
public class SerializableDictionnary_SlideDirection : SerializableDictionnary<SlideDirection, SlideDirection> {

    //public static SerializableDictionnary_SlideDirection Default { get; private set; }

    //private void OnEnable() {
    //    Default = new SerializableDictionnary_SlideDirection();

    //    foreach (SlideDirection direction in DirectionUtility.DirectionValues) {
    //        Default.Add(direction, direction);
    //    }
    //}

    public SerializableDictionnary_SlideDirection() {
        foreach (SlideDirection direction in DirectionUtility.DirectionValues) {
            dictionnary.Add(direction, direction);
        }
    }
}
