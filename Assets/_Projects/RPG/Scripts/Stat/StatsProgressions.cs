using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.RPG.Stats {
  /// <summary>
  /// Centralized file of all individual character stats progression
  /// </summary>
  [CreateAssetMenu(fileName = "StatsProgressions", menuName = "Project/RPG/Stats Progressions", order = 0)]
  public class StatsProgressions : SerializedScriptableObject {
    [SerializeField] [InlineEditor] private List<StatsProgressionIndividual> _statsProgressionIndividuals;

    // TODO: Make multi-level Dictionary Serializable
    // [SerializeField, InlineEditor] Dictionary<CharacterType, Dictionary<StatName, List<int>>> _statsProgressions;

    [Button]
    public int? GetStatValue(StatName statName, int level, CharacterType characterType) {
      foreach (var statsProgression in _statsProgressionIndividuals)
        if (statsProgression.CharacterType == characterType) {
          if (level > statsProgression.StatsProgression[statName].Count) return null;
          return statsProgression.StatsProgression[statName][level - 1];
        }

      Debug.LogError("Stat value not set in progression file.");
      return null;
    }

    public List<int> GetStatProgression(StatName statName, CharacterType characterType) {
      foreach (var statsProgression in _statsProgressionIndividuals)
        if (statsProgression.CharacterType == characterType)
          return statsProgression.StatsProgression[statName];

      Debug.LogError("Stat progression not set in progression file.");
      return null; // TODO: handle error
    }
  }
}