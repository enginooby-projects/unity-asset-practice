using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Enginoobz.Operator;
using Enginoobz.Core;
using Sirenix.OdinInspector;

namespace Project.RPG.Combat {
  public class Fighter : MonoBehaviour, IAction {
    [Tooltip("Move to target and stop at this distance to attack.")]
    [SerializeField] private float _attackRange = 2;
    [SerializeField] private int _attackDamage = 1;
    [SerializeField, Min(0.5f)] private float _attackRate = 1f; // TODO: Implement rate not delay

    [AutoRef, SerializeField, HideInInspector]
    private NavMeshAgentOperator _agentOpr;

    [AutoRef, SerializeField, HideInInspector]
    private Animator _animator;
    private readonly int attackHash = Animator.StringToHash("attack");
    private readonly int stopAttackHash = Animator.StringToHash("stopAttack");
    private readonly int isTurning = Animator.StringToHash("isTurning");

    private CombatTarget _currentTarget;
    private bool _isAttacking;
    private float timeSinceLastAttack;

    private bool _isLookingAtCurrentTarget;
    private bool _isTurningToCurrentTarget;

    private void Update() {
      if (_isAttacking && _currentTarget) ApproachAndAttackCurrentTarget();
    }

    // ? Rename to set target
    public void Attack(CombatTarget target) {
      if (_isAttacking && target == _currentTarget) return;

      print("Start attack " + target.name);
      _isAttacking = true;
      _currentTarget = target;
      _isLookingAtCurrentTarget = false;
      _animator.ResetTrigger(stopAttackHash);
    }

    public void Attack(Reference targetRef) {
      if (targetRef.GameObject.TryGetComponent<CombatTarget>(out CombatTarget target)) {
        Attack(target);
      }
    }

    public void ApproachAndAttackCurrentTarget() {
      if (!transform.IsInRange(_currentTarget.transform, _attackRange)) {
        // FIX: agent does not guarantee to arrive at the range (e.g target is on the air)
        _agentOpr.MoveTo(_currentTarget.transform.position, _attackRange);
        _isLookingAtCurrentTarget = true; // agent auto turn towards to the destination (target)
      } else {
        if (Time.time - timeSinceLastAttack > _attackRate) {
          LookAtAndAttackCurrentTarget();
          timeSinceLastAttack = Time.time;
        }
      }
    }

    private void LookAtAndAttackCurrentTarget() {
      if (!_isLookingAtCurrentTarget && !_isTurningToCurrentTarget) {
        TurnToCurrentTarget();
      } else
        AttackCurrentTarget();
    }

    private void AttackCurrentTarget() {
      _animator.SetTrigger(attackHash);
    }

    private void TurnToCurrentTarget() {
      _isTurningToCurrentTarget = true;
      _animator.SetBool(isTurning, true);
      transform.DOLookAt(_currentTarget.transform.position, 400)
              .SetSpeedBased()
              .OnComplete(() => {
                _isLookingAtCurrentTarget = true;
                _isTurningToCurrentTarget = false;
                _animator.SetBool(isTurning, false);
                AttackCurrentTarget();
              });
    }

    // animation events
    void OnHit() {
      _currentTarget?.TakeDamage(_attackDamage);
      if (!_currentTarget) _isAttacking = false; // target dead
    }

    public void Cancel() {
      if (!_isAttacking) return;

      print("Cancel attack");
      _isAttacking = false;
      _isLookingAtCurrentTarget = false;
      _animator.SetTrigger(stopAttackHash);
    }
  }
}