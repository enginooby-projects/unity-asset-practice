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
  public class StatsProgressions : SerializedScriptableObject {
    [SerializeField, InlineEditor] List<StatsProgressionIndividual> _statsProgressionIndividuals;

    // TODO: Make multi-level Dictionay Serializable
    // [SerializeField, InlineEditor] Dictionary<CharacterType, Dictionary<StatName, List<int>>> _statsProgressions;

    [Button]
    public int GetStatValue(StatName statName, int level, CharacterType characterType) {
      foreach (var statsProgression in _statsProgressionIndividuals) {
        if (statsProgression.CharacterType == characterType) {
          return statsProgression.StatsProgression[statName][level - 1];
        }
      }

      Debug.LogError("Stat value not set in progression file.");
      return 0;
    }

    public List<int> GetStatProgression(StatName statName, CharacterType characterType) {
      foreach (var statsProgression in _statsProgressionIndividuals) {
        if (statsProgression.CharacterType == characterType) {
          return statsProgression.StatsProgression[statName];
        }
      }

      Debug.LogError("Stat progression not set in progression file.");
      return null; // TODO: handle error
    }
  }
}