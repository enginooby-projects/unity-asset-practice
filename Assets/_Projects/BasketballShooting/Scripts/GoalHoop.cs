using UnityEngine;

namespace Project.BasketballShooting {
  // Generate small sphere colliders around the net
  public class GoalHoop : MonoBehaviour {
    private void Start() {
      const float rr = 1.046f;
      var rc = new Vector3(0.088f, 2.874f, 1.109f);

      for (var i = 0; i < 360; i += 30) {
        var rad = (i * Mathf.PI) / 180.0f;
        var sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.center = new Vector3(rc.x + rr * Mathf.Sin(rad), rc.y, rc.z + rr * Mathf.Cos(rad));
        sphereCollider.radius = 0.042f;
      }
    }
  }
}