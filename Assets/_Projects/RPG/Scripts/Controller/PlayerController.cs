using System.Collections.Generic;
using UnityEngine;
using Enginooby.Operator;
using Enginooby.UI;
using Project.RPG.Combat;
using Sirenix.OdinInspector;
using static RayUtils;
using UnityEngine.EventSystems;

namespace Project.RPG.Controller {
  [RequireComponent(typeof(NavMeshAgentOperator))]
  /// <summary>
  /// Point and click controller.
  /// </summary>
  public class PlayerController : MonoBehaviour {
    [SerializeField, InlineEditor, LabelText("Cursor Preset")]
    private CursorDataPreset _cursor;

    [SerializeField, HideInInspector] private Enginooby.Core.ActionScheduler _actionScheduler;

    // ! Set Event mask on Physics Raycaster to UI layer
    private bool CursorAtUI => EventSystem.current.IsPointerOverGameObject();

    private void Awake() {
      _actionScheduler = GetComponent<Enginooby.Core.ActionScheduler>();
      _mover = GetComponent<NavMeshAgentOperator>();
      _fighter = GetComponent<Fighter>();
    }

    private void Start() {
      _cursor?.Init();
    }

    void Update() {
      if (CursorAtUI) {
        _cursor?.Set(CursorName.UI);
      }
      else if (CursorAtAttacker) {
        HandleCombat();
        _cursor?.Set(CursorName.Attack);
      }
      else if (CanMoveToCursor) {
        HandleMovement();
        _cursor?.Set(CursorName.Move);
      }
      else {
        _cursor?.Set(CursorName.None);
      }
    }

    #region MOVEMENT ===================================================================================================================================

    [SerializeField, HideInInspector] private NavMeshAgentOperator _mover;
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

    [SerializeField, HideInInspector] private Fighter _fighter;
    private List<Attackable> currentAttackableTargets = new List<Attackable>();

    private bool CursorAtAttacker {
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