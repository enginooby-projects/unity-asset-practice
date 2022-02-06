using UnityEditor;
using System.IO;

namespace Enginoobz.Editor {
  public static class Generator {
    // REFACTOR: Generate file names
    // TODO: Auto execute on Gizmos directory change
    [MenuItem("Enginoobz/Generator/Generate Gizmos Icons")]
    public static void GenerateGizmosIcons() {
      var enumName = "GizmosIcon";
      var filePath = "Assets/Gizmos/" + enumName + ".cs";
      var dir = new DirectoryInfo("Assets/Gizmos");
      var info = dir.GetAllFilesIgnoreMeta();

      // TODO: Create new file if not found
      using (var streamWriter = new StreamWriter(filePath)) {
        streamWriter.WriteLine("public static class " + enumName);
        streamWriter.WriteLine("{");
        foreach (var file in info) {
          if (file.Extension == ".cs") continue;
          streamWriter.WriteLine("\t public static readonly string " + file.GetNameWithoutExtension().RemoveWhitespace() + " = \"" + file.Name + "\";");
        }
        streamWriter.WriteLine("}");
      }
      AssetDatabase.Refresh();
    }
  }
}