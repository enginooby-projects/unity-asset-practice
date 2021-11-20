using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Project.RPG.Stats {
  /// <summary>
  /// Stats that belong specific to player, not other characters.
  /// </summary>
  public class PlayerStats : MonoBehaviour {
    [SerializeField, HideLabel] private Stat _xpStat = new Stat(StatName.Experience.ToString());
  }
}