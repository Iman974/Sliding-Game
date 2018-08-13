using UnityEngine;

public static class RandomUtility {

    public static T PickRandomItemInArray<T>(T[] array) {
        return array[Random.Range(0, array.Length)];
    }
}
