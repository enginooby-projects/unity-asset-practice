using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enginoobz.Operator;
using DG.Tweening;

namespace Project.RPG.Combat {
  public class Fighter : MonoBehaviour {
    [Tooltip("Move to target and stop at this distance to attack.")]
    [SerializeField] private float _attackRange = 2;

    [AutoRef, SerializeField, HideInInspector]
    private NavMeshAgentOperator _agentOpr;

    public void Attack(CombatTarget target) {
      if (transform.DistanceFrom(target.transform) > _attackRange) {
        // FIX: agent does not guarantee to arrive at the range (e.g target is on the air)
        _agentOpr.MoveTo(target.transform.position, _attackRange);
      } else {
        transform.DOLookAt(target.transform.position, 1);
      }

      // TODO: Only attack after finish movement & in attack range
      print("Attacking " + target.name);
    }
  }
}