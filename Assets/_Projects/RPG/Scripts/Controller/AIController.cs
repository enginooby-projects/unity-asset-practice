using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Project.RPG.Combat;
using Enginoobz.Operator;

namespace Project.RPG.Controller {
  public class AIController : MonoBehaviour {
    [SerializeField] private Reference _playerRef;
    [SerializeField, HideLabel] private AreaCircular _vision = new AreaCircular(label: "Vision", radius: 5, angle: 90);

    [Tooltip("Duration enemy stay in place before returning to guarding point when lose tracking of player.")]
    [SerializeField] private Vector2Wrapper _suspiciousTime = new Vector2Wrapper(new Vector2(2, 5), 0, 10);

    [AutoRef, SerializeField, HideInInspector] private Fighter _fighter;
    [AutoRef, SerializeField, HideInInspector] private NavMeshAgentOperator _agentOpr;

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
        _agentOpr.MoveTo(guardPos, delay: _suspiciousTime.Random);
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
