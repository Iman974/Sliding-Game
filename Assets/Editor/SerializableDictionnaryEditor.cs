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

[CustomEditor(typeof(SerializableDictionnary_SlideDirection))]
public class SerializableDictionnary_SlideDirectionEditor : Editor {

    SerializableDictionnary<SlideDirection, SlideDirection> dictionnary;

    private void OnEnable() {
        dictionnary = (SerializableDictionnary<SlideDirection, SlideDirection>)target;
    }

    public override void OnInspectorGUI() {
        if (dictionnary == null) {
            return;
        }

        var keys = new List<SlideDirection>(dictionnary.Keys);
        var values = new List<SlideDirection>(dictionnary.Values);

        for (int i = 0; i < dictionnary.Count; i++) {
            EditorGUILayout.BeginHorizontal();

            SlideDirection newKey = (SlideDirection)EditorGUILayout.EnumPopup(keys[i]);
            SlideDirection newValue = (SlideDirection)EditorGUILayout.EnumPopup(values[i]);

            if (newKey != keys[i]) {
                if (!keys.Contains(newKey)) {
                    dictionnary.Remove(keys[i]);
                    dictionnary.Add(newKey, newValue);
                }
            } else if (newValue != values[i]) {
                dictionnary[keys[i]] = newValue;
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add key")) {
            SlideDirection newDirection = 0;
            while (keys.Contains(newDirection)) {
                newDirection++;
            }

            if ((int)newDirection >= Enum.GetNames(typeof(SlideDirection)).Length) {
                Debug.LogWarning("Cannot add any more entries to the dictionnary.");
                return;
            }

            dictionnary.Add(newDirection, 0);
        }
    }
}
