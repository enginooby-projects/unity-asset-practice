using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using Project.RPG.Combat;
using Enginoobz.Operator;
using SWS;

namespace Project.RPG.Controller {
  public class AIController : MonoBehaviour {
    [SerializeField]
    private Reference _playerRef;

    [SerializeField, HideLabel]
    private AreaCircular _vision = new AreaCircular(label: "Vision", radius: 5, angle: 90);

    [Tooltip("Duration enemy stay in place before return patrolling when lose track of player.")]
    [SerializeField]
    private Vector2Wrapper _suspiciousTime = new Vector2Wrapper(new Vector2(2, 5), 0, 10);

    [Tooltip("Enemy stays triggered duration this duration when get attacked.")]
    [SerializeField]
    private Vector2 _aggravationTime = new Vector2(3f, 5f);

    [Tooltip("When get attacked, notify other enemies within this radius.")]
    [SerializeField]
    private float _notifyDistance = 5f; // TODO: draw Gizmos

    [SerializeField]
    private bool _notifyOnAttack;

    [SerializeField]
    private float _patrolSpeed = 2f;

    [AutoRef, SerializeField, HideInInspector]
    private Fighter _fighter;

    [AutoRef, SerializeField, HideInInspector]
    private navMove _navMover; // ? Create a wrapper for SWS.navMove

    [AutoRef, SerializeField, HideInInspector]
    private NavMeshAgentOperator _agentOpr;

    private bool isPatrolling;
    private bool _isAggravated;

    void Update() {
      if (_vision.Contains(_playerRef) || _isAggravated) {
        HandleAttacking();
      } else {
        HandlePatrolling();
      }
    }

    public void Aggravate() {
      _isAggravated = true;
      StartCoroutine(StopAggravationCoroutine(_aggravationTime.Random()));
    }

    public void NotifyNeighbours() {
      RaycastHit[] hits = Physics.SphereCastAll(transform.position, _notifyDistance, Vector3.up, 0);
      foreach (var hit in hits) {
        if (hit.transform.TryGetComponent<AIController>(out var ai)) {
          ai?.Aggravate();
        }
      }
    }

    private IEnumerator StopAggravationCoroutine(float delay) {
      yield return new WaitForSeconds(delay);
      _isAggravated = false;
    }

    private void HandleAttacking() {
      if (_notifyOnAttack && !_isAggravated) NotifyNeighbours();
      _fighter.Attack(_playerRef);
      isPatrolling = false;
      if (!_navMover.IsPaused()) _navMover.Pause();
    }

    private void HandlePatrolling() {
      _fighter.Cancel();
      if (!isPatrolling) {
        _navMover.ChangeSpeed(_patrolSpeed);
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
