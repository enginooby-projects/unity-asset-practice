using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enginoobz.Operator;
using Project.RPG.Combat;
using static RayUtils;

namespace Project.RPG.Controller {
  [RequireComponent(typeof(NavMeshAgentOperator))]
  public class PlayerController : MonoBehaviour {
    void Update() {
      if (CanAttack) {
        HandleCombat();
      } else if (CanMove) {
        HandleMovement();
        HandleAnimation();
      }
    }

    #region MOVEMENT ===================================================================================================================================
    [AutoRef, SerializeField, HideInInspector]
    private NavMeshAgentOperator _agentOpr;
    private Ray _lastRay;
    private bool CanMove => IsMouseRayHit;

    private void HandleMovement() {
      Debug.DrawRay(_lastRay.origin, _lastRay.direction * 100, Color.red);

      if (!MouseButton.Left.IsHeld()) return;
      _lastRay = MouseRay;
      if (Physics.Raycast(_lastRay, out RaycastHit hit)) {
        _agentOpr.MoveTo(hit.point);
      }
    }
    #endregion ===================================================================================================================================

    #region ANIMATION ===================================================================================================================================
    [AutoRef, SerializeField, HideInInspector]
    private Animator _animator;
    private int _forwardSpeedHash = Animator.StringToHash("forwardSpeed");

    private void HandleAnimation() {
      _animator.SetFloat(_forwardSpeedHash, _agentOpr.LocalVelocity.z);
    }
    #endregion ===================================================================================================================================

    #region COMBAT ===================================================================================================================================
    [AutoRef, SerializeField, HideInInspector]
    private Fighter _fighter;
    private List<CombatTarget> currentAttackableTargets = new List<CombatTarget>();

    private bool CanAttack {
      get {
        currentAttackableTargets = GetComponentsViaMouseRay<CombatTarget>();
        return currentAttackableTargets.Count > 0;
      }
    }

    private void HandleCombat() {
      if (!MouseButton.Left.IsDown()) return;

      currentAttackableTargets.ForEach(_fighter.Attack);
    }
    #endregion ===================================================================================================================================

  }
}
