using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Project.RPG.Combat {
  public class CombatTarget : MonoBehaviour {
    [SerializeField, HideLabel] private Stat healthStat = new Stat("Health", 10);

    private void Start() {
      print(healthStat.CurrentValue);

    }

    public void TakeDamage(int damage) {
      healthStat.Update(-damage);
    }

    public void Die() {
      print("Die");
      Destroy(gameObject);
    }

    public void LogHeath() {
      print(healthStat.CurrentValue);
    }
  }
}
