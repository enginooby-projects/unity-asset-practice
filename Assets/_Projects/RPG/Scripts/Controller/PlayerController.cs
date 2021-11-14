using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgentOperator))]
public class PlayerController : MonoBehaviour {
  void Update() {
    HandleMovement();
    HandleAnimation();
  }

  #region MOVEMENT ===================================================================================================================================
  [AutoRef, SerializeField, HideInInspector]
  private NavMeshAgentOperator _agentOpr;
  private Ray _lastRay;


  private void HandleMovement() {
    HandleMoveToCursor();
  }

  private void HandleMoveToCursor() {
    Debug.DrawRay(_lastRay.origin, _lastRay.direction * 100, Color.red);

    if (!MouseButton.Left.IsHeld()) return;
    _lastRay = Camera.main.ScreenPointToRay(Input.mousePosition); //
    if (Physics.Raycast(_lastRay, out RaycastHit hit)) {
      _agentOpr.MoveTo(hit.point);
    }
  }
  #endregion ===================================================================================================================================

  #region ANIMATION ===================================================================================================================================
  [AutoRef, SerializeField, HideInInspector]
  private Animator _animator;

  private void HandleAnimation() {
    _animator.SetFloat("forwardSpeed", _agentOpr.LocalVelocity.z); // TODO: Hash string
  }
  #endregion ===================================================================================================================================
}
