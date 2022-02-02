using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;

namespace Enginoobz.Editor {
  public static class SceneUtils {
    [MenuItem("Enginoobz/Scene/Create Empty Scene")]
    public static void CreateEmptyScene() {
      EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
      EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
    }

    [MenuItem("Enginoobz/Scene/Clean Scene")]
    public static void CleanScene() {
      if (EditorUtility.DisplayDialog(
        "Clean Scene",
        "Are you sure to remove all GameObjects?",
        "Yes",
        "No"
      )) {
        var gos = Object.FindObjectsOfType<GameObject>();
        foreach (var go in gos) {
          GameObject.DestroyImmediate(go);
        }
      }
    }
  }
}