using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Project.RPG.Stats;
using System;

namespace Project.RPG.Combat {
  // ? Rename AttackableTarget implementing IAttackable (TakeDamage, Die)
  public class CombatTarget : Attackable {
    [SerializeField, HideLabel]
    private Stat healthStat = new Stat(StatName.Health, 10);

    [AutoRef, SerializeField, HideInInspector]
    private CharacterBaseStats _characterBaseStats;

    [AutoRef, SerializeField, HideInInspector]
    private Animator _animator;
    private readonly int getHitHash = Animator.StringToHash("getHit");
    private readonly int dieHash = Animator.StringToHash("die");

    private bool _isDead;
    private Attacker _lastAttacker;
    private int _experienceReward = 10;


    private void Start() {
      UpdateStatsByLevel();
    }

    public override void GetAttacked(Attacker attacker, int damage) {
      // print(attacker.name + " attacked " + name);
      _lastAttacker = attacker;
      healthStat.Update(-damage);
      if (!_isDead) _animator.SetTrigger(getHitHash);
    }

    public override void Die() {
      _isDead = true;
      _animator.SetTrigger(dieHash);
      if (_lastAttacker.TryGetComponent<PlayerStats>(out PlayerStats playerStats)) {
        playerStats.ExperienceStat.Add(_experienceReward);
      }
      Destroy(this);
    }

    public void LogHeath() {
      print(healthStat.CurrentValue);
    }

    #region STAT
    private void OnEnable() {
      if (!_characterBaseStats) _characterBaseStats = GetComponent<CharacterBaseStats>();
      healthStat.enableMax = true;
      _characterBaseStats.LevelStat.OnStatChangeEvent += UpdateStatsByLevel;
    }

    private void OnDisable() {
      _characterBaseStats.LevelStat.OnStatChangeEvent -= UpdateStatsByLevel;
    }

    public void UpdateStatsByLevel() {
      int currentHealthValue = _characterBaseStats.GetStatValue(healthStat.statName.ToEnumString<StatName>());
      healthStat.Set(currentHealthValue);
      healthStat.MaxValue = currentHealthValue;

      if (!gameObject.CompareTag("Player")) { // REFACTOR
        _experienceReward = _characterBaseStats.GetStatValue(StatName.ExperienceReward);
      }
    }
    #endregion
  }
}
