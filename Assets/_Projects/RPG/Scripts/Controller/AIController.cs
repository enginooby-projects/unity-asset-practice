using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Project.RPG.Combat;
using Enginoobz.Operator;

namespace Project.RPG.Controller {
  public class AIController : MonoBehaviour {
    [AutoRef, SerializeField, HideInInspector] private Fighter _fighter;
    [AutoRef, SerializeField, HideInInspector] private NavMeshAgentOperator _agentOpr;
    [SerializeField] private Reference _playerRef;
    [SerializeField, HideLabel] private AreaCircular _vision = new AreaCircular(label: "Vision", radius: 5, angle: 90);

    private Vector3 guardPos;
    private bool isReturningToGuardPos;

    private void Start() {
      guardPos = transform.position;
    }

    void Update() {
      if (_vision.Contains(_playerRef)) {
        _fighter.Attack(_playerRef);
        isReturningToGuardPos = false;
      } else {
        HandleGuarding();
      }
    }

    private void HandleGuarding() {
      _fighter.Cancel();
      if (!isReturningToGuardPos) {
        _agentOpr.MoveTo(guardPos);
        isReturningToGuardPos = true;
      }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
      _vision.DrawGizmos();
    }
  }
#endif
}
