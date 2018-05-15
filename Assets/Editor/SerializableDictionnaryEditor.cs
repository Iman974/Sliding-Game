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

    private SerializableDictionnary_SlideDirection dictionnary;
    int enumElementCount = Enum.GetNames(typeof(SlideDirection)).Length;
    SlideDirection[] enumValues = (SlideDirection[])Enum.GetValues(typeof(SlideDirection));
    //private bool displayAddButton;

    private void OnEnable() {
        dictionnary = (SerializableDictionnary_SlideDirection)target;

        //if (dictionnary.Count < enumElementCount) {
        //    displayAddButton = true;
        //}
    }

    public override void OnInspectorGUI() {
        if (dictionnary == null) {
            return;
        }

        var keys = new SlideDirection[dictionnary.Keys.Count];
        var values = new SlideDirection[dictionnary.Values.Count];

        dictionnary.Keys.CopyTo(keys, 0);
        dictionnary.Values.CopyTo(values, 0);

        for (int i = 0; i < dictionnary.Count; i++) {
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
                    dictionnary.Remove(keys[i]);
                    dictionnary.Add(newKey, newValue);

                    EditorUtility.SetDirty(dictionnary);
                }
            } else if (newValue != values[i]) {
                dictionnary[keys[i]] = newValue;

                EditorUtility.SetDirty(dictionnary);
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
