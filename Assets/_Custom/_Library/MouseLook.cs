using Sirenix.OdinInspector;
using UnityEngine;

// ! Not working
public class MouseLook : MonoBehaviour {
  [SerializeField, EnumToggleButtons]
  private AxisFlag _lookAxis = AxisFlag.X | AxisFlag.Z;

  private Vector3 _rotation;

  // TIP: Manually transform part of animated character in LateUpdate
  private void LateUpdate() {
    Vector3 upAxis = new Vector3(0, 0, -1);
    Vector3 mouseScreenPosition = Input.mousePosition;

    //set mouses z to your targets
    mouseScreenPosition.z = transform.position.z;
    Vector3 mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    transform.LookAt(mouseWorldSpace, upAxis);

    //zero out all rotations except the axis I want
    // REFACTOR
    _rotation = transform.eulerAngles;
    if (!_lookAxis.HasFlag(AxisFlag.X)) {
      _rotation = _rotation.WithX(0);
    }
    if (!_lookAxis.HasFlag(AxisFlag.Y)) {
      _rotation = _rotation.WithY(0);
    }
    if (!_lookAxis.HasFlag(AxisFlag.Z)) {
      _rotation = _rotation.WithZ(0);
    }
    transform.eulerAngles = _rotation;
  }
}
