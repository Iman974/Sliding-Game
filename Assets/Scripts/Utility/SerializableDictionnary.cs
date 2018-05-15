using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class SerializableDictionnary<TKey, TValue> : ScriptableObject, ISerializationCallbackReceiver, IDictionary<TKey,TValue>,
    IDictionary {

    private List<TKey> keys = new List<TKey>();
    private List<TValue> values = new List<TValue>();

    //public object Current {
    //    get {
    //        return new KeyValuePair<TKey, TValue>(keys[], values[]);
    //    }
    //}

    private Dictionary<TKey, TValue> dictionnary = new Dictionary<TKey, TValue>();

    public TValue this[TKey key] {
        get {
            return ((IDictionary<TKey, TValue>)dictionnary)[key];
        }

        set {
            ((IDictionary<TKey, TValue>)dictionnary)[key] = value;
        }
    }

    public object this[object key] {
        get {
            return ((IDictionary)dictionnary)[key];
        }

        set {
            ((IDictionary)dictionnary)[key] = value;
        }
    }

    public ICollection<TKey> Keys {
        get {
            return ((IDictionary<TKey, TValue>)dictionnary).Keys;
        }
    }

    public ICollection<TValue> Values {
        get {
            return ((IDictionary<TKey, TValue>)dictionnary).Values;
        }
    }

    public int Count {
        get {
            return ((IDictionary<TKey, TValue>)dictionnary).Count;
        }
    }

    public bool IsReadOnly {
        get {
            return ((IDictionary<TKey, TValue>)dictionnary).IsReadOnly;
        }
    }

    public bool IsFixedSize {
        get {
            return ((IDictionary)dictionnary).IsFixedSize;
        }
    }

    public bool IsSynchronized {
        get {
            return ((IDictionary)dictionnary).IsSynchronized;
        }
    }

    public object SyncRoot {
        get {
            return ((IDictionary)dictionnary).SyncRoot;
        }
    }

    ICollection IDictionary.Keys {
        get {
            return ((IDictionary)dictionnary).Keys;
        }
    }

    ICollection IDictionary.Values {
        get {
            return ((IDictionary)dictionnary).Values;
        }
    }

    public void Add(TKey key, TValue value) {
        ((IDictionary<TKey, TValue>)dictionnary).Add(key, value);
    }

    public void Add(KeyValuePair<TKey, TValue> item) {
        ((IDictionary<TKey, TValue>)dictionnary).Add(item);
    }

    public void Add(object key, object value) {
        ((IDictionary)dictionnary).Add(key, value);
    }

    public void Clear() {
        ((IDictionary<TKey, TValue>)dictionnary).Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) {
        return ((IDictionary<TKey, TValue>)dictionnary).Contains(item);
    }

    public bool Contains(object key) {
        return ((IDictionary)dictionnary).Contains(key);
    }

    public bool ContainsKey(TKey key) {
        return ((IDictionary<TKey, TValue>)dictionnary).ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
        ((IDictionary<TKey, TValue>)dictionnary).CopyTo(array, arrayIndex);
    }

    public void CopyTo(Array array, int index) {
        ((IDictionary)dictionnary).CopyTo(array, index);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
        return ((IDictionary<TKey, TValue>)dictionnary).GetEnumerator();
    }

    public void OnBeforeSerialize() {
        keys.Clear();
        values.Clear();

        foreach (var keyValuePair in dictionnary) {
            keys.Add(keyValuePair.Key);
            values.Add(keyValuePair.Value);
        }
    }

    public void OnAfterDeserialize() {
        dictionnary = new Dictionary<TKey, TValue>();

        for (int i = 0; i < Mathf.Min(keys.Count, values.Count); i++) {
            dictionnary.Add(keys[i], values[i]);
        }
    }

    public bool Remove(TKey key) {
        return ((IDictionary<TKey, TValue>)dictionnary).Remove(key);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) {
        return ((IDictionary<TKey, TValue>)dictionnary).Remove(item);
    }

    public void Remove(object key) {
        ((IDictionary)dictionnary).Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value) {
        return ((IDictionary<TKey, TValue>)dictionnary).TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return ((IDictionary<TKey, TValue>)dictionnary).GetEnumerator();
    }

    IDictionaryEnumerator IDictionary.GetEnumerator() {
        return ((IDictionary)dictionnary).GetEnumerator();
    }

    //public int KeyCount {
    //    get {
    //        return keys.Count;
    //    }
    //}

    //public int ValueCount {
    //    get {
    //        return values.Count;
    //    }
    //}

    //public TValue this[TKey index] {
    //    get {
    //        int valueIndex = keys.FindIndex(x => EqualityComparer<TKey>.Default.Equals(x, index));

    //        if (valueIndex == -1) {
    //            throw new KeyNotFoundException("The key " + index + " was not found.");
    //        }

    //        return values[valueIndex];
    //    }
    //    set {
    //        int valueIndex = keys.FindIndex(x => EqualityComparer<TKey>.Default.Equals(x, index));

    //        if (valueIndex == -1) {
    //            values.Add(value);
    //        } else {
    //            values[valueIndex] = value;
    //        }
    //    }
    //}

    //public TValue this[int index] {
    //    get {
    //        return values[index];
    //    }
    //    set {
    //        values[index] = value;
    //    }
    //}

    //public void Add(TKey key, TValue value) {
    //    new Dictionary<string, int>()["d"] = 5;
    //    int valueIndex = keys.FindIndex(x => EqualityComparer<TKey>.Default.Equals(x, key));

    //    if (keys.Contains(key)) {
    //        throw new ArgumentException("A value with the key " + key + " already exists.");
    //    }

    //    keys.Add(key);
    //    values.Add(value);
    //}

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
