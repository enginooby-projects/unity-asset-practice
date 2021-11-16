using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Project.RPG.Combat;
using Enginoobz.Operator;
using SWS;

namespace Project.RPG.Controller {
  public class AIController : MonoBehaviour {
    [SerializeField] private Reference _playerRef;
    [SerializeField, HideLabel] private AreaCircular _vision = new AreaCircular(label: "Vision", radius: 5, angle: 90);

    [Tooltip("Duration enemy stay in place before return patrolling when lose track of player.")]
    [SerializeField] private Vector2Wrapper _suspiciousTime = new Vector2Wrapper(new Vector2(2, 5), 0, 10);

    [AutoRef, SerializeField, HideInInspector] private Fighter _fighter;
    [AutoRef, SerializeField, HideInInspector] private navMove _navMover; // ? Create a wrapper for SWS.navMove
    [AutoRef, SerializeField, HideInInspector] private NavMeshAgentOperator _agentOpr;

    private bool isPatrolling;

    void Update() {
      if (_vision.Contains(_playerRef)) {
        _fighter.Attack(_playerRef);
        isPatrolling = false;
        _navMover.Pause();
      } else {
        HandlePatrolling();
      }
    }

    private void HandlePatrolling() {
      _fighter.Cancel();
      if (!isPatrolling) {
        _navMover.Resume();
        isPatrolling = true;
      }
    }

    public void OnDead() {
      _navMover.Stop();
      _fighter.Cancel();
      Destroy(this);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
      _vision.DrawGizmos();
    }
  }
#endif
}
