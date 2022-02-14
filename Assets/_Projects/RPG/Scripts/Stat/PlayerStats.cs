using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Project.RPG.Stats {
  /// <summary>
  /// Stats that belong specific to player, not other characters.
  /// </summary>
  // ? Merge into FighterPlayer
  public class PlayerStats : MonoBehaviour {
    [SerializeField, HideLabel] private Stat _experienceStat = new Stat(StatName.Experience);


    private Stat _experienceToLevel = new Stat(StatName.ExperienceToLevel);

    [SerializeField, HideInInspector] private CharacterBaseStats _characterBaseStats;
    private List<int> _requiredXpOnLevel = new List<int>();

    public Stat ExperienceStat => _experienceStat;


    private void Start() {
      _characterBaseStats = GetComponent<CharacterBaseStats>();
      _requiredXpOnLevel =
        _characterBaseStats.StatsProgressions.GetStatProgression(StatName.ExperienceToLevel, CharacterType.Player);
      UpdateLevel();
    }

    private void OnEnable() {
      _experienceStat.OnStatChangeEvent += UpdateLevel;
    }

    private void OnDisable() {
      _experienceStat.OnStatChangeEvent -= UpdateLevel;
    }

    public void UpdateLevel() {
      int currentLevel = _characterBaseStats.LevelStat.CurrentValue;
      int maxLevel = _requiredXpOnLevel.Count;
      int currentXp = _experienceStat.CurrentValue;

      // TODO: Fix index offset
      for (int i = currentLevel; i < maxLevel; i++) {
        // print("Require " + _requiredXpOnLevel[i] + " for level " + (i + 1));
        if (currentXp < _requiredXpOnLevel[i]) {
          _characterBaseStats.LevelStat.Set(i);
          return;
        }
      }

      _characterBaseStats.LevelStat.Set(maxLevel);
    }
  }
}