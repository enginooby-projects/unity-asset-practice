using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HelloEditorScripting {
#if UNITY_EDITOR
  [MenuItem("GameObject/Create New GameObject (From HelloEditorScripting)")]
  private static void CreateNewGameObject() {
    if (EditorUtility.DisplayDialog(
      "Hello Editor Scripting",
      "Do you want to create a new GameObject?",
      "Yes",
      "No"
    )) {
      new GameObject("Hello Editor Scripting");
    }
  }
#endif
}
