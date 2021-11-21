using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace Project.RPG.Stats {
  public enum CharacterType {
    Player,
    Grunt,
    Archer,
    Mage,
    Warrior,
  }

  /// <summary>
  /// Centralize all stats for each type of character based on level.
  /// </summary>
  public class CharacterBaseStats : MonoBehaviour {
    [SerializeField]
    private CharacterType _characterType;

    [SerializeField, HideLabel]
    private Stat _levelStat = new Stat(StatName.Level, initialValue: 1);

    [SerializeField]
    private ParticleSystem _levelUpVfx;  // TODO: Implement FXs in Stat

    [SerializeField, InlineEditor]
    private StatsProgressions _statsProgressions;

    public Stat LevelStat => _levelStat;
    public StatsProgressions StatsProgressions => _statsProgressions;

    private void OnEnable() {
      _levelStat.OnStatIncreaseEvent += OnLevelUp;
    }

    private void OnDisable() {
      _levelStat.OnStatIncreaseEvent -= OnLevelUp;
    }

    public Nullable<int> GetStatValue(StatName statName) {
      return _statsProgressions.GetStatValue(statName, _levelStat.CurrentValue, _characterType);
    }

    private void OnLevelUp() {
      _levelUpVfx?.Play();
    }
  }
}
