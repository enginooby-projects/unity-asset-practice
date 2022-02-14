using UnityEngine;

namespace Project.ShapeTunnel {
  // REFACTOR: Create in Library ActionTrigger -> DestroyingTrigger
  public class ObjectDestroyer : MonoBehaviour {
    private void OnTriggerEnter(Collider other) => other.gameObject.Destroy(@if: !other.CompareTag("Pipe"));
  }
} 