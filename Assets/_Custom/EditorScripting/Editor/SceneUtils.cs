using UnityEditor.SceneManagement;
using UnityEngine;

namespace Enginoobz.Editor {
  public static class SceneUtils {
    public static void CreateEmptyScene() {
      EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
      EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
    }

    public static void CleanScene() {
      // ? Separate confirm dialog code   
      if (EditorUtils.DisplayDialog("Are you sure to remove all GameObjects?")) {
        // UTIL
        var gos = Object.FindObjectsOfType<GameObject>();
        foreach (var go in gos) {
          GameObject.DestroyImmediate(go);
        }
      }
    }
  }
}