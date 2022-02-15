using UnityEngine;

namespace Project.HighwayRacer {
  public class HR_BarrierCollisionProtector : MonoBehaviour {
    public CollisionSide collisionSide;

    public enum CollisionSide {
      Left,
      Right
    }

    private Rigidbody _playerRigid;

    private void OnTriggerStay(Collider col) {
      if (!col.transform.root.CompareTag("Player")) return;

      var forceSide = collisionSide == CollisionSide.Right ? -1 : 1;
      _playerRigid ??= col.gameObject.GetComponentInParent<RCC_CarControllerV3>().rigid;
      _playerRigid.AddForce(Vector3.right * 50f * forceSide, ForceMode.Acceleration);
      _playerRigid.ZeroizeVelocityX();
      _playerRigid.ZeroizeAngularVelocityYZ();
    }

    private void OnDrawGizmos() {
      Gizmos.color = new Color(1f, .5f, 0f, .75f);
      Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);
    }
  }
}