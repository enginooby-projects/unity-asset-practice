using UnityEditor;

namespace Enginoobz.Editor {
  public static class MenuItems {
    // Menu header hierarchy
    private const string H1 = "Enginoobz/";
    private const string HSCENE = H1 + "Scene/";

    [MenuItem(HSCENE + "Create Empty Scene")]
    private static void CreateEmptyScene() => SceneUtils.CreateEmptyScene();

    [MenuItem(HSCENE + "Clean Scene")]
    private static void CleanScene() => SceneUtils.CleanScene();

    // UTIL
    public static string ToSentenceCase(this string str) {
      return System.Text.RegularExpressions.Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
    }
  }
}