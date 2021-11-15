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
    private CombatTarget _target;
    private bool _isAttacking;

    private void Update() {
      if (_isAttacking) Attack(_target);
    }

    public void Attack(CombatTarget target) {
      _isAttacking = true;
      if (transform.DistanceFrom(target.transform) > _attackRange) {
        // FIX: agent does not guarantee to arrive at the range (e.g target is on the air)
        _agentOpr.MoveTo(target.transform.position, _attackRange);
      } else {
        transform.DOLookAt(target.transform.position, 1);
      }

      // TODO: Only attack after finish movement & in attack range
      print("Attacking " + target.name);
    }

    public void Cancel() {
      _isAttacking = false;
    }
  }
}