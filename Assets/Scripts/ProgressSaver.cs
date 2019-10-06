using UnityEngine;

public static class ProgressSaver {

    public static void SaveHighscore(int highscore) {
        PlayerPrefs.SetInt("highscore", highscore);
        PlayerPrefs.Save();
    }

    public static int LoadHighscore() {
        return PlayerPrefs.GetInt("highscore");
    }
}
