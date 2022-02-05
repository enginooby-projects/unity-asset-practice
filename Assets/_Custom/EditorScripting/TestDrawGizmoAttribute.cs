using UnityEngine;

public class TestDrawGizmoAttribute : MonoBehaviour {
  private void OnDrawGizmos() {
    Gizmos.DrawIcon(transform.position, GizmosIcon.Robot, true, Color.blue);
  }
}
