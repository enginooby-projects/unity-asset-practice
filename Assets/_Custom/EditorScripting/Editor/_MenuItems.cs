using UnityEditor;

namespace Enginoobz.Editor {
  public static class MenuItems {
    // Menu header hierarchy
    private const string H1 = "Enginoobz/";
    private const string H_SCENE = H1 + "Scene/";
    private const string H_GO = H1 + "GameObject/";


    [MenuItem(H_SCENE + "Create Empty Scene")]
    private static void CreateEmptyScene() => SceneUtils.CreateEmptyScene();

    [MenuItem(H_SCENE + "Clean Scene")]
    private static void CleanScene() => SceneUtils.CleanScene();

    [MenuItem(H_GO + "Create Spawner")]
    private static void CreateSpanwer() => GameObjectUtils.CreateGameObject<Spawner>();
  }
}