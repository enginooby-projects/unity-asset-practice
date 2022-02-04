using UnityEditor;

namespace Enginoobz.Editor {
  public static class EditorUtils {
    public static bool DisplayDialog(string message, string title = "", string ok = "Yes", string cancel = "No") {
      return EditorUtility.DisplayDialog(title, message, ok, cancel);
    }
  }
}