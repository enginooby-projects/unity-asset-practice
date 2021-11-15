using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Project.RPG.Combat {
  public class CombatTarget : MonoBehaviour {
    [SerializeField, HideLabel] private Stat healthStat = new Stat("Health", 10);

    [AutoRef, SerializeField, HideInInspector]
    private Animator _animator;
    private readonly int getHitHash = Animator.StringToHash("getHit");
    private readonly int dieHash = Animator.StringToHash("die");
    private bool _isDead;

    public void TakeDamage(int damage) {
      healthStat.Update(-damage);
      if (!_isDead) _animator.SetTrigger(getHitHash);
    }

    public void Die() {
      _isDead = true;
      _animator.SetTrigger(dieHash);
      Destroy(this);
    }

    public void LogHeath() {
      print(healthStat.CurrentValue);
    }
  }
}
