using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Project.RPG.Stats {
  public enum CharacterType { Player, Grunt, Archer, Mage }

  /// <summary>
  /// Centralize all stats for each type of character based on level.
  /// </summary>
  public class CharacterBaseStats : MonoBehaviour {
    [SerializeField] private CharacterType _characterType;
    [SerializeField, HideLabel] private Stat _levelStat = new Stat(StatName.Level.ToString(), initialValue: 1);
    [SerializeField, InlineEditor] private StatsProgressions _statsProgressions;

    public Stat LevelStat => _levelStat;

    // TODO: using enum string for stat name
    public int GetStatValue(string statName) {
      return _statsProgressions.GetStatValue(statName, _levelStat.CurrentValue, _characterType);
    }
  }
}
