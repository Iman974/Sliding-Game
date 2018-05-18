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
    //private bool displayAddButton;

    private void OnEnable() {
        dictionary = (SerializableDictionary_SlideDirection)target;

        //if (dictionnary.Count < enumElementCount) {
        //    displayAddButton = true;
        //}
    }

    public override void OnInspectorGUI() {
        if (dictionary == null) {
            return;
        }

        var keys = new SlideDirection[dictionary.Keys.Count];
        var values = new SlideDirection[dictionary.Values.Count];

        dictionary.Keys.CopyTo(keys, 0);
        dictionary.Values.CopyTo(values, 0);

        for (int i = 0; i < dictionary.Count; i++) {
            EditorGUILayout.BeginHorizontal();

            //if (GUILayout.Button("X", GUILayout.MaxHeight(15f))) {
            //    dictionnary.Remove(keys[i]);
            //    displayAddButton = true;

            //    EditorUtility.SetDirty(dictionnary);
            //}

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

        //if (!displayAddButton) {
        //    return;
        //}

        //if (GUILayout.Button("Add key")) {
        //    SlideDirection newDirection = enumValues[0];

        //    int i = 0;
        //    while (Array.IndexOf(keys, newDirection) > -1) {
        //        newDirection = enumValues[++i];
        //    }

        //    if (dictionnary.Count + 1 >= DirectionUtility.DirectionCount) {
        //        //Debug.LogWarning("Cannot add any more entries to the dictionnary.");
        //        displayAddButton = false;
        //    }

        //    dictionnary.Add(newDirection, newDirection);

        //    EditorUtility.SetDirty(dictionnary);
        //}
    }
}
