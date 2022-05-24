using UnityEngine;

public class PlayerSaves : MonoBehaviour {
    public static void SetLevelBeaten(string levelName) {
        PlayerPrefs.SetInt(levelName, 1);
    }

    public static bool IsLevelBeaten(string levelName) {
        return PlayerPrefs.GetInt(levelName, 0) == 1;
    }

    public static void ClearUserSaves() {
        PlayerPrefs.DeleteAll();
    }
}
