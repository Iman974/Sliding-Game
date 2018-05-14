using UnityEngine;
using UnityEditor;

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

        for (int i = 0; i < dictionnary.KeyCount; i++) {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.EnumPopup(dictionnary[i]);

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add key")) {
            //dictionnary[]
        }
    }
}
