#if UNITY_EDITOR
using UnityEditor;

namespace Enginoobz.Utils {
  public static class EditorUtils {
    public static void StopPlayMode() {
      UnityEditor.EditorApplication.isPlaying = false;
      // Application.Quit();
    }

    public static bool IsInteger(this SerializedProperty property) {
      return property.propertyType == SerializedPropertyType.Integer;
    }
  }
}
#endif
