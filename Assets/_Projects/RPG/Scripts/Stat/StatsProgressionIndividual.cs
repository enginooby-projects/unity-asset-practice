using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

namespace Project.RPG.Stats {
  [CreateAssetMenu(fileName = "StatsProgression", menuName = "Project/RPG/Stats Progression Individual", order = 0)]
  /// <summary>
  /// Centralized file of all stats for individual type of character based on level.
  /// </summary>
  class StatsProgressionIndividual : SerializedScriptableObject {
    // TODO: multiply factor for each stats (game difficulity)
    [HideLabel, EnumToggleButtons, SerializeField]
    private CharacterType _characterType;

    [SerializeField] private Dictionary<StatName, List<int>> _statsProgression = new Dictionary<StatName, List<int>>();

    public CharacterType CharacterType => _characterType;
    public Dictionary<StatName, List<int>> StatsProgression => _statsProgression;
  }
}