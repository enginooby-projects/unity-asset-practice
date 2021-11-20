using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Project.RPG.Stats {
  /// <summary>
  /// Stats that belong specific to player, not other characters.
  /// </summary>
  public class PlayerStats : MonoBehaviour {
    [SerializeField, HideLabel]
    private Stat _experienceStat = new Stat(StatName.Experience);

    [AutoRef, SerializeField, HideInInspector]
    private CharacterBaseStats _characterBaseStats;
    private Stat _experienceToLevel = new Stat(StatName.ExperienceToLevel);
    private List<int> _requiredXpOnLevel = new List<int>();

    public Stat ExperienceStat => _experienceStat;

    private void Start() {
      _requiredXpOnLevel = _characterBaseStats.StatsProgressions.GetStatProgression(StatName.ExperienceToLevel, CharacterType.Player);
      UpdateLevel();
    }

    private void OnEnable() {
      _experienceStat.OnStatChangeEvent += UpdateLevel;
    }

    private void OnDisable() {
      _experienceStat.OnStatChangeEvent -= UpdateLevel;
    }

    public void UpdateLevel() {
      // print("Calculating level based on total XP...");
      int currentLevel = _characterBaseStats.LevelStat.CurrentValue;
      int maxLevel = _requiredXpOnLevel.Count;
      int currentXp = _experienceStat.CurrentValue;

      // TODO: Fix index offset
      for (int i = currentLevel - 1; i < maxLevel; i++) {
        // print("Require " + _requiredXpOnLevel[i] + " for level " + (i + 1));
        if (currentXp < _requiredXpOnLevel[i]) {
          _characterBaseStats.LevelStat.Set(i + 1);
          return;
        }
      }

      _characterBaseStats.LevelStat.Set(maxLevel + 1);
    }
  }
}