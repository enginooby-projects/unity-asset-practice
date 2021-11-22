using System.Collections.Generic;
using UnityEngine;
using Enginoobz.Operator;
using Enginoobz.Core;
using Enginoobz.UI;
using Project.RPG.Combat;
using static RayUtils;

namespace Project.RPG.Controller {
  [RequireComponent(typeof(NavMeshAgentOperator))]
  /// <summary>
  /// Handling inputs which invokes different actions of the player.
  /// </summary>
  public class PlayerController : MonoBehaviour {
    [SerializeField] CursorData _cursorNone;
    [SerializeField] CursorData _cursorAttack;
    [SerializeField] CursorData _cursorMove;
    private CursorData _currentCursor;

    /// <summary>
    /// [Safe-Update method]
    /// </summary>
    private void SetCurrentCursor(CursorData cursorData) {
      if (_currentCursor != cursorData) {
        _currentCursor = cursorData;
        _currentCursor.SetCursor();
      }
    }

    [AutoRef, SerializeField, HideInInspector]
    private ActionScheduler _actionScheduler;

    void Update() {
      if (CanAttackAtCursor) {
        HandleCombat();
        SetCurrentCursor(_cursorAttack);
      } else if (CanMoveToCursor) {
        HandleMovement();
        SetCurrentCursor(_cursorMove);
      } else {
        SetCurrentCursor(_cursorNone);
      }
    }

    #region MOVEMENT ===================================================================================================================================
    [AutoRef, SerializeField, HideInInspector]
    private NavMeshAgentOperator _mover;
    private Ray _lastRay;
    private bool CanMoveToCursor => IsMouseRayHit;

    private void HandleMovement() {
      Debug.DrawRay(_lastRay.origin, _lastRay.direction * 100, Color.red);

      if (!MouseButton.Left.IsHeld()) return;

      _actionScheduler.SwitchAction(_mover);
      _lastRay = MouseRay;
      if (Physics.Raycast(_lastRay, out RaycastHit hit)) {
        _mover.MoveTo(hit.point);
      }
    }
    #endregion ===================================================================================================================================

    #region COMBAT ===================================================================================================================================
    [AutoRef, SerializeField, HideInInspector]
    private Fighter _fighter;
    private List<Attackable> currentAttackableTargets = new List<Attackable>();

    private bool CanAttackAtCursor {
      get {
        currentAttackableTargets = GetComponentsViaMouseRay<Attackable>();
        return currentAttackableTargets.Count > 0;
      }
    }

    private void HandleCombat() {
      if (!MouseButton.Left.IsDown()) return;

      _actionScheduler.SwitchAction(_fighter);
      currentAttackableTargets.ForEach(_fighter.Attack);
    }
    #endregion ===================================================================================================================================

  }
}
