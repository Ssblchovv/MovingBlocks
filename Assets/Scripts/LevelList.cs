using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelList : MonoBehaviour {
    [SerializeField]
    private GameObject levelSelectorPrefab;

    [SerializeField]
    List<string> levels = new();

    private SceneLoader sceneLoader;

    void Start() {
        if (levelSelectorPrefab == null) {
            Debug.LogError("Нет префаба кнопки!");
            return;
        }
        sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader == null) {
            Debug.LogError("Нет загрузчика сцен!");
            return;
        }

        DrawLevelList();
    }

    public void DrawLevelList() {
        foreach (var ct in GetComponentsInChildren<Button>()) {
            Destroy(ct.gameObject);
        }
        for (int i = 0; i < levels.Count; i++) {
            var selector = Instantiate(levelSelectorPrefab);
            selector.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = (i + 1).ToString();
            var scenepath = levels[i];
            var button = selector.GetComponent<Button>();
            button.onClick.AddListener(() => {
                sceneLoader.LoadScene(scenepath);
            });
            button.enabled = i == 0 || PlayerSaves.IsLevelBeaten(levels[i-1]);
            selector.transform.SetParent(transform, false);
        }
    }
}