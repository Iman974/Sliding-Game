using UnityEngine;
using System.Linq;

public static class RandomUtility {

    public interface IWeighted {
        int Weight { get; }
    }

    // Algorithm found on the Unity forum:
    // https://forum.unity.com/threads/random-numbers-with-a-weighted-chance.442190/
    public static int SelectRandomWeightedIndex(IWeighted[] weightedObjects) {
        int weightSum = weightedObjects.Sum(x => x.Weight);
        int p = 0;
        int randomValue = Random.Range(0, weightSum);
        for (int i = 0; i < weightedObjects.Length - 1; i++) {
            p += weightedObjects[i].Weight;
            if (randomValue < p) {
                return i;
            }
        }
        return weightedObjects.Length - 1;
    }
}
