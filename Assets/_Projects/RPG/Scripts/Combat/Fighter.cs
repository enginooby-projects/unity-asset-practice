using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Enginoobz.Operator;
using Enginoobz.Core;

namespace Project.RPG.Combat {
  public class Fighter : MonoBehaviour, IAction {
    [Tooltip("Move to target and stop at this distance to attack.")]
    [SerializeField] private float _attackRange = 2;

    [AutoRef, SerializeField, HideInInspector]
    private NavMeshAgentOperator _agentOpr;

    [AutoRef, SerializeField, HideInInspector]
    private Animator _animator;
    private readonly int attackHash = Animator.StringToHash("attack");

    private CombatTarget _currentTarget;
    private bool _isAttacking;

    private void Update() {
      if (_isAttacking) AttackCurrentTarget();
    }

    public void Attack(CombatTarget target) {
      _isAttacking = true;
      _currentTarget = target;
    }

    public void AttackCurrentTarget() {
      if (transform.DistanceFrom(_currentTarget.transform) > _attackRange) {
        // FIX: agent does not guarantee to arrive at the range (e.g target is on the air)
        _agentOpr.MoveTo(_currentTarget.transform.position, _attackRange);
      } else {
        _animator.SetTrigger(attackHash);
        // transform.DOLookAt(_currentTarget.transform.position, 1).OnComplete(() => {
        //   // print("Attacking " + _currentTarget.name);
        //   // _animator.SetBool(_isAttackingHash, true);
        // });
      }
    }

    // animation events
    void OnHit() {
      print("On Hit");
    }

    public void Cancel() {
      _isAttacking = false;
    }
  }
}