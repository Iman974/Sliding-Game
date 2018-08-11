using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

//public class SerializableDictionnaryEditor<TKey, TValue> : Editor {

//    SerializableDictionnary<TKey, TValue> dictionnary;

//    private void OnEnable() {
//        dictionnary = (SerializableDictionnary<TKey, TValue>)target;
//    }
//}

[CustomEditor(typeof(SerializableDictionary_SlideDirection))]
public class SerializableDictionary_SlideDirectionEditor : Editor {

    private SerializableDictionary_SlideDirection dictionary;

    private void OnEnable() {
        dictionary = (SerializableDictionary_SlideDirection)target;
    }

    public override void OnInspectorGUI() {
        if (dictionary == null) {
            return;
        }

        var keys = new SlideDirection[dictionary.Keys.Count];
        var values = new SlideDirection[dictionary.Values.Count];

        dictionary.Keys.CopyTo(keys, 0);
        dictionary.Values.CopyTo(values, 0);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Input Direction", GUIStyleUtility.boldFont, GUILayout.Width(150f));
        EditorGUILayout.LabelField("Output Direction", GUIStyleUtility.boldFont);
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < dictionary.Count; i++) {
            EditorGUILayout.BeginHorizontal();

            SlideDirection newKey = (SlideDirection)EditorGUILayout.EnumPopup(keys[i]);
            SlideDirection newValue = (SlideDirection)EditorGUILayout.EnumPopup(values[i]);

            if (newKey != keys[i]) {
                if (Array.IndexOf(keys, newKey) > -1) {
                    dictionary.Remove(keys[i]);
                    dictionary.Add(newKey, newValue);

                    EditorUtility.SetDirty(dictionary);
                }
            } else if (newValue != values[i]) {
                dictionary[keys[i]] = newValue;

                EditorUtility.SetDirty(dictionary);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
