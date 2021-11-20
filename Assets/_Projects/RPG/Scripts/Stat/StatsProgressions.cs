using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace Project.RPG.Stats {
  [CreateAssetMenu(fileName = "StatsProgressions", menuName = "Project/RPG/Stats Progressions", order = 0)]
  /// <summary>
  /// Centralized file of all individual character stats progression
  /// </summary>
  public class StatsProgressions : ScriptableObject {
    [SerializeField, InlineEditor] List<StatsProgressionIndividual> _statsProgressionIndividuals;

    [Button]
    public int GetStatValue(StatName statName, int level, CharacterType characterType) {
      foreach (var statsProgression in _statsProgressionIndividuals) {
        if (statsProgression.CharacterType == characterType) {
          return statsProgression.GetStatValue(statName, level);
        }
      }

      Debug.LogError("Stat value not set in progression file.");
      return 0;
    }
  }
}