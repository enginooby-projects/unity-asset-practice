using UnityEngine;
using Sirenix.OdinInspector;
using Project.RPG.Stats;
using System;
using TMPro;
using Enginoobz.Audio;

namespace Project.RPG.Combat {
  // ? Rename AttackableTarget implementing IAttackable (TakeDamage, Die)
  public class CombatTarget : Attackable {
    [SerializeField, HideLabel] private Stat healthStat = new Stat(StatName.Health, 10);

    [SerializeField] private Spawner _damageLabelSpawner;

    [SerializeField] private AudioSourceOperator _audioSourceOpt;

    // [AutoRef]
    [SerializeField, HideInInspector] private CharacterBaseStats _characterBaseStats;

    // [AutoRef]
    [SerializeField, HideInInspector] private Animator _animator;
    private readonly int getHitHash = Animator.StringToHash("getHit");
    private readonly int dieHash = Animator.StringToHash("die");

    private Attacker _lastAttacker;
    private int _experienceReward = 10;

    private void Awake() {
      _characterBaseStats = GetComponent<CharacterBaseStats>();
      _animator = GetComponent<Animator>();
    }

    private void Start() {
      UpdateStatsByLevel();
    }

    public override void GetAttacked(Attacker attacker, int damage) {
      // print(attacker.name + " attacked " + name + " - damage: " + damage);
      _lastAttacker = attacker;
      healthStat.Add(-damage);
      // OPTIM: cache?
      _damageLabelSpawner.Spawn().ForEach(label => {
        var damageTMP = label.GetComponent<TextMeshProUGUI>();
        damageTMP.text = damage.ToString();
      });

      if (!_isDead) {
        _animator.SetTrigger(getHitHash);
        _audioSourceOpt?.Play(SFXAction.Damaged);
      }
    }

    public override void Die() {
      _isDead = true;
      _animator.SetTrigger(dieHash);
      if (_lastAttacker.TryGetComponent<PlayerStats>(out PlayerStats playerStats)) {
        playerStats.ExperienceStat.Add(_experienceReward);
      }

      _audioSourceOpt?.Play(SFXAction.Die);
      Destroy(this);
    }

    public void LogHeath() {
      // print(name + " health: " + healthStat.CurrentValue);
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
      Nullable<int> currentHealthValue = _characterBaseStats.GetStatValue(healthStat.statName.ToEnumString<StatName>());
      if (currentHealthValue.HasValue) {
        healthStat.Set(currentHealthValue.Value);
        healthStat.MaxValue = currentHealthValue.Value;
      }

      if (!gameObject.CompareTag("Player")) {
        // REFACTOR
        _experienceReward = _characterBaseStats.GetStatValue(StatName.ExperienceReward).Value;
      }
    }

    #endregion
  }
}