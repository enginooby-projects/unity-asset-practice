using System;
using Project.RPG.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.RPG.Combat {
  /// <summary>
  /// Extend Chatacter Fighter to give extra specific play stats related to Attacker (Combat)
  /// </summary>
  public class FighterPlayer : Fighter { // TODO: Create IStatContainer w/ UpdateStats() (for class have any stat need to update)
    [SerializeField, HideLabel]
    private Stat _strengthStat = new Stat(StatName.Strength);

    [AutoRef, SerializeField, HideInInspector]
    private CharacterBaseStats _characterBaseStats;
    public Stat StrengthStat => _strengthStat;

    protected override int AttackDamage => base.AttackDamage + _strengthStat.CurrentValue;

    protected override void Start() {
      base.Start();
      UpdateStatsByLevel();
    }

    protected override void OnEnable() {
      base.OnEnable();
      if (!_characterBaseStats) _characterBaseStats = GetComponent<CharacterBaseStats>();
      _characterBaseStats.LevelStat.OnStatChangeEvent += UpdateStatsByLevel;
    }

    protected override void OnDisable() {
      base.OnEnable();
      _characterBaseStats.LevelStat.OnStatChangeEvent -= UpdateStatsByLevel;
    }

    public void UpdateStatsByLevel() {
      Nullable<int> currentStrength = _characterBaseStats.GetStatValue(_strengthStat.statName.ToEnumString<StatName>());
      if (currentStrength.HasValue) {
        _strengthStat.Set(currentStrength.Value);
      }
    }
  }
}
