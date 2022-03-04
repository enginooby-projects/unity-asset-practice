using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

// REFACTOR: abstract class and make concrete class for KinematicCharacterMotor
/// <summary>
/// Used in case the controller cannot be set transform simply by transform assignment.
/// </summary>
public class ControllerMover : MonoBehaviour {
  private KinematicCharacterMotor _controller;

  private void Start() {
    _controller = GetComponent<KinematicCharacterMotor>();
  }

  public void CopyTransform(Transform target) {
    MoveTo(target.position);
    RotateTo(target.rotation);
  }

  [Button]
  public void MoveTo(Vector3 pos) {
    // _controller.MoveCharacter(pos);
    _controller?.SetPosition(pos);
  }

  [Button]
  public void RotateTo(Quaternion rot) {
    // _controller.RotateCharacter(rot);
    _controller?.SetRotation(rot);
  }
}