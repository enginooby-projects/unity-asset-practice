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

    [Tooltip("Delay between attacks - Opposite of rate.")]
    [SerializeField, Min(0.5f)] private float _attackCooldown = 1f;

    [Tooltip("Override agent speed when approaching target before attack.")]
    [SerializeField, Min(0.5f)] private float _chaseSpeed = 5f;

    public float ChaseSpeed { get => _chaseSpeed; set { if (value > 0.5f) _chaseSpeed = value; } }

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
    /// <summary>
    /// [Update-safe method]
    /// </summary>
    public void Attack(CombatTarget target) {
      if (_isAttacking && target == _currentTarget) return;

      print("Start attack " + target.name);
      _isAttacking = true;
      _currentTarget = target;
      _isLookingAtCurrentTarget = false;
      _agentOpr.SetSpeed(_chaseSpeed);
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
        if (Time.time - timeSinceLastAttack > _attackCooldown) {
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


    /// <summary>
    /// [Update-safe method]
    /// </summary>
    // TIP: use flag to make update-safe method: method is ok to call in Update() w/o high performance cost
    public void Cancel() {
      if (!_isAttacking) return;

      print("Cancel attack");
      _isAttacking = false;
      _isLookingAtCurrentTarget = false;
      _animator.SetTrigger(stopAttackHash);
    }
  }
}