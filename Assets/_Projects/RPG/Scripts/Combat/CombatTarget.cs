using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Project.RPG.Stats;
using System;

namespace Project.RPG.Combat {
  // ? Rename AttackableTarget implementing IAttackable (TakeDamage, Die)
  public class CombatTarget : MonoBehaviour {
    [SerializeField, HideLabel] private Stat healthStat = new Stat(StatName.Health.ToString(), 10);

    [AutoRef, SerializeField, HideInInspector]
    private CharacterBaseStats _characterBaseStats;

    [AutoRef, SerializeField, HideInInspector]
    private Animator _animator;
    private readonly int getHitHash = Animator.StringToHash("getHit");
    private readonly int dieHash = Animator.StringToHash("die");
    private bool _isDead;

    // TODO: health not update on level changed
    private void OnEnable() {
      if (!_characterBaseStats) _characterBaseStats = GetComponent<CharacterBaseStats>();
      _characterBaseStats.LevelStat.OnStatChangeEvent += UpdateStatsByLevel;
    }

    private void OnDisable() {
      _characterBaseStats.LevelStat.OnStatChangeEvent -= UpdateStatsByLevel;
    }

    private void Start() {
      UpdateStatsByLevel();
    }

    public void UpdateStatsByLevel() {
      int currentHealthValue = _characterBaseStats.GetStatValue(healthStat.statName);
      healthStat.Set(currentHealthValue);
    }

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
