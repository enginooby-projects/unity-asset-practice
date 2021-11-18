using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Project.RPG.Combat {
  // ? Create Projectile<TargetType> generic (replace CombatTarget)
  public class Projectile : MonoBehaviour {
    [SerializeField] private Transform _target; // ? Constraint target transform by type
    [SerializeField] private float _speed = 10f;
    [SerializeField] private bool _chasingTarget;

    public event Action<CombatTarget> onHitCombatTarget;

    /// <summary>
    /// If not chasing target, then only aim at inital position.
    /// </summary>
    private Vector3 initialTargetPos;
    private PoolObject poolComponent;


    void Start() {
      GetInitialTargerPos();
      poolComponent = GetComponent<PoolObject>();
    }

    private void GetInitialTargerPos() {
      initialTargetPos = _target.GetColliderCenter();
      transform.LookAtY(initialTargetPos);
    }

    void Update() {
      MoveToTarget();
    }

    public void SetTarget(Transform target) {
      _target = target;
      GetInitialTargerPos();
    }

    private void MoveToTarget() {
      // TODO: Paramiterize axis
      if (_chasingTarget)
        transform.LookAtAndMoveY(_target.GetColliderCenter(), distance: _speed);
      else {
        transform.position += transform.up * Time.deltaTime * _speed; // UTIL
      }
    }

    private void OnTriggerEnter(Collider other) {
      print(gameObject.name + " hit " + other.name);
      // TODO: option ignore everything not target type (only realse when hit target)
      if (other.TryGetComponent<CombatTarget>(out CombatTarget combatTarget)) {
        onHitCombatTarget?.Invoke(combatTarget);
      }

      // TIP: reset event to prevent action invokes mutiple times because projectile is reused by pool
      onHitCombatTarget = null;
      poolComponent?.ReleaseToPool();
    }
  }
}
