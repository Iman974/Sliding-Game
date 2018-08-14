using UnityEngine;
using System.IO;

[System.Serializable]
public static class StateSaver {

    public static void SaveStateAsJson<T>(string stateFilePath, T stateContainer) {
        string serializedState = JsonUtility.ToJson(stateContainer);
        File.WriteAllText(stateFilePath, serializedState);
    }

    public static bool StateExists(string stateFilePath) {
        return File.Exists(stateFilePath);
    }

    public static T RetrieveStateFromJson<T>(string stateFilePath) {
        if (!File.Exists(stateFilePath)) {
            throw new FileNotFoundException("No game state file was found !");
        }

        string serializedState = File.ReadAllText(stateFilePath);
        return JsonUtility.FromJson<T>(serializedState);
    }

    public static void RetrieveStateFromJson<T>(string stateFilePath, T objectToOverwrite) where T : class {
        if (!File.Exists(stateFilePath)) {
            throw new FileNotFoundException("No game state file was found !");
        }

        string serializedState = File.ReadAllText(stateFilePath);
        JsonUtility.FromJsonOverwrite(serializedState, objectToOverwrite);
    }
}
