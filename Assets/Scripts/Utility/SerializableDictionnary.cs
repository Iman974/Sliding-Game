using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class SerializableDictionnary<TKey, TValue> : ScriptableObject/*, IEnumerable, IEnumerator*/ {

    private List<TKey> keys = new List<TKey>();
    private List<TValue> values = new List<TValue>();

    //public object Current {
    //    get {
    //        return new KeyValuePair<TKey, TValue>(keys[], values[]);
    //    }
    //}

    public int KeyCount {
        get {
            return keys.Count;
        }
    }

    public int ValueCount {
        get {
            return values.Count;
        }
    }

    public TValue this[TKey index] {
        get {
            int valueIndex = keys.FindIndex(x => EqualityComparer<TKey>.Default.Equals(x, index));

            if (valueIndex == -1) {
                throw new KeyNotFoundException("The key " + index + " was not found.");
            }

            return values[valueIndex];
        }
        set {
            int valueIndex = keys.FindIndex(x => EqualityComparer<TKey>.Default.Equals(x, index));

            if (valueIndex == -1) {
                values.Add(value);
            } else {
                values[valueIndex] = value;
            }
        }
    }

    public TValue this[int index] {
        get {
            return values[index];
        }
        set {
            values[index] = value;
        }
    }

    public void Add(TKey key, TValue value) {
        new Dictionary<string, int>()["d"] = 5;
        int valueIndex = keys.FindIndex(x => EqualityComparer<TKey>.Default.Equals(x, key));

        if (keys.Contains(key)) {
            throw new ArgumentException("A value with the key " + key + " already exists.");
        }

        keys.Add(key);
        values.Add(value);
    }

    //public IEnumerator GetEnumerator() {
    //    return this;
    //}

    //public bool MoveNext() {
    //    throw new System.NotImplementedException();
    //}

    //public void Reset() {
    //    throw new System.NotImplementedException();
    //}
}
