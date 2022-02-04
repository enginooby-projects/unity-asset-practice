using UnityEngine;
using UnityEditor;

namespace Enginoobz.Editor {
  /// <summary>
  /// Separate Gizmos drawing logic from MonoBehaviour using DrawGizmo attribute. <br\>
  /// Or to supply Gizmos for MonoBehaviour that we cannot modify.
  /// </summary>
  public static class GizmosUtils {
    // [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.Active)]
    // private static void DrawCube<T>(T component, GizmoType gizmoType) where T : MonoBehaviour {
    //   Gizmos.color = Color.green;
    //   Gizmos.DrawCube(component.transform.position, Vector3.one);
    // }

    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    private static void DrawWireCube(TestDrawGizmoAttribute component, GizmoType gizmoType) {
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(component.transform.position, Vector3.one);
    }
  }
}