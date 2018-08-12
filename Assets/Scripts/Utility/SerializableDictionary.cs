using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class SerializableDictionary<TKey, TValue> : ScriptableObject, ISerializationCallbackReceiver, IDictionary<TKey,TValue>,
    IDictionary {

    [SerializeField] protected List<TKey> keys = new List<TKey>();
    [SerializeField] protected List<TValue> values = new List<TValue>();

    protected Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    public TValue this[TKey key] {
        get {
            return ((IDictionary<TKey, TValue>)dictionary)[key];
        }

        set {
            ((IDictionary<TKey, TValue>)dictionary)[key] = value;
        }
    }

    public object this[object key] {
        get {
            return ((IDictionary)dictionary)[key];
        }

        set {
            ((IDictionary)dictionary)[key] = value;
        }
    }

    public ICollection<TKey> Keys {
        get {
            return ((IDictionary<TKey, TValue>)dictionary).Keys;
        }
    }

    public ICollection<TValue> Values {
        get {
            return ((IDictionary<TKey, TValue>)dictionary).Values;
        }
    }

    public int Count {
        get {
            return ((IDictionary<TKey, TValue>)dictionary).Count;
        }
    }

    public bool IsReadOnly {
        get {
            return ((IDictionary<TKey, TValue>)dictionary).IsReadOnly;
        }
    }

    public bool IsFixedSize {
        get {
            return ((IDictionary)dictionary).IsFixedSize;
        }
    }

    public bool IsSynchronized {
        get {
            return ((IDictionary)dictionary).IsSynchronized;
        }
    }

    public object SyncRoot {
        get {
            return ((IDictionary)dictionary).SyncRoot;
        }
    }

    ICollection IDictionary.Keys {
        get {
            return ((IDictionary)dictionary).Keys;
        }
    }

    ICollection IDictionary.Values {
        get {
            return ((IDictionary)dictionary).Values;
        }
    }

    public SerializableDictionary() {
        dictionary = new Dictionary<TKey, TValue>();
    }

    public void Add(TKey key, TValue value) {
        ((IDictionary<TKey, TValue>)dictionary).Add(key, value);
    }

    public void Add(KeyValuePair<TKey, TValue> item) {
        ((IDictionary<TKey, TValue>)dictionary).Add(item);
    }

    public void Add(object key, object value) {
        ((IDictionary)dictionary).Add(key, value);
    }

    public void Clear() {
        ((IDictionary<TKey, TValue>)dictionary).Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) {
        return ((IDictionary<TKey, TValue>)dictionary).Contains(item);
    }

    public bool Contains(object key) {
        return ((IDictionary)dictionary).Contains(key);
    }

    public bool ContainsKey(TKey key) {
        return ((IDictionary<TKey, TValue>)dictionary).ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
        ((IDictionary<TKey, TValue>)dictionary).CopyTo(array, arrayIndex);
    }

    public void CopyTo(Array array, int index) {
        ((IDictionary)dictionary).CopyTo(array, index);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
        return ((IDictionary<TKey, TValue>)dictionary).GetEnumerator();
    }

    public void OnBeforeSerialize() {
        keys.Clear();
        values.Clear();

        foreach (var keyValuePair in dictionary) {
            keys.Add(keyValuePair.Key);
            values.Add(keyValuePair.Value);
        }
    }

    public void OnAfterDeserialize() {
        dictionary = new Dictionary<TKey, TValue>();

        for (int i = 0; i < Mathf.Min(keys.Count, values.Count); i++) {
            dictionary.Add(keys[i], values[i]);
        }
    }

    public bool Remove(TKey key) {
        return ((IDictionary<TKey, TValue>)dictionary).Remove(key);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) {
        return ((IDictionary<TKey, TValue>)dictionary).Remove(item);
    }

    public void Remove(object key) {
        ((IDictionary)dictionary).Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value) {
        return ((IDictionary<TKey, TValue>)dictionary).TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return ((IDictionary<TKey, TValue>)dictionary).GetEnumerator();
    }

    IDictionaryEnumerator IDictionary.GetEnumerator() {
        return ((IDictionary)dictionary).GetEnumerator();
    }
}
